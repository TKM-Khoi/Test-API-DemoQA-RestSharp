using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class FilePathExtension
    {
        public static string GetAbsolutePath(this string relativePath)
        {
            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            if (File.Exists(absolutePath))
            {
                return absolutePath;
            }
            throw new FileNotFoundException("FILE NOT FOUND: " + relativePath);
        }
    }
}