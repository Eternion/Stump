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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Npcs;

namespace Stump.Server.WorldServer.Entities
{
    public class NpcSpawn : Entity, ISpawnEntry
    {
        public NpcSpawn(NpcTemplate template, int contextualId)
            : base(contextualId)
        {
            Look = template.Look;
        }

        public NpcTemplate Template
        {
            get;
            protected set;
        }

        public int SpecialArtworkId
        {
            get;
            set;
        }

        public bool IsQuestGiver
        {
            get;
            protected set;
        }

        public bool Sex
        {
            get;
            set;
        }

        public int ContextualId
        {
            get { return (int) Id; }
            set { Id = value; }
        }

        public VectorIsometric Location
        {
            get;
            protected set;
        }

        public void StartDialog(NpcActionTypeEnum actionTypeEnum, Character dialoger)
        {
            NpcManager.HandleNpcAction(actionTypeEnum, dialoger, this);
        }

        public override GameRolePlayActorInformations ToNetworkActor(WorldClient client)
        {
            return new GameRolePlayNpcInformations(
                ContextualId,
                Look,
                GetEntityDisposition(),
                (uint) Template.Id,
                Sex,
                (uint) SpecialArtworkId,
                IsQuestGiver);
        }
    }
}