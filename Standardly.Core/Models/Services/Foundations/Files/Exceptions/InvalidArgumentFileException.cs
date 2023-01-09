// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Foundations.Files.Exceptions
{
    public class InvalidArgumentFileException : Xeption
    {
        public InvalidArgumentFileException()
            : base(message: "Invalid file argument(s), please correct the errors and try again.")
        { }
    }
}
