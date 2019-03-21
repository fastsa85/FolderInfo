using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderInfo.Classes;

namespace FolderInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryTree directoryTree = new DirectoryTree(@"C:\Users\Sergey");
            Console.ReadKey();
        }
    }
}
