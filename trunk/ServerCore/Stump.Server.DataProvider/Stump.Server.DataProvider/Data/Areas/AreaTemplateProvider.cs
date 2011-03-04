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
using System.Linq;
using ProtoBuf;
using Stump.BaseCore.Framework.Attributes;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Areas
{
    public class AreaTemplateProvider : DataProvider<int,AreaTemplate>
    {
        /// <summary>
        ///   Name of Area templates file
        /// </summary>
        [Variable]
        public static string AreaFile = "AreaTemplates.xml";

        protected override AreaTemplate GetData(int id)
        {
            using (var sr = new StreamReader(Settings.StaticPath + AreaFile))
            {
                var areas = Serializer.Deserialize<List<AreaTemplate>>(sr.BaseStream);

                return areas[id];
            }
        }

        protected override Dictionary<int, AreaTemplate> GetAllData()
        {
            using (var sr = new StreamReader(Settings.StaticPath + AreaFile))
            {
                return Serializer.Deserialize<List<AreaTemplate>>(sr.BaseStream).ToDictionary(s => s.Id);
            }
        }
    }
}