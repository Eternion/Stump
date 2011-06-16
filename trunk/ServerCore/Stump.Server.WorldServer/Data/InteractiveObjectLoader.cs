
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Xml;
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