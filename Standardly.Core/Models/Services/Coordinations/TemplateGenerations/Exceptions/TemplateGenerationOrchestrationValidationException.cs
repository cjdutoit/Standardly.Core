// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Coordinations.TemplateGenerations.Exceptions
{
    public class TemplateGenerationCoordinationValidationException : Xeption
    {
        public TemplateGenerationCoordinationValidationException(Xeption innerException)
            : base(message: "Template coordination validation errors occurred, please try again.",
                  innerException)
        { }
    }
}
