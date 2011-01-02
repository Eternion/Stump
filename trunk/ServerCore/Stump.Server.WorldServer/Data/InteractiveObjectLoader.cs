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
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.XmlUtils;
using Stump.DofusProtocol.Classes;
using Stump.Server.WorldServer.Skills;
using Stump.Server.WorldServer.XmlSerialize;

namespace Stump.Server.WorldServer.Data
{
    public class InteractiveObjectLoader
    {
        [Variable]
        public static string InteractiveObjectsDir = "InteractiveObjects/";

        [Variable]
        public static string SkillActionsDir = "SkillActions/";

        public static IEnumerable<InteractiveElementSerialized> LoadsInteractiveObjects()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + InteractiveObjectsDir);

            foreach (FileInfo file in directory.GetFiles("*.xml", SearchOption.AllDirectories))
            {
                yield return XmlUtils.Deserialize<InteractiveElementSerialized>(file.FullName);
            }
        }

        public static IEnumerable<SkillInstanceSerialized> LoadSkills()
        {
            var directory = new DirectoryInfo(Settings.StaticPath + SkillActionsDir);

            foreach (FileInfo file in directory.GetFiles("*.xml", SearchOption.AllDirectories))
            {
                yield return XmlUtils.Deserialize<SkillInstanceSerialized>(file.FullName);
            }
        }
    }
}