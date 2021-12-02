using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Platform
{
    public static class PlatformFile
    {
        public static void CopyDirectory(string sourceDir, string targetDir)
        {
            /*
             * Validate target dir
             */
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            /*
             * Copy files
             */
            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)),true);

            /*
             * Copy directories
             */
            foreach (var directory in Directory.GetDirectories(sourceDir))
                CopyDirectory(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }
    }
}
