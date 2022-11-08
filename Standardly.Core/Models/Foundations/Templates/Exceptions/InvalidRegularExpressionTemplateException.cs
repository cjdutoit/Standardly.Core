// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Standardly.Core.Models.Foundations.Templates.Exceptions
{
    public class InvalidRegularExpressionTemplateException : Xeption
    {
        public InvalidRegularExpressionTemplateException(Exception exception)
            : base(
                  message: "Failed regular expression template error occurred, contact support.",
                  innerException: exception)
        { }
    }
}
