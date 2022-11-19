// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Templates.Exceptions
{
    public class NullTemplateGenerationOrchestrationException : Xeption
    {
        public NullTemplateGenerationOrchestrationException()
            : base(message: "Template Generation Info is null.")
        { }
    }
}
