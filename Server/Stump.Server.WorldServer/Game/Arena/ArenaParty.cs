using System;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Parties;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaParty : Party
    {
        [Variable] public static int MaxArenaMemberCount = 3;

        private int m_rankSum ;

        public ArenaParty(int id)
            : base(id)
        {
        }

        public override PartyTypeEnum Type
        {
            get { return PartyTypeEnum.PARTY_TYPE_ARENA; }
        }

        public override int MembersLimit
        {
            get { return MaxArenaMemberCount; }
        }

        public int GroupRankAverage
        {
            get;
            private set;
        }

        public override bool CanInvite(Character character, out PartyJoinErrorEnum error, Character inviter = null, bool send = true)
        {
            if (Members.Any())
            {
                var lower = Members.Min(x => x.Level);
                var upper = Members.Max(x => x.Level);

                if (Math.Max(character.Level, upper) - Math.Min(character.Level, lower) > ArenaManager.ArenaMaxLevelDifference)
                {
                    if (inviter != null && send)
                    {
                        // Impossible d'inviter %1 : l'écart maximal entre le plus haut et le plus bas niveau d'une équipe Kolizéum ne peut dépasser 50 niveaux.
                        inviter.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 359,
                            character.Name);
                    }

                    error = PartyJoinErrorEnum.PARTY_JOIN_ERROR_UNMET_CRITERION;
                    return false;
                }
            }

            if (character.ArenaPenality > DateTime.Now)
            {
                if (inviter != null && send)
                {
                    // %1 est interdit de Kolizéum pour un certain temps car il a abandonné un match de Kolizéum.
                    inviter.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 328, character.Name);
                }

                error = PartyJoinErrorEnum.PARTY_JOIN_ERROR_UNMET_CRITERION;
                return false;            
            }

            if (character.Level >= ArenaManager.ArenaMinLevel)
                return base.CanInvite(character, out error, inviter, send);

            if (send)
            {
                if (inviter != null)
                {
                    // %1 doit être au moins niveau 50 pour faire des combats en Kolizéum.
                    inviter.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 327, character.Name);
                }
                else
                {
                    // Vous devez être au moins niveau 50 pour faire des combats en Kolizéum.
                    character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 326);
                }
            }

            error = PartyJoinErrorEnum.PARTY_JOIN_ERROR_UNMET_CRITERION;
            return false;
        }

        public override void Kick(Character member)
        {
            if (Leader.Fight is ArenaFight)
                return;

            base.Kick(member);
        }

        public override bool CanLeaveParty(Character character)
        {
            return base.CanLeaveParty(character) && !(character.Fight is ArenaFight);
        }

        protected override void OnGuestPromoted(Character groupMember)
        {
            base.OnGuestPromoted(groupMember);

            m_rankSum += groupMember.ArenaRank;
            GroupRankAverage = m_rankSum/MembersCount;

            ArenaManager.Instance.RemoveFromQueue(groupMember);
        }

        protected override void OnMemberRemoved(Character groupMember, bool kicked)
        {
            ArenaManager.Instance.RemoveFromQueue(this);

            base.OnMemberRemoved(groupMember, kicked);


            m_rankSum -= groupMember.ArenaRank;
            GroupRankAverage = MembersCount > 0 ? m_rankSum / MembersCount : 0;
        }

        public override PartyMemberInformations GetPartyMemberInformations(Character character)
        {
            return character.GetPartyMemberArenaInformations();
        }
    }
}