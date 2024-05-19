using Pronia.Models;
using System.Runtime.CompilerServices;

namespace Pronia.Extencions
{
    public static class FileHelper
    {
        public static async Task<string> CreateFileAsync(this IFormFile file,string rootPath,params string[] folders)
        {
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string path =GetPath(fileName,rootPath,folders);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
            return fileName;
        }
        public static void DeleteFile(this string fileName, string rootPath, params string[] folders)
        {

            string path = GetPath(fileName, rootPath, folders);

            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }
        private static string GetPath(this string fileName, string rootPath, params string[] folders)
        {
            string path = rootPath;
            foreach (string folder in folders)
            {

                path = Path.Combine(path, folder);
            }
             return  path = Path.Combine(path, fileName);
        }
    }
}
