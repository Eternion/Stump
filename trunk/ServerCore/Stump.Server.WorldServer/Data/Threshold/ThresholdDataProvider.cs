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
using Stump.BaseCore.Framework.Xml;
using Stump.Server.BaseServer.Initializing;

namespace Stump.Server.WorldServer.Data.Threshold
{
    public static class ThresholdDataProvider
    {
        /// <summary>
        ///   Name of Thresholds folder
        /// </summary>
        [Variable] public static string ThresholdsDir = "Thresholds/";

        private static readonly Dictionary<string, ThresholdDictionnary> m_thresholds =
            new Dictionary<string, ThresholdDictionnary>();


        [StageStep(Stages.One, "Loaded Thresholds")]
        public static void LoadThresholds()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + ThresholdsDir);

            foreach (FileInfo file in directory.GetFiles("*.xml"))
            {
                var name = file.Name.Split('.')[0];
                var list = XmlUtils.Deserialize<List<Threshold>>(file.FullName);
                var dico = new Dictionary<uint, long>(list.Count);
                foreach (var element in list)
                    dico.Add(element.Level, element.Value);
                m_thresholds.Add(name, new ThresholdDictionnary(name, dico));
            }
        }

        public static ThresholdDictionnary Get(string name)
        {
            if (m_thresholds.ContainsKey(name))
                return m_thresholds[name];
            return null;
        }
    }
}