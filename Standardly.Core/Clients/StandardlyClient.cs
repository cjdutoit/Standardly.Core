// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Executions;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Brokers.RegularExpressions;
using Standardly.Core.Models.Clients.Exceptions;
using Standardly.Core.Models.Configurations.Retries;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.Templates;
using Standardly.Core.Models.Orchestrations.Templates.Exceptions;
using Standardly.Core.Services.Foundations.Executions;
using Standardly.Core.Services.Foundations.Files;
using Standardly.Core.Services.Foundations.Templates;
using Standardly.Core.Services.Orchestrations.Templates;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;
using Standardly.Core.Services.Processings.Templates;
using Xeptions;

namespace Standardly.Core.Clients
{
    public class StandardlyClient : IStandardlyClient
    {
        public event Action<DateTimeOffset, string, string> LogRaised = delegate { };
        private readonly ITemplateOrchestrationService templateOrchestrationService;

        public StandardlyClient()
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            string templateFolderPath = Path.Combine(Path.GetDirectoryName(assembly), @"Templates");
            string templateDefinitionFileName = "Template.json";
            ILoggingBroker loggingBroker = null;

            this.templateOrchestrationService =
                this.InitialiseClient(templateFolderPath, templateDefinitionFileName, loggingBroker);

            this.LogEventSetup();
        }

        public StandardlyClient(ILoggingBroker loggingBroker)
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            string templateFolderPath = Path.Combine(Path.GetDirectoryName(assembly), @"Templates");
            string templateDefinitionFileName = "Template.json";

            this.templateOrchestrationService =
                this.InitialiseClient(templateFolderPath, templateDefinitionFileName, loggingBroker);
        }

        public StandardlyClient(
            string templateFolderPath,
            string templateDefinitionFileName)
        {
            ILoggingBroker loggingBroker = null;

            this.templateOrchestrationService =
                this.InitialiseClient(templateFolderPath, templateDefinitionFileName, loggingBroker);
        }

        public StandardlyClient(
            string templateFolderPath,
            string templateDefinitionFileName,
            ILoggingBroker loggingBroker)
        {
            this.templateOrchestrationService =
                this.InitialiseClient(templateFolderPath, templateDefinitionFileName, loggingBroker);
        }

        public async ValueTask<List<Template>> FindAllTemplatesAsync()
        {
            try
            {
                return await this.templateOrchestrationService.FindAllTemplatesAsync();
            }
            catch (TemplateOrchestrationValidationException templateOrchestrationValidationException)
            {
                throw new StandardlyClientValidationException(
                    templateOrchestrationValidationException.InnerException as Xeption);
            }
            catch (TemplateOrchestrationDependencyValidationException
                templateOrchestrationDependencyValidationException)
            {
                throw new StandardlyClientValidationException(
                    templateOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (TemplateOrchestrationDependencyException
                templateOrchestrationDependencyException)
            {
                throw new StandardlyClientDependencyException(
                    templateOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (TemplateOrchestrationServiceException
                templateOrchestrationServiceException)
            {
                throw new StandardlyClientServiceException(
                    templateOrchestrationServiceException.InnerException as Xeption);
            }
        }

        private void LogEventSetup() =>
            this.templateOrchestrationService.LogRaised += this.LogRaised;

        private ITemplateOrchestrationService InitialiseClient(
            string templateFolderPath,
            string templateDefinitionFileName,
            ILoggingBroker loggingBroker)
        {
            var fileProcessingService = new FileProcessingService(
                fileService: new FileService(
                    fileBroker: new FileBroker(),
                    retryConfig: new RetryConfig(),
                    loggingBroker: loggingBroker),
                loggingBroker: loggingBroker);

            var executionProcessingService = new ExecutionProcessingService(
                executionService: new ExecutionService(
                    executionBroker: new ExecutionBroker(),
                    loggingBroker: loggingBroker),
                loggingBroker: loggingBroker);

            var templateProcessingService = new TemplateProcessingService(
                templateService: new TemplateService(
                    fileBroker: new FileBroker(),
                    regularExpressionBroker: new RegularExpressionBroker(),
                    loggingBroker: loggingBroker),
                loggingBroker: loggingBroker);

            var templateConfig = new TemplateConfig(templateFolderPath, templateDefinitionFileName);

            return new TemplateOrchestrationService(
                fileProcessingService,
                executionProcessingService,
                templateProcessingService,
                templateConfig,
                loggingBroker);
        }
    }
}
