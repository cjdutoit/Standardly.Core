// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Processings.Files.Exceptions
{
    public class FileProcessingServiceException : Xeption
    {
        public FileProcessingServiceException(Xeption innerException)
            : base(message: "File service error occurred, please contact support", innerException)
        { }
    }
}
