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
using Stump.Server.DataProvider.Core;
using Stump.Server.WorldServer.XmlSerialize;

namespace Stump.Server.DataProvider.Data.Interactives
{
    public class InteractiveObjectLoader
    {
        [Variable]
        public static string InteractiveObjectsFile = "InteractiveObjects.xml";

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