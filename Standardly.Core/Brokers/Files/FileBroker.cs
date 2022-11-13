// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Standardly.Core.Brokers.Files
{
    public class FileBroker : IFileBroker
    {
        public bool CheckIfFileExists(string path) =>
            File.Exists(path);

        public bool WriteToFile(string path, string content)
        {
            File.WriteAllText(path, content);

            return true;
        }

        public string ReadFile(string path) =>
            File.ReadAllText(path);

        public bool DeleteFile(string path)
        {
            File.Delete(path);

            return true;
        }

        public List<string> GetListOfFiles(string path, string searchPattern = "*") =>
            Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories).ToList();

        public bool CheckIfDirectoryExists(string path) =>
            Directory.Exists(path);

        public bool CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);

            return true;
        }

        public bool DeleteDirectory(string path, bool recursive = false)
        {
            Directory.Delete(path, recursive);

            return true;
        }
    }
}
