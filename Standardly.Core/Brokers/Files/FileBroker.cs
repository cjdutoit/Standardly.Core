// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.IO;

namespace Standardly.Core.Brokers.Files
{
    public class FileBroker
    {
        public bool CheckIfFileExists(string path) =>
            File.Exists(path);
    }
}
