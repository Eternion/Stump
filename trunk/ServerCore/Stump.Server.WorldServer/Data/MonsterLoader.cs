
using System.Collections.Generic;
using System.IO;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.WorldServer.Data
{
    public class MonsterLoader
    {
        /// <summary>
        /// Name of monsters folder
        /// </summary>
        [Variable]
        public static string MapsDir = "Monsters/";

        public static IEnumerable<GameRolePlayGroupMonsterInformations> LoadSpawnData()
        {
            var mapDirectory = new DirectoryInfo(Settings.StaticPath + MapsDir);

            foreach (FileInfo file in mapDirectory.GetFiles("*.xml", SearchOption.AllDirectories))
            {
             
                yield return null;
            }
        }
    }
}