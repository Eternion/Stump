using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
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

        protected override void OnGuestPromoted(Character groupMember)
        {
            base.OnGuestPromoted(groupMember);

            m_rankSum += groupMember.ArenaRank;
            GroupRankAverage = m_rankSum/MembersCount;
        }

        protected override void OnMemberRemoved(Character groupMember, bool kicked)
        {
            base.OnMemberRemoved(groupMember, kicked);


            m_rankSum -= groupMember.ArenaRank;
            GroupRankAverage = m_rankSum/MembersCount;
        }
    }
}