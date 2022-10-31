// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace Standardly.Core.Services.Processings.Files
{
    public interface IFileProcessingService
    {
        ValueTask<bool> CheckIfFileExistsAsync(string path);
        ValueTask WriteToFileAsync(string path, string content);
    }
}
