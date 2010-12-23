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
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;
using Stump.Server.WorldServer.Entities;
using NpcEx = Stump.DofusProtocol.D2oClasses.Npc;
using NpcActionEx = Stump.DofusProtocol.D2oClasses.NpcAction;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcManager
    {
        private static readonly Dictionary<NpcActionTypeEnum, Action<Character, NpcSpawn>> NpcActions = new Dictionary
            <NpcActionTypeEnum, Action<Character, NpcSpawn>>
            {
                {NpcActionTypeEnum.ACTION_TALK, NpcActionHandler.HandleTalkAction},
            };

        [StageStep(Stages.Two, "Loaded Npcs")]
        public static void LoadNpcs()
        {
            IEnumerable<NpcEx> npcsEx = DataLoader.LoadData<NpcEx>();

            foreach (NpcEx npc in npcsEx)
            {
                var npcTemplate = new NpcTemplate(npc)
                    {
                        Name = DataLoader.GetI18NText((int) npc.nameId)
                    };
            }
        }

        public static void HandleNpcAction(NpcActionTypeEnum action, Character character, NpcSpawn npc)
        {
            if (!NpcActions.ContainsKey(action))
                throw new NotImplementedException("Npc action '" + action + "' is not implemented");

            NpcActions[action].DynamicInvoke(character, npc);
        }
    }
}