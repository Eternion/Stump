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
    public abstract class Humanoid : NamedActor, IInventoryOwner,IExchanger, IAttackable
    {

        protected Humanoid(long id, ExtendedLook look, VectorIsometric position, string name, List<EntityLook> followingCharacters, int emoteId, uint emoteEndTime, HumanoidRestrictions restrictions, uint titleId, string titleParam)
            : base(id, look, position, name)
        {
            FollowingCharactersLook = followingCharacters;
            EmoteId = emoteId;
            EmoteEndTime = emoteEndTime;
            Restrictions = restrictions;
            TitleId = titleId;
            TitleParam = titleParam;
        }

        #region Fields

        public List<EntityLook> FollowingCharactersLook
        {
            get;
            set;
        }

        public int EmoteId
        {
            get;
            set;
        }

        public uint EmoteEndTime
        {
            get;
            set;
        }

        public HumanoidRestrictions Restrictions
        {
            get;
            set;
        }

        public uint TitleId
        {
            get;
            set;
        }

        public string TitleParam
        {
            get;
            set;
        }

        #endregion

        #region Actions

        public void Challenge()
        {

        }

        public void Attack()
        {

        }

        public void Aggress()
        {

        }

        public void Exchange()
        {

        }

        public void Trade()
        {

        }

        public void BeMerchant()
        {

        }

        public void UseItem()
        {

        }

        public void UseTaxCollector()
        {

        }

        public void UseInteractive()
        {

        }

        public void SpeakToNpc()
        {

        }

        public void ChangeZone()
        {

        }

        #endregion

        public GameRolePlayHumanoidInformations ToGameRolePlayHumanoidInformations()
        {
            return new GameRolePlayHumanoidInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, new HumanInformations(FollowingCharactersLook, EmoteId, EmoteEndTime, Restrictions.ToActorRestrictionsInformations(), TitleId, TitleParam));
        }

    }
}