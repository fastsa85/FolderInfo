using System;
using System.IO;
using System.IO.Compression;
using FolderInfo.WinHelper;

namespace FolderInfo
{
    class Program
    {
        private const string ARCHIVE_NAME = "Fast";

        static void Main(string[] args)
        {            
            //get paths
            var targetDirectory = getCurrentUserDirectory();
            var desktopFullPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            //write out files to temporary file
            DirectoryInfo directory = new DirectoryInfo(targetDirectory);
            var temporaryFile = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.GetHashCode() + ".txt";
            var tw = new StreamWriter(temporaryFile, true);
            Console.WriteLine($"Processing directory: {directory}...");
            writeOutDirectory(directory, 17, tw);
            tw.Close();

            //archive file
            archiveFile(temporaryFile, desktopFullPath, ARCHIVE_NAME);

            //clean up
            File.Delete(temporaryFile);

            //Move archive to right-top corner
            Desktop desktop = new Desktop();
            var desktopWidth = desktop.GetWidth();
            var iconSize = desktop.GetIconSize();
            desktop.SetIconPosition(ARCHIVE_NAME, desktopWidth - iconSize, 0);

            //Done
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
        private static void writeOutDirectory(DirectoryInfo directory, int days, StreamWriter sreamWriter, string levelAppendSymbols = "")
        {
            var directorySymbol = ">";
            var fileSymbol = " -";
            
            try
            {
                if (hasDirectoryFileOlderThan(directory, days))
                {                   
                    sreamWriter.WriteLine($"{levelAppendSymbols}{directorySymbol}{directory.Name}");

                    foreach (var file in directory.GetFiles())
                        sreamWriter.WriteLine($"{levelAppendSymbols}{fileSymbol}{file.Name}");


                    foreach (var subdirectory in directory.GetDirectories())
                    {
                        writeOutDirectory(subdirectory, days, sreamWriter, levelAppendSymbols + ' ');
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
