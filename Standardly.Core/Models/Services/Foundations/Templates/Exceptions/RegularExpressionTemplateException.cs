﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Foundations.Templates.Exceptions
{
    public class RegularExpressionTemplateException : Xeption
    {
        public RegularExpressionTemplateException(string message)
            : base(message: message)
        { }
    }
}
