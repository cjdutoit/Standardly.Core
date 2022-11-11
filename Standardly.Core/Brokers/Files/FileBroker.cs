// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Standardly.Core.Brokers.Files
{
    public class FileBroker : IFileBroker
    {
        public async ValueTask<bool> CheckIfFileExistsAsync(string path) =>
            await Task.FromResult(File.Exists(path));

        public async ValueTask<bool> WriteToFileAsync(string path, string content) =>
            await Task.Run(() =>
            {
                File.WriteAllText(path, content);

                return true;
            });

        public async ValueTask<string> ReadFileAsync(string path) =>
            await Task.FromResult(File.ReadAllText(path));

        public async ValueTask<bool> DeleteFileAsync(string path) =>
            await Task.Run(() =>
            {
                File.Delete(path);

                return true;
            });

        public async ValueTask<List<string>> GetListOfFilesAsync(string path, string searchPattern = "*") =>
            await Task.FromResult(Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories).ToList());

        public async ValueTask<bool> CheckIfDirectoryExistsAsync(string path) =>
            await Task.FromResult(Directory.Exists(path));

        public async ValueTask<bool> CreateDirectoryAsync(string path) =>
            await Task.Run(() =>
            {
                Directory.CreateDirectory(path);

                return true;
            });

        public async ValueTask<bool> DeleteDirectoryAsync(string path, bool recursive = false) =>
            await Task.Run(() =>
            {
                Directory.Delete(path, recursive);
                return true;
            });
    }
}
