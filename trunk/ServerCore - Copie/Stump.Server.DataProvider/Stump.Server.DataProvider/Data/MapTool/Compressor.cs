
using System;
using System.IO;
using Ionic.Zlib;

namespace Stump.Server.DataProvider.Data.MapTool
{
    public static class Compressor
    {
        public static void CompressMapFile(string mapPath, string dlmPath)
        {
            string dlmFullPath = Path.GetFullPath(dlmPath);
            string mapFullPath = Path.GetFullPath(mapPath);

            var dlmStream = new FileStream(dlmFullPath, FileMode.Open);
            var mapStream = new FileStream(mapFullPath, FileMode.Create);
            var zoutput = new ZlibStream(mapStream, CompressionMode.Compress, CompressionLevel.BestCompression, true);

            try
            {
                CopyStream(dlmStream, zoutput);
            }
            catch
            {
                throw new Exception(string.Format("Cannot compress \'{0}\', unknown error during the compresion",
                                                  Path.GetFileName(mapPath)));
            }
            finally
            {
                zoutput.Close();
                dlmStream.Close();
                mapStream.Close();
            }
        }

        public static void UnCompressDlmFile(string dlmPath, string mapPath)
        {
            string dlmFullPath = Path.GetFullPath(dlmPath);
            string mapFullPath = Path.GetFullPath(mapPath);

            var dlmStream = new FileStream(dlmFullPath, FileMode.Open);
            var mapStream = new FileStream(mapFullPath, FileMode.Create);
            var zoutput = new ZlibStream(mapStream, CompressionMode.Decompress, true);

            try
            {
                CopyStream(dlmStream, zoutput);
            }
            catch
            {
                throw new Exception(
                    string.Format("Cannot uncompress \'{0}\', check that the file is compressed and is not corrupted",
                                  Path.GetFileName(dlmFullPath)));
            }
            finally
            {
                zoutput.Close();
                mapStream.Close();
                dlmStream.Close();
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[1024];
            int len;

            while ((len = input.Read(buffer, 0, 1024)) > 0)
            {
                output.Write(buffer, 0, len);
            }

            output.Flush();
        }
    }
}