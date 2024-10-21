

// ReSharper disable once CheckNamespace

using System.Linq;

namespace System.IO
{
    public static class OltSystemIOExtensions
    {

        /// <summary>
        /// Converts <see cref="long"/> to <see cref="int"/>
        /// </summary>
        /// <param name="self">Extends <see cref="long"/>.</param>
        /// <returns>Returns converted value to <see cref="int"/>. if cast fails, null int</returns>
        private static int ToInt(this long self)
        {
            return Convert.ToInt32(self);
        }


        /// <summary>
        /// Writes byte array to MemoryStream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns><see cref="MemoryStream"/></returns>
        public static MemoryStream ToMemoryStream(this byte[] stream)
        {
            var memStream = new MemoryStream();
            memStream.Write(stream, 0, stream.Length);
            return memStream;
        }


        /// <summary>
        /// Write Stream to Bytes
        /// </summary>
        /// <param name="stream"></param>
        /// <returns><see cref="byte"/> array</returns>
        public static byte[] ToBytes(this Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }


        /// <summary>
        /// Writes File to MemoryStream using <see cref="File.ReadAllBytes"/>
        /// </summary>
        /// <param name="file"></param>
        /// <returns><see cref="byte"/></returns>
        public static byte[] ToBytes(this FileInfo file)
        {
            var fStream = file.OpenRead();            
            var fileData = new byte[fStream.Length];
            fStream.Read(fileData, 0, ToInt(fStream.Length));
            fStream.Close();
            return fileData;
        }


        /// <summary>
        /// Saves <see cref="MemoryStream"/> to file
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="saveToFileName"></param>
        public static void ToFile(this byte[] stream, string saveToFileName)
        {

            if (File.Exists(saveToFileName))
            {
                File.Delete(saveToFileName);
            }

            var fileOutput = File.Create(saveToFileName, stream.Length);
            fileOutput.Write(stream, 0, stream.Length);
            fileOutput.Close();
        }


        /// <summary>
        /// Saves <see cref="MemoryStream"/> to file
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="saveToFileName"></param>
        /// <returns></returns>
        public static void ToFile(this MemoryStream stream, string saveToFileName)
        {
            if (File.Exists(saveToFileName))
            {
                File.Delete(saveToFileName);
            }

            var fileOutput = File.Create(saveToFileName, ToInt(stream.Length - 1));
            stream.WriteTo(fileOutput);
            fileOutput.Close();
        }

        /// <summary>
        /// Permanently deletes all files older than specified date
        /// </summary>
        /// <param name="self"></param>
        /// <param name="searchPattern"></param>
        public static void DeleteFiles(this DirectoryInfo self, string searchPattern)
        {
            DeleteFiles(self, searchPattern, false);
        }

        /// <summary>
        /// Permanently deletes all files older than specified date
        /// </summary>
        /// <param name="self"></param>
        /// <param name="searchPattern"></param>
        /// <param name="recursive"></param>
        public static void DeleteFiles(this DirectoryInfo self, string searchPattern, bool recursive)
        {
            foreach (var fileInfo in self.GetFiles(searchPattern).ToList())
            {
                fileInfo.Delete();
            }

            if (recursive)
            {
                foreach (var dir in self.GetDirectories())
                {
                    DeleteFiles(dir, searchPattern, true);
                }
            }
        }

        /// <summary>
        /// Permanently deletes all files older than specified date
        /// </summary>
        /// <param name="self"></param>
        /// <param name="searchPattern"></param>
        /// <param name="olderThan"></param>
        /// <param name="recursive"></param>
        public static void DeleteFiles(this DirectoryInfo self, string searchPattern, DateTime olderThan, bool recursive)
        {
            foreach (var fileInfo in self.GetFiles().Where(p => p.CreationTime <= olderThan).ToList())
            {
                if (fileInfo.CreationTime <= olderThan)
                {
                    fileInfo.Delete();
                }
            }

            if (recursive)
            {
                foreach (var dir in self.GetDirectories())
                {
                    DeleteFiles(dir, searchPattern, olderThan, true);
                }
            }

        }

        /// <summary>
        /// Removes extension from <see cref="FileInfo.Name"/> 
        /// </summary>
        /// <param name="file"></param>
        /// <returns><see cref="string"/></returns>
        public static string NameWithoutExt(this FileInfo file)
        {
            return file.Name.Replace(file.Extension, string.Empty);
        }
    }
}