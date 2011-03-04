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

namespace Stump.Server.DataProvider.Data.Trigger
{
    public class TriggerManager : DataManager<int, List<TriggerTemplate>>
    {
        /// <summary>
        ///   Name of Trigger file
        /// </summary>
        [Variable]
        public static string TriggerFile = "Triggers.xml";

        protected override List<TriggerTemplate> GetData(int id)
        {
            return null;
        }

        protected override Dictionary<int, List<TriggerTemplate>> GetAllData()
        {
            using (var sr = new StreamReader(Settings.StaticPath + TriggerFile))
            {
                return
                    Serializer.Deserialize<List<TriggerTemplate>>(sr.BaseStream).GroupBy(t => t.MapId).
                    ToDictionary(g=>(int)g.First().MapId,  g => g.ToList());
            }
        }
    }
}