// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace Standardly.Core.Brokers.Files
{
    public interface IFileBroker
    {
        bool CheckIfFileExists(string path);
        bool WriteToFile(string path, string content);
        string ReadFile(string path);
        bool DeleteFile(string path);
        List<string> GetListOfFiles(string path, string searchPattern = "*");
        bool CheckIfDirectoryExists(string path);
        bool CreateDirectory(string path);
        bool DeleteDirectory(string path, bool recursive = false);
    }
}
