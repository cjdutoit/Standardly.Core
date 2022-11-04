// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Orchestrations.Templates.Exceptions
{
    public class TemplateOrchestrationDependencyException : Xeption
    {
        public TemplateOrchestrationDependencyException(Xeption innerException) :
            base(message: "Template orchestration dependency error occurred, contact support.", innerException)
        { }
    }
}
