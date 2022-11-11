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
        ValueTask<bool> CheckIfFileExistsAsync(string path);
        ValueTask<bool> WriteToFileAsync(string path, string content);
        ValueTask<string> ReadFromFileAsync(string path);
        ValueTask<bool> DeleteFileAsync(string path);
        ValueTask<List<string>> RetrieveListOfFilesAsync(string path, string searchPattern = "*");
        ValueTask<bool> CheckIfDirectoryExistsAsync(string path);
        ValueTask<bool> CreateDirectoryAsync(string path);
        ValueTask<bool> DeleteDirectoryAsync(string path, bool recursive = false);
    }
}
