// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Coordinations.TemplateGenerations.Exceptions
{
    public class TemplateGenerationCoordinationDependencyValidationException : Xeption
    {
        public TemplateGenerationCoordinationDependencyValidationException(Xeption innerException)
            : base(message: "Template coordination dependency validation occurred, please try again.", innerException)
        { }
    }
}
