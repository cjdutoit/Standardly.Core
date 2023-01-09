// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Coordinations.TemplateGenerations.Exceptions
{
    public class FailedTemplateGenerationCoordinationServiceException : Xeption
    {
        public FailedTemplateGenerationCoordinationServiceException(Exception innerException)
            : base(message: "Failed template coordination service occurred, please contact support", innerException)
        { }
    }
}
