using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.STATUE)]
    public class StatueChallenge : DefaultChallenge
    {
        public StatueChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 25;
            BonusMax = 55;
        }

        public override void Initialize()
        {
            base.Initialize();

            Fight.TurnStopped += OnTurnStopped;
        }

        private void OnTurnStopped(IFight fight, FightActor fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            if (fighter.Position.Cell.Id == fighter.TurnStartPosition.Cell.Id)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);

            Fight.TurnStopped -= OnTurnStopped;
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            OnTurnStopped(fight, fight.FighterPlaying);

            base.OnWinnersDetermined(fight, winners, losers, draw);
        }
    }
}
