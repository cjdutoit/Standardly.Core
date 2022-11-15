// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;

namespace Standardly.Core.Services.Foundations.Files
{
    public interface IFileService
    {
        bool CheckIfFileExists(string path);
        bool WriteToFile(string path, string content);
        string ReadFromFile(string path);
        bool DeleteFile(string path);
        List<string> RetrieveListOfFiles(string path, string searchPattern = "*");
        bool CheckIfDirectoryExists(string path);
        bool CreateDirectory(string path);
        bool DeleteDirectory(string path, bool recursive = false);
    }
}
