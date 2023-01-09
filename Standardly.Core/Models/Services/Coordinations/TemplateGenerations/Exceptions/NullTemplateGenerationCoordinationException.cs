// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Coordinations.TemplateGenerations.Exceptions
{
    public class NullTemplateGenerationCoordinationException : Xeption
    {
        public NullTemplateGenerationCoordinationException()
            : base(message: "Template Generation Info is null.")
        { }
    }
}
