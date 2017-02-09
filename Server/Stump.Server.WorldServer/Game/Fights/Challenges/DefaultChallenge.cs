using System;
using System.Linq;
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
        }

        public int Id
        {
            get;
            protected set;
        }

        public IFight Fight
        {
            get;
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

        public int BonusMin
        {
            get;
            protected set;
        }

        public int BonusMax
        {
            get;
            protected set;
        }

        public int Bonus
        {
            get;
            protected set;
        }

        public virtual void Initialize()
        {
            var team = Fight.DefendersTeam as FightMonsterTeam;
            var monsterTeam = team ?? (FightMonsterTeam)Fight.ChallengersTeam;

            var groupLevel = monsterTeam.Fighters.Sum(x => x.Level);

            var ratio = Math.Max(1, (groupLevel / (monsterTeam.OpposedTeam.Fighters.Sum(x => x.Level) * 2)));
            Bonus = (int)(Math.Round((double)(BonusMin + (BonusMax - BonusMin)*ratio)));

            Fight.WinnersDetermined += OnWinnersDetermined;
        }

        public virtual bool IsEligible() => Fight.GetAllFighters<MonsterFighter>().All(x => !x.Monster.Template.IncompatibleChallenges.Contains((uint)Id));

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

            Fight.WinnersDetermined -= OnWinnersDetermined;
        }
    }
}
