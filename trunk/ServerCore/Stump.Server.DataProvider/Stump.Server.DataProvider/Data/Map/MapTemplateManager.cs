//// /*************************************************************************
////  *
////  *  Copyright (C) 2010 - 2011 Stump Team
////  *
////  *  This program is free software: you can redistribute it and/or modify
////  *  it under the terms of the GNU General Public License as published by
////  *  the Free Software Foundation, either version 3 of the License, or
////  *  (at your option) any later version.
////  *
////  *  This program is distributed in the hope that it will be useful,
////  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
////  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////  *  GNU General Public License for more details.
////  *
////  *  You should have received a copy of the GNU General Public License
////  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
////  *
////  *************************************************************************/
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using ProtoBuf;
//using Stump.BaseCore.Framework.Attributes;
//using Stump.DofusProtocol.D2oClasses;
//using Stump.Server.DataProvider.Core;
//using Stump.Server.DataProvider.Data.D2oTool;
//using Point = System.Drawing.Point;

//namespace Stump.Server.DataProvider.Data.Map
//{
//    public class MapTemplateManager : DataManager<int, MapTemplate>
//    {
//        /// <summary>
//        ///   Name of maps file
//        /// </summary>
//        [Variable]
//        public static string MapsFile = "Maps.xml";

        protected override MapTemplate InternalGetOne(int id)
        {
            throw new Exception("Hey man, are you crazy ?!");
        }

        protected override Dictionary<int, MapTemplate> InternalGetAll()
        {
            var positions = D2OLoader.LoadDataByIdAsDictionary<int, MapPosition>(entry => entry.id);

//            using (var sr = new StreamReader(Settings.StaticPath + MapsFile))
//            {
//                var maps = Serializer.Deserialize<List<MapTemplate>>(sr.BaseStream);

//                foreach (var map in maps)
//                {
//                    if (positions.ContainsKey(map.Id))
//                    {
//                        var pos = positions[map.Id];
//                        map.Position = new Point(pos.posX, pos.posY);
//                        map.Capabilities = new MapCapabilities(pos.capabilities);
//                    }
//                }
//                return maps.ToDictionary(m => m.Id);
//            }
//        }
//    }
//}