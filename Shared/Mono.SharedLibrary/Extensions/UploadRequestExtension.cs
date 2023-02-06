using Mono.SharedLibrary.Dtos.Requests;
using Mono.SharedLibrary.Exceptions;
using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Extensions
{
    public static class UploadRequestExtension
    {
        public static async Task<Models.File> SaveFile(this UploadRequest request, string createBy = "system")
        {
            if (request.Data == null)
                throw new BadRequestException("File's data can not be null");
            var streamData = new MemoryStream(request.Data);
            if (streamData.Length == 0)
                throw new BadRequestException("File's data can not be empty");

            var folder = request.UploadType.ToDescriptionString();
            var folderName = Path.Combine("Upload", folder);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            bool exists = Directory.Exists(pathToSave);
            if (!exists) Directory.CreateDirectory(pathToSave);
            var fileName = request.FileName.Trim('"');
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);
            if (System.IO.File.Exists(dbPath))
            {
                dbPath = NextAvailableFilename(dbPath);
                fullPath = NextAvailableFilename(fullPath);
            }
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await streamData.CopyToAsync(stream);
            }
            return new Models.File() { 
                Guid = Guid.NewGuid(), FileName = fileName, FilePath = dbPath.Replace("\\", "/"), CreatedBy = createBy,
                CreatedTime = DateTime.Now, FileSize = streamData.Length, Extension = Path.GetExtension(fileName)
            };
        }

        #region private filename methods
        private static string numberPattern = " ({0})";
        public static string NextAvailableFilename(string path)
        {
            // Short-cut if already available
            if (!System.IO.File.Exists(path))
                return path;

            // If path has extension then insert the number pattern just before the extension and return next filename
            if (Path.HasExtension(path))
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path)), numberPattern));

            // Otherwise just append the pattern to the path and return next filename
            return GetNextFilename(path + numberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);
            //if (tmp == pattern)
            //throw new ArgumentException("The pattern must include an index place-holder", "pattern");

            if (!System.IO.File.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (System.IO.File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (System.IO.File.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }
        #endregion
    }
}
