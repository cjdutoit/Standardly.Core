// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Standardly.Core.Brokers.Executions;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.RegularExpressions;
using Standardly.Core.Models.Clients.Exceptions;
using Standardly.Core.Models.Configurations.Retries;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.TemplateGenerations.Exceptions;
using Standardly.Core.Models.Orchestrations.Templates.Exceptions;
using Standardly.Core.Services.Foundations.Executions;
using Standardly.Core.Services.Foundations.Files;
using Standardly.Core.Services.Foundations.Templates;
using Standardly.Core.Services.Orchestrations.TemplateRetrievals;
using Standardly.Core.Services.Processings.Executions;
using Standardly.Core.Services.Processings.Files;
using Standardly.Core.Services.Processings.Templates;
using Xeptions;

namespace Standardly.Core.Clients
{
    public class StandardlyTemplateClient : IStandardlyTemplateClient
    {
        private readonly ITemplateRetrievalOrchestrationService templateRetrievalOrchestrationService;

        public StandardlyTemplateClient()
        {
            this.templateRetrievalOrchestrationService =
                this.InitialiseClient();
        }

        public async ValueTask<List<Template>> FindAllTemplatesAsync()
        {
            try
            {
                string assembly = Assembly.GetExecutingAssembly().Location;
                string templateFolderPath = Path.Combine(Path.GetDirectoryName(assembly), @"Templates");
                string templateDefinitionFileName = "Template.json";

                return await this.templateRetrievalOrchestrationService
                    .FindAllTemplatesAsync(templateFolderPath, templateDefinitionFileName);
            }
            catch (TemplateGenerationOrchestrationValidationException templateOrchestrationValidationException)
            {
                throw new StandardlyClientValidationException(
                    templateOrchestrationValidationException.InnerException as Xeption);
            }
            catch (TemplateGenerationOrchestrationDependencyValidationException
                templateOrchestrationDependencyValidationException)
            {
                throw new StandardlyClientValidationException(
                    templateOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (TemplateGenerationOrchestrationDependencyException
                templateOrchestrationDependencyException)
            {
                throw new StandardlyClientDependencyException(
                    templateOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (TemplateGenerationOrchestrationServiceException
                templateOrchestrationServiceException)
            {
                throw new StandardlyClientServiceException(
                    templateOrchestrationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<List<Template>> FindAllTemplatesAsync(string templateFolderPath, string templateDefinitionFileName)
        {
            try
            {
                return await this.templateRetrievalOrchestrationService
                    .FindAllTemplatesAsync(templateFolderPath, templateDefinitionFileName);
            }
            catch (TemplateGenerationOrchestrationValidationException templateOrchestrationValidationException)
            {
                throw new StandardlyClientValidationException(
                    templateOrchestrationValidationException.InnerException as Xeption);
            }
            catch (TemplateGenerationOrchestrationDependencyValidationException
                templateOrchestrationDependencyValidationException)
            {
                throw new StandardlyClientValidationException(
                    templateOrchestrationDependencyValidationException.InnerException as Xeption);
            }
            catch (TemplateGenerationOrchestrationDependencyException
                templateOrchestrationDependencyException)
            {
                throw new StandardlyClientDependencyException(
                    templateOrchestrationDependencyException.InnerException as Xeption);
            }
            catch (TemplateGenerationOrchestrationServiceException
                templateOrchestrationServiceException)
            {
                throw new StandardlyClientServiceException(
                    templateOrchestrationServiceException.InnerException as Xeption);
            }
        }

        private ITemplateRetrievalOrchestrationService InitialiseClient()
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

            return new TemplateRetrievalOrchestrationService(
                fileProcessingService,
                templateProcessingService);
        }
    }
}
