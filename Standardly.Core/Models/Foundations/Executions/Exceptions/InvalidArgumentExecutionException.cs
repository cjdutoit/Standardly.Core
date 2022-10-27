// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Foundations.Executions.Exceptions
{
    public class InvalidArgumentExecutionException : Xeption
    {
        public InvalidArgumentExecutionException()
            : base(message: "Invalid execution argument(s), please correct the errors and try again.")
        { }
    }
}
