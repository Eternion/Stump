using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.NOMADE)]
    [ChallengeIdentifier((int)ChallengeEnum.PÉTULANT)]
    public class NomadChallenge : DefaultChallenge
    {
        public NomadChallenge(int id, IFight fight)
            : base(id, fight)
        {
            if (id == (int)ChallengeEnum.NOMADE)
            {
                BonusMin = 20;
                BonusMax = 55;
            }
            else
            {
                BonusMin = 10;
                BonusMax = 10;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            Fight.BeforeTurnStopped += OnTurnStopped;
            Fight.Tackled += OnTackled;
        }

        private void OnTackled(FightActor fighter, int apTackled, int mpTackled)
        {
            if (!(fighter is CharacterFighter))
                return;

            Fight.Tackled -= OnTackled;
            UpdateStatus(ChallengeStatusEnum.FAILED, fighter);
        }

        private void OnTurnStopped(IFight fight, FightActor fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            if (Id == (int)ChallengeEnum.NOMADE && fighter.MP <= 0)
                return;

            if (Id == (int)ChallengeEnum.PÉTULANT && fighter.AP <= 0)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
            Fight.BeforeTurnStopped -= OnTurnStopped;
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            OnTurnStopped(fight, fight.FighterPlaying);

            base.OnWinnersDetermined(fight, winners, losers, draw);
        }
    }
}
