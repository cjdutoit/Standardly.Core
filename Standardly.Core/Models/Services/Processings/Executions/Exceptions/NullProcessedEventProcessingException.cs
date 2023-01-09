// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Xeptions;

namespace Standardly.Core.Models.Services.Processings.Executions.Exceptions
{
    public class NullProcessedEventProcessingException : Xeption
    {
        public NullProcessedEventProcessingException()
            : base(message: "Processed status event handler is null.")
        { }
    }
}
