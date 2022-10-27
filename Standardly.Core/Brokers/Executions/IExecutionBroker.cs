﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Executions;

namespace Standardly.Core.Brokers.Executions
{
    public interface IExecutionBroker
    {
        string Run(List<Execution> executions, string executionFolder);
    }
}
