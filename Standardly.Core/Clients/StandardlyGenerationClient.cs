// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Standardly.Core.Brokers.Executions;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Brokers.RegularExpressions;
using Standardly.Core.Models.Clients.Exceptions;
using Standardly.Core.Models.Configurations.Retries;
using Standardly.Core.Models.Foundations.Templates;
using Standardly.Core.Models.Orchestrations.TemplateGenerations;
using Standardly.Core.Models.Orchestrations.TemplateGenerations.Exceptions;
using Standardly.Core.Models.Orchestrations.Templates.Exceptions;
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
        public event Action<DateTimeOffset, string, string> LogRaised = delegate { };
        private readonly ITemplateGenerationOrchestrationService templateGenerationOrchestrationService;

        public StandardlyGenerationClient()
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            string templateFolderPath = Path.Combine(Path.GetDirectoryName(assembly), @"Templates");
            string templateDefinitionFileName = "Template.json";
            ILoggingBroker loggingBroker = this.InitialiseLogger();

            this.templateGenerationOrchestrationService =
                this.InitialiseClient(templateFolderPath, templateDefinitionFileName, loggingBroker);

            this.LogEventSetup();
        }

        public StandardlyGenerationClient(ILoggingBroker loggingBroker)
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            string templateFolderPath = Path.Combine(Path.GetDirectoryName(assembly), @"Templates");
            string templateDefinitionFileName = "Template.json";

            this.templateGenerationOrchestrationService =
                this.InitialiseClient(templateFolderPath, templateDefinitionFileName, loggingBroker);

            this.LogEventSetup();
        }

        public StandardlyGenerationClient(
            string templateFolderPath,
            string templateDefinitionFileName)
        {
            ILoggingBroker loggingBroker = this.InitialiseLogger();

            this.templateGenerationOrchestrationService =
                this.InitialiseClient(templateFolderPath, templateDefinitionFileName, loggingBroker);

            this.LogEventSetup();
        }

        public StandardlyGenerationClient(
            string templateFolderPath,
            string templateDefinitionFileName,
            ILoggingBroker loggingBroker)
        {
            this.templateGenerationOrchestrationService =
                this.InitialiseClient(templateFolderPath, templateDefinitionFileName, loggingBroker);

            this.LogEventSetup();
        }

        public bool ScriptExecutionIsEnabled
        {
            get { return this.templateGenerationOrchestrationService.ScriptExecutionIsEnabled; }
            set { this.templateGenerationOrchestrationService.ScriptExecutionIsEnabled = value; }
        }

        public void GenerateCode(
            List<Template> templates,
            Dictionary<string, string> replacementDictionary)
        {
            try
            {
                this.templateGenerationOrchestrationService.GenerateCode(templates, replacementDictionary);
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

        private void LogEventSetup() =>
            this.templateGenerationOrchestrationService.LogRaised += this.LogRaised;

        private ITemplateGenerationOrchestrationService InitialiseClient(
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

            return new TemplateGenerationOrchestrationService(
                fileProcessingService,
                executionProcessingService,
                templateProcessingService,
                templateConfig,
                loggingBroker);
        }

        private ILoggingBroker InitialiseLogger()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("Standardly", LogLevel.Warning);
            });

            ILogger<LoggingBroker> logger = loggerFactory.CreateLogger<LoggingBroker>();
            return new LoggingBroker(logger);
        }
    }
}
