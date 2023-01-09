// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Executions;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.RegularExpressions;
using Standardly.Core.Models.Clients.Exceptions;
using Standardly.Core.Models.Configurations.Retries;
using Standardly.Core.Models.Events;
using Standardly.Core.Models.Services.Coordinations.TemplateGenerations.Exceptions;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations;
using Standardly.Core.Services.Coordinations.TemplatesGenerations;
using Standardly.Core.Services.Foundations.Executions;
using Standardly.Core.Services.Foundations.Files;
using Standardly.Core.Services.Foundations.Templates;
using Standardly.Core.Services.Orchestrations.TemplatesGenerations;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;
using Standardly.Core.Services.Processings.Templates;
using Xeptions;

namespace Standardly.Core.Clients
{
    public class StandardlyGenerationClient : IStandardlyGenerationClient
    {
        public event EventHandler<ProcessedEventArgs> Processed;
        private readonly ITemplateGenerationCoordinationService templateGenerationOrchestrationService;

        public StandardlyGenerationClient()
        {
            this.templateGenerationOrchestrationService =
                this.InitialiseClient();

            this.LogEventSetup();
        }

        public async ValueTask GenerateCodeAsync(TemplateGenerationInfo templateGenerationInfo)
        {
            try
            {
                await this.templateGenerationOrchestrationService.GenerateCodeAsync(templateGenerationInfo);
            }
            catch (TemplateGenerationCoordinationValidationException templateOrchestrationValidationException)
            {
                throw new StandardlyClientValidationException(
                    templateOrchestrationValidationException.InnerException as Xeption);
            }
            catch (TemplateGenerationCoordinationDependencyValidationException
                templateOrchestrationDependencyValidationException)
            {
                throw new StandardlyClientValidationException(
                    templateOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (TemplateGenerationCoordinationDependencyException
                templateOrchestrationDependencyException)
            {
                throw new StandardlyClientDependencyException(
                    templateOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (TemplateGenerationCoordinationServiceException
                templateOrchestrationServiceException)
            {
                throw new StandardlyClientServiceException(
                    templateOrchestrationServiceException.InnerException as Xeption);
            }
        }

        private void LogEventSetup()
        {
            this.templateGenerationOrchestrationService.Processed += ItemProcessed;
        }

        private ITemplateGenerationCoordinationService InitialiseClient()
        {
            var fileProcessingService = new FileProcessingService(
                fileService: new FileService(
                    fileBroker: new FileBroker(),
                    retryConfig: new RetryConfig()));

            var executionProcessingService = new ExecutionProcessingService(
                executionService: new ExecutionService(
                    executionBroker: new ExecutionBroker()));

            var templateProcessingService = new TemplateProcessingService(
                templateService: new TemplateService(
                    fileBroker: new FileBroker(),
                    regularExpressionBroker: new RegularExpressionBroker()));

            return new TemplateGenerationCoordinationService(
                fileProcessingService,
                executionProcessingService,
                templateProcessingService);
        }

        private void ItemProcessed(object sender, ProcessedEventArgs e)
        {
            OnProcessed(e);
        }

        protected virtual void OnProcessed(ProcessedEventArgs e)
        {
            EventHandler<ProcessedEventArgs> handler = Processed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
