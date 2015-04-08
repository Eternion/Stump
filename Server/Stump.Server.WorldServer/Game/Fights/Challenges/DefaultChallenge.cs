using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Fights.Challenges
{
    public class DefaultChallenge
    {
        public DefaultChallenge(int id, IFight fight)
        {
            Id = id;
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

        public FightActor Target
        {
            get;
            set;
        }

        public int Bonus
        {
            get;
            protected set;
        }

        public void UpdateStatus(ChallengeStatusEnum status, FightActor from = null)
        {
            if (Status != ChallengeStatusEnum.RUNNING)
                return;

            Status = status;

            ContextHandler.SendChallengeResultMessage(Fight.Clients, this);

            if (Status == ChallengeStatusEnum.FAILED && @from is CharacterFighter)
                BasicHandler.SendTextInformationMessage(Fight.Clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 188, ((CharacterFighter)from).Name, Id);
        }

        protected virtual void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            if (winners is FightMonsterTeam)
                UpdateStatus(ChallengeStatusEnum.FAILED);

            UpdateStatus(ChallengeStatusEnum.SUCCESS);
        }
    }
}
