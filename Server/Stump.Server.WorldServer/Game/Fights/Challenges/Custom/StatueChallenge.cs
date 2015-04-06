using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier(2)]
    public class StatueChallenge : DefaultChallenge
    {
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

            if (fighter.Position.Cell.Id != m_startCell)
                UpdateStatus(ChallengeStatusEnum.FAILED);
        }
    }
}
