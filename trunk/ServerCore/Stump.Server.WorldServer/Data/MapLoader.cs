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
using System.Collections.Generic;
using System.IO;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.IO;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Data
{
    // todo : extract the d2p file if necessary to be synchronised with the client cache
    public static class MapLoader
    {
        /// <summary>
        /// Name of maps folder
        /// </summary>
        [Variable]
        public static string MapsDir = "/maps/";

        /// <summary>
        /// Name of d2o file for maps
        /// </summary>
        [Variable]
        public static string MapsPakFile = "maps.d2p";

        /// <summary>
        ///   Load ripped maps from maps directory
        /// </summary>
        public static IEnumerable<Map> LoadMaps()
        {
            foreach (string file in Directory.GetFiles(Settings.ContentPath + MapsDir, "*.map", SearchOption.AllDirectories))
            {
                var reader = new BigEndianReader(File.OpenRead(file));

                yield return reader.ReadMap();
            }
        }

        public static int GetMapFilesCount()
        {
            return Directory.GetFiles(Settings.ContentPath + MapsDir, "*.map", SearchOption.AllDirectories).Length;
        }

        // todo : extract d2p file and rips the maps
        public static void ExtractMaps()
        {
        }
    }
}