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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Entities
{
    public class Character : Humanoid
    {

        public Character(long id, ExtendedLook look, VectorIsometric position, string name, List<EntityLook> followingCharacters, int emoteId, uint emoteEndTime, HumanoidRestrictions restrictions, uint titleId, string titleParam, ActorAlignment alignment)
            : base(id, look, position, name,followingCharacters,emoteId,emoteEndTime,restrictions,titleId,titleParam)
        {
            Alignment = alignment;
        }

        public ActorAlignment Alignment
        {
            get;
            set;
        }

        public GameRolePlayCharacterInformations ToGameRolePlayCharacterInformations()
        {
            return new GameRolePlayCharacterInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, new HumanInformations(FollowingCharactersLook, EmoteId, EmoteEndTime, Restrictions.ToActorRestrictionsInformations(), TitleId, TitleParam),Alignment.ToActorAlignmentInformations());
        }

    }
}