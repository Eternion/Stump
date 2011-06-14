
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