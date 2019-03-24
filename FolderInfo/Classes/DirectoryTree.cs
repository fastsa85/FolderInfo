using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderInfo.Classes
{
    class DirectoryTree
    {
        private DirectoryInfo itself;
        private List<DirectoryInfo> childDirectories;

        public DirectoryTree(string path)
        {
            try
            {
                this.itself = new DirectoryInfo(path);

                this.childDirectories = new List<DirectoryInfo>();

                foreach (var childDirectory in itself.GetDirectories())
                {
                    childDirectories.Add(new DirectoryInfo(itself.FullName + "\\" + childDirectory.Name));
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Directory does not exists: {path}");
                return;
            }
        }

        public void WriteToFile(string filename)
        {

        }
    }
}
