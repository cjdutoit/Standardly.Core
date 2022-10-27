﻿// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

namespace Standardly.Core.Brokers.Files
{
    public interface IFileBroker
    {
        bool CheckIfFileExists(string path);
        void WriteToFile(string path, string content);
    }
}
