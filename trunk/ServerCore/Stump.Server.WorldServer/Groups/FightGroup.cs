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
using System.Linq;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Fights;

namespace Stump.Server.WorldServer.Groups
{
    public sealed class FightGroup : Group<FightGroupMember>
    {
        public FightGroup()
        {
        }

        public int TeamId
        {
            get;
            internal set;
        }

        public ushort[] Positions
        {
            get;
            internal set;
        }

        public Fight Fight
        {
            get;
            internal set;
        }

        public bool IsWinner
        {
            get;
            internal set;
        }

        public bool IsAllDead()
        {
            return Members.Count(entry => !(entry.IsDead || entry.HasLeft)) == 0;
        }

        public bool IsAllReady()
        {
            return Members.All(member => member.IsReady);
        }

        public IEnumerable<int> GetAlivesIds()
        {
            return
                Members.Where(
                    entry =>
                    !((entry.IsDead) || entry.HasLeft))
                    .Select(entry => (int) entry.Entity.Id);
        }

        public IEnumerable<int> GetDeadsIds()
        {
            return
                Members.Where(entry =>
                    entry.IsDead || entry.HasLeft)
                    .Select(entry => (int) entry.Entity.Id);
        }

        public IEnumerable<int> GetLeftIds()
        {
            return Members.Where(entry => entry.HasLeft)
                .Select(entry => (int) entry.Entity.Id);
        }

        /// <summary>
        ///   Add a new member to this group.
        /// </summary>
        public FightGroupMember AddMember(LivingEntity entity)
        {
            FightGroupMember newMember = AddMember(new FightGroupMember(entity, this));

            // todo : send Update to all

            return newMember;
        }

        public FightTeamInformations ToNetworkFightTeam()
        {
            return new FightTeamInformations(
                (uint) Id,
                (int) Leader.Entity.Id,
                (int) AlignmentSideEnum.ALIGNMENT_WITHOUT,
                (uint) TeamEnum.TEAM_CHALLENGER,
                Members.Select(entry => entry.Entity.ToNetworkTeamMember()).ToList());
        }
    }
}