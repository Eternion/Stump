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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Npcs;
using Stump.Server.WorldServer.Threshold;

namespace Stump.Server.WorldServer.Data
{
    public class ThresholdLoader
    {
        /// <summary>
        ///   Name of Thresholds folder
        /// </summary>
        [Variable]
        public static string ThresholdsDir = "Thresholds/";


        public static Dictionary<string, ThresholdDictionnary> LoadThresholds()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + ThresholdsDir);
            var dico = new Dictionary<string, ThresholdDictionnary>();

            foreach (FileInfo file in directory.GetFiles("*.xml"))
            {
                var name = file.Name.Split('.')[0];
                var doc = XDocument.Load(file.FullName);
                dico.Add(name, new ThresholdDictionnary(name, XDocument.Load(file.FullName)));
            }
            return dico;
        }

    }
}