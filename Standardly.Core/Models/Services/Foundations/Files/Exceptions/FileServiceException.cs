﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Foundations.Files.Exceptions
{
    public class FileServiceException : Xeption
    {
        public FileServiceException(Xeption innerException)
            : base(message: "File service error occurred, contact support.",
                  innerException)
        { }
    }
}
