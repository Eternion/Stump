using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Fights.Challenges
{
    public class DefaultChallenge
    {
        public DefaultChallenge(int id, IFight fight)
        {
            Id = id;
            TargetId = -1;
            Bonus = 0;
            Fight = fight;
            Status = ChallengeStatusEnum.RUNNING;

            Fight.WinnersDetermined += OnWinnersDetermined;
        }

        public int Id
        {
            get;
            protected set;
        }

        public IFight Fight
        {
            get;
            private set;
        }

        public ChallengeStatusEnum Status
        {
            get;
            private set;
        }

        public int TargetId
        {
            get;
            protected set;
        }

        public int Bonus
        {
            get;
            protected set;
        }

        public void UpdateStatus(ChallengeStatusEnum status)
        {
            if (Status != ChallengeStatusEnum.RUNNING)
                return;

            Status = status;

            ContextHandler.SendChallengeResultMessage(Fight.Clients, this);
        }

        private void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            if (winners is FightMonsterTeam)
                UpdateStatus(ChallengeStatusEnum.FAILED);

            UpdateStatus(ChallengeStatusEnum.SUCCESS);
        }
    }
}
