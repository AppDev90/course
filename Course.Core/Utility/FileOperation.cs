using Course.Core.Common;
using Course.Core.Utility.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Utility
{
    public class FileOperation
    {

        public static string GetFullPath(string folder, string name)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + folder, name);
        }

        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static string GenerateAvatarFullName(string extention)
        {
            return CodeGenerator.GenerateUnicCodeForAvatar() + extention;
        }

    }
}
