// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Coordinations.TemplateGenerations.Exceptions
{
    public class TemplateGenerationCoordinationDependencyException : Xeption
    {
        public TemplateGenerationCoordinationDependencyException(Xeption innerException) :
            base(message: "Template coordination dependency error occurred, contact support.", innerException)
        { }
    }
}
