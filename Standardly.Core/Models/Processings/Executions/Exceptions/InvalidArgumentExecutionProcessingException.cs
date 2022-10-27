// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Processings.Templates.Exceptions
{
    public class InvalidArgumentExecutionProcessingException : Xeption
    {
        public InvalidArgumentExecutionProcessingException()
            : base(message: "Invalid execution processing argument(s), please correct the errors and try again.")
        { }
    }
}
