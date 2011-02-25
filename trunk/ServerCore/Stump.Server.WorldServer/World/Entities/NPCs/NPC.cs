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
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.World.Entities.Actors;

namespace Stump.Server.WorldServer.Entities
{
    public class NPC : Actor, ITrader, IExchanger
    {

        protected NPC(long id,string name, ExtendedLook look, VectorIsometric position, uint npcId, bool sex,uint specialArtworkId, bool canGiveQuest)
            :base(id,name,look,position)
        {
            NpcId = npcId;
            Sex = sex;
            SpecialArtworkId = specialArtworkId;
            CanGiveQuest = canGiveQuest;
        }

        public uint NpcId
        {
            get;
            set;
        }

        public bool Sex
        {
            get;
            set;
        }

        public uint SpecialArtworkId
        {
            get;
            set;
        }

        public bool CanGiveQuest
        {
            get;
            set;
        }

        public override GameRolePlayActorInformations ToGameRolePlayActor()
        {
            return new GameRolePlayNpcInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), NpcId, Sex, SpecialArtworkId, CanGiveQuest);
        }

    }
}