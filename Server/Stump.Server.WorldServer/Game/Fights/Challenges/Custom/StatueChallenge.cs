using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.STATUE)]
    public class StatueChallenge : DefaultChallenge
    {
        public StatueChallenge(IFight fight)
            : base(fight)
        {
        }

        public StatueChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 25;

            Fight.TurnStarted += OnTurnStarted;
            Fight.TurnStopped += OnTurnStopped;
        }

        private int m_startCell;

        private void OnTurnStarted(IFight fight, FightActor fighter)
        {
            m_startCell = fighter.Position.Cell.Id;
        }

        private void OnTurnStopped(IFight fight, FightActor fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            if (fighter.Position.Cell.Id == m_startCell)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);

            Fight.TurnStarted -= OnTurnStarted;
            Fight.TurnStopped -= OnTurnStopped;
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            OnTurnStopped(fight, fight.FighterPlaying);

            base.OnWinnersDetermined(fight, winners, losers, draw);
        }
    }
}
