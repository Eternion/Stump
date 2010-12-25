using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.XmlUtils;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.WorldServer.Data
{
    public class NpcLoader
    {
        /// <summary>
        /// Name of monsters folder
        /// </summary>
        [Variable]
        public static string NpcsDir = "/Npcs/";

        public static IEnumerable<Tuple<uint, GameRolePlayNpcInformations>> LoadSpawnData()
        {
            var mapDirectory = new DirectoryInfo(Settings.StaticPath + NpcsDir);

            foreach (var file in mapDirectory.GetFiles("*.xml", SearchOption.AllDirectories))
            {
                uint mapId = uint.Parse(file.Name.Split('_').First());

                yield return new Tuple<uint, GameRolePlayNpcInformations>(mapId, XmlUtils.Deserialize<GameRolePlayNpcInformations>(file.FullName));
            }
        }
    }
}