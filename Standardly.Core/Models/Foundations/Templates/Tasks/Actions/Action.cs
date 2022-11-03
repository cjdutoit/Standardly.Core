﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using Standardly.Core.Models.Foundations.Executions;
using Standardly.Core.Models.Foundations.Templates.Tasks.Actions.Files;

namespace Standardly.Core.Models.Foundations.Templates.Tasks.Actions
{
    public class Action
    {
        public string Name { get; set; }
        public string ExecutionFolder { get; set; }
        public List<File> Files { get; set; } = new List<File>();
        public List<Execution> Executions { get; set; } = new List<Execution>();
    }
}