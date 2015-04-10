using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.NOMADE)]
    [ChallengeIdentifier((int)ChallengeEnum.PÉTULANT)]
    public class NomadeChallenge : DefaultChallenge
    {
        public NomadeChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = id == (int)ChallengeEnum.NOMADE ? 20 : 10;
        }

        public override void Initialize()
        {
            base.Initialize();

            Fight.BeforeTurnStopped += OnTurnStopped;
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
