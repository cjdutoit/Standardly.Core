// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Clients.Exceptions
{
    public class StandardlyClientValidationException : Xeption
    {
        public StandardlyClientValidationException(Xeption innerException)
            : base(message: "Standardly client validation error occurred, fix errors and try again.",
                  innerException)
        { }
    }
}
