// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Clients.Exceptions
{
    public class StandardlyClientDependencyException : Xeption
    {
        public StandardlyClientDependencyException(Xeption innerException)
            : base(message: "Standardly client dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
