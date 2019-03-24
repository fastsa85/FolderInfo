using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FolderInfo.Classes;
using System.IO.Compression;

namespace FolderInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            //get paths
            var targetDirectory = getCurrentUserDirectory();
            var desktopFullPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            //write out files to temporary file
            DirectoryInfo directory = new DirectoryInfo(targetDirectory);
            var temporaryFile = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.GetHashCode() + ".txt";
            var tw = new StreamWriter(temporaryFile, true);
            writeOutDirectoryToStream(directory, 17, tw);
            tw.Close();

            //archive
            archiveFile(temporaryFile, desktopFullPath, "Fast");

            //clean up
            File.Delete(temporaryFile);

            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Returns full path to current user directory
        /// </summary>
        /// <returns></returns>
        private static string getCurrentUserDirectory()
        {
            return Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
        }

        /// <summary>
        /// Check if directory has any file (in subderictories also) older than specified valu in days
        /// </summary>
        /// <param name="directory">Target directory</param>
        /// <param name="days">Days count</param>
        /// <returns></returns>
        private static bool hasDirectoryFileOlderThan(DirectoryInfo directory, int days)
        {
            foreach (var file in directory.GetFiles())
            {
                if ((DateTime.Now - file.CreationTime).Days > days) return true;
            }

            foreach (var subdirectory in directory.GetDirectories())
            {
                return hasDirectoryFileOlderThan(subdirectory, days);
            }

            return false;
        }

        /// <summary>
        /// Write out filenames and directory names for files older than specified value in days
        /// </summary>
        /// <param name="directory">Target directory name</param>
        /// <param name="days">Days count</param>
        /// <param name="sreamWriter">Stream to write</param>
        /// <param name="levelAppendSymbols" default="">Symbols to add before directory name (to visualize directory level)</param>
        private static void writeOutDirectoryToStream(DirectoryInfo directory, int days, StreamWriter sreamWriter, string levelAppendSymbols = "")
        {
            var directorySymbol = ">";
            var fileSymbol = " -";
            
            try
            {
                if (hasDirectoryFileOlderThan(directory, days))
                {
                    Console.WriteLine($"Processing directory: {directory}...");
                    sreamWriter.WriteLine($"{levelAppendSymbols}{directorySymbol}{directory.Name}");

                    foreach (var file in directory.GetFiles())
                        sreamWriter.WriteLine($"{levelAppendSymbols}{fileSymbol}{file.Name}");


                    foreach (var subdirectory in directory.GetDirectories())
                    {
                        writeOutDirectoryToStream(subdirectory, days, sreamWriter, levelAppendSymbols + ' ');
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                sreamWriter.WriteLine($"{levelAppendSymbols}{directorySymbol}{directory.Name}: Can't access.");
            }
        }

        /// <summary>
        /// Archive file to specified destination
        /// </summary>
        /// <param name="sourceFile">Target file to archive</param>
        /// <param name="destinationPath">Destination path where archive will be placed</param>
        /// <param name="archiveName">Archive name</param>
        private static void archiveFile(string sourceFile, string destinationPath, string archiveName)
        {
            using (FileStream input = new FileStream(sourceFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (FileStream output = new FileStream(destinationPath + "\\" + archiveName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (GZipStream zip = new GZipStream(output, CompressionMode.Compress))
                    {
                        input.CopyTo(zip);
                    }
                }
            }
        }
    }
}
