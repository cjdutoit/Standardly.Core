// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Standardly.Core.Services.Foundations.Files
{
    public interface IFileService
    {
        ValueTask<bool> CheckIfFileExists(string path);
        ValueTask WriteToFile(string path, string content);
        ValueTask<string> ReadFromFile(string path);
        ValueTask DeleteFile(string path);
        ValueTask<List<string>> RetrieveListOfFiles(string path, string searchPattern = "*");
        ValueTask<bool> CheckIfDirectoryExists(string path);
        ValueTask CreateDirectory(string path);
        ValueTask DeleteDirectory(string path, bool recursive = false);
    }
}
