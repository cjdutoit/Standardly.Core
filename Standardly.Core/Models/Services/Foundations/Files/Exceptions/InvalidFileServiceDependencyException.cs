// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Foundations.Files.Exceptions
{
    public class InvalidFileServiceDependencyException : Xeption
    {
        public InvalidFileServiceDependencyException(Exception innerException)
            : base(message: "Invalid file service dependency error occurred.",
                  innerException)
        { }
    }
}
