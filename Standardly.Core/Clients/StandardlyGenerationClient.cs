// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Microsoft.Extensions.Logging;
using Standardly.Core.Brokers.Executions;
using Standardly.Core.Brokers.Files;
using Standardly.Core.Brokers.Loggings;
using Standardly.Core.Brokers.RegularExpressions;
using Standardly.Core.Models.Clients.Exceptions;
using Standardly.Core.Models.Configurations.Retries;
using Standardly.Core.Models.Events;
using Standardly.Core.Models.Orchestrations;
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
        public event EventHandler<ProcessedEventArgs> Processed;
        private readonly ITemplateGenerationOrchestrationService templateGenerationOrchestrationService;

        public StandardlyGenerationClient()
        {
            ILoggingBroker loggingBroker = this.InitialiseLogger();

            this.templateGenerationOrchestrationService =
                this.InitialiseClient(loggingBroker);

            this.LogEventSetup();
        }

        public StandardlyGenerationClient(ILoggingBroker loggingBroker)
        {
            this.templateGenerationOrchestrationService =
                this.InitialiseClient(loggingBroker);

            this.LogEventSetup();
        }

        public bool ScriptExecutionIsEnabled
        {
            get { return this.templateGenerationOrchestrationService.ScriptExecutionIsEnabled; }
            set { this.templateGenerationOrchestrationService.ScriptExecutionIsEnabled = value; }
        }

        public void GenerateCode(TemplateGenerationInfo templateGenerationInfo)
        {
            try
            {
                this.templateGenerationOrchestrationService.GenerateCode(templateGenerationInfo);
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

        private void LogEventSetup()
        {
            this.templateGenerationOrchestrationService.Processed += ItemProcessed;
        }

        private ITemplateGenerationOrchestrationService InitialiseClient(ILoggingBroker loggingBroker)
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

            return new TemplateGenerationOrchestrationService(
                fileProcessingService,
                executionProcessingService,
                templateProcessingService,
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
