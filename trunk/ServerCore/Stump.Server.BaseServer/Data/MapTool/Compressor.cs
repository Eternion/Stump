// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.IO;
using zlib;

namespace Stump.Server.BaseServer.Data.MapTool
{
    public static class Compressor
    {
        public static void CompressMapFile(string mapPath, string dlmPath)
        {
            string dlmFullPath = Path.GetFullPath(dlmPath);
            string mapFullPath = Path.GetFullPath(mapPath);

            var dlmStream = new FileStream(dlmFullPath, FileMode.Open);
            var mapStream = new FileStream(mapFullPath, FileMode.Create);
            var zoutput = new ZOutputStream(mapStream, zlibConst.Z_DEFAULT_COMPRESSION);

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
            var zoutput = new ZOutputStream(mapStream);

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