using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public enum Directories
    {
        Backup,
        Error
    }

    public class FileOperations
    {
        public static void MoveFile(FileInfo file, Directories dir)
        {
            DirectoryInfo Maindir = file.Directory;
            string SubDirPathInfo = Path.Combine(Maindir.FullName, dir.ToString());

            if (!Directory.Exists(SubDirPathInfo))
                Directory.CreateDirectory(SubDirPathInfo);

            string destinationFile = Path.Combine(SubDirPathInfo, file.Name);
            if (File.Exists(destinationFile))
                File.Delete(destinationFile);

            File.Move(file.FullName, destinationFile);
        }
    }
}
