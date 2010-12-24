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
using System.IO;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.XmlUtils;
using Stump.DofusProtocol.Classes;

namespace Stump.Tools.Proxy.Data
{
    public static class DataFactory
    {
        [Variable]
        public static string Output = "./../../static/";

        [Variable]
        public static string NpcDir = "Npcs/";

        [Variable]
        public static string MonstersDir = "Monsters/";


        public static void HandleActorInformations(GameRolePlayActorInformations actorInformations, uint mapId)
        {
            if (actorInformations is GameRolePlayNpcInformations)
                HandleNpcInformations(actorInformations as GameRolePlayNpcInformations, mapId);
        }

        public static void HandleNpcInformations(GameRolePlayNpcInformations npcInformations, uint mapId)
        {
            string zoneName = ZonesManager.GetZoneNameByMap(mapId);

            if (!Directory.Exists(Output + NpcDir))
            {
                Directory.CreateDirectory(Output + NpcDir);
            }

            if (!Directory.Exists(Output + NpcDir + zoneName))
            {
                Directory.CreateDirectory(Output + NpcDir + zoneName);
            }

            XmlUtils.Serialize(Output + NpcDir + zoneName + "/" +
                               mapId + "_" + npcInformations.npcId + ".xml", npcInformations);
        }
    }
}