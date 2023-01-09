// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Coordinations.TemplateGenerations.Exceptions
{
    public class TemplateGenerationCoordinationServiceException : Xeption
    {
        public TemplateGenerationCoordinationServiceException(Exception innerException)
            : base(message: "Template coordination service error occurred, contact support.", innerException)
        { }
    }
}
