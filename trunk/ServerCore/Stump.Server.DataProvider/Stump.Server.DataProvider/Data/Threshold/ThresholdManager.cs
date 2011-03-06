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

namespace Stump.Server.DataProvider.Data.Threshold
{
    public class ThresholdManager : DataManager<string, ThresholdDictionnary>
    {
        /// <summary>
        ///   Name of the Thresholds file
        /// </summary>
        [Variable]
        public static string ThresholdsFile = "Thresholds.xml";

        protected override ThresholdDictionnary InternalGetOne(string id)
        {
            using (var reader = new StreamReader(Settings.StaticPath + ThresholdsFile))
            {
                var t = Serializer.Deserialize<List<ThresholdDictionnaryTemplate>>(reader.BaseStream).FirstOrDefault(  th => th.Name == id);

                return t == null ? null : new ThresholdDictionnary(t.Name, t.Thresholds.ToDictionary(th => th.Level, th => th.Value));
            }
        }

        protected override Dictionary<string, ThresholdDictionnary> InternalGetAll()
        {
            using (var reader = new StreamReader(Settings.StaticPath + ThresholdsFile))
            {
                return Serializer.Deserialize<List<ThresholdDictionnaryTemplate>>(reader.BaseStream).
                    Select(t => new ThresholdDictionnary(t.Name, t.Thresholds.ToDictionary(t2 => t2.Level, t2 => t2.Value))).
                    ToDictionary(t3 => t3.Name);
            }
        }
    }
}