// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Standardly.Core.Models.Services.Foundations.ProcessedEvents;
using Standardly.Core.Models.Services.Orchestrations.TemplateGenerations;
using Standardly.Core.Models.Services.Processings.ProcessedEvents.Exceptions;
using Standardly.Core.Services.Orchestrations.TemplateGenerations;
using Standardly.Core.Services.Processings.ProcessedEvents;
using Standardly.Core.Services.Processings.Templates;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Standardly.Core.Tests.Unit.Services.Orchestrations.TemplateGenerations
{
    public partial class TemplateGenerationOrchestrationTests
    {
        private readonly Mock<IProcessedEventProcessingService> processedEventProcessingServiceMock;
        private readonly Mock<ITemplateProcessingService> templateProcessingServiceMock;
        private readonly ITemplateGenerationOrchestrationService templateGenerationOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public TemplateGenerationOrchestrationTests()
        {
            this.processedEventProcessingServiceMock = new Mock<IProcessedEventProcessingService>();
            this.templateProcessingServiceMock = new Mock<ITemplateProcessingService>();
            this.compareLogic = new CompareLogic();

            this.templateGenerationOrchestrationService = new TemplateGenerationOrchestrationService(
                processedEventProcessingService: this.processedEventProcessingServiceMock.Object,
                templateProcessingService: this.templateProcessingServiceMock.Object);
        }

        public static TheoryData ProcessedEventsDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ProcessedEventProcessingValidationException(innerException),
                new ProcessedEventProcessingDependencyValidationException(innerException),
            };
        }

        public static TheoryData ProcessedEventsDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new ProcessedEventProcessingDependencyException(innerException),
                new ProcessedEventProcessingServiceException(innerException)
            };
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static TemplateGenerationInfo CreateRandomTemplateGenerationInfo() =>
            CreateProcessedFiller().Create();

        private static Filler<TemplateGenerationInfo> CreateProcessedFiller()
        {
            var filler = new Filler<TemplateGenerationInfo>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(DateTime.Now)
                .OnProperty(templateGenerationInfo => templateGenerationInfo.Templates)
                    .Use(new System.Collections.Generic.List<Core.Models.Services.Foundations.Templates.Template>());

            return filler;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Processed CreateRandomProcessed(DateTimeOffset? dateTimeOffset = null) =>
            CreateProcessedFiller(dateTimeOffset ?? GetRandomDateTimeOffset()).Create();

        private static Filler<Processed> CreateProcessedFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<Processed>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(borough => borough.Message).Use(GetRandomString())
                .OnProperty(borough => borough.Status).Use(GetRandomString())
                .OnProperty(borough => borough.ProcessedItems).Use(GetRandomNumber())
                .OnProperty(borough => borough.TotalItems).Use(GetRandomNumber());

            return filler;
        }

        private Expression<Func<TemplateGenerationInfo, bool>> SameTemplateGenerationInfoAs(
            TemplateGenerationInfo expectedTemplateGenerationInfo)
        {
            return actualTemplateGenerationInfo =>
                this.compareLogic.Compare(expectedTemplateGenerationInfo, actualTemplateGenerationInfo).AreEqual;
        }
    }
}
