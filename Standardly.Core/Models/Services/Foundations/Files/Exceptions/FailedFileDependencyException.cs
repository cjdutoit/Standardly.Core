// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Services.Foundations.Files.Exceptions
{
    public class FailedFileDependencyException : Xeption
    {
        public FailedFileDependencyException(Exception innerException)
            : base(message: "File service dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
