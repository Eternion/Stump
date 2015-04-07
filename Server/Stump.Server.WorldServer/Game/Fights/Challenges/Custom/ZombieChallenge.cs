using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier(1)]
    public class ZombieChallenge : DefaultChallenge
    {
        public ZombieChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 30;

            Fight.BeforeTurnStopped += OnTurnStopped;
        }

        private void OnTurnStopped(IFight fight, FightActor fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            if (fighter.UsedMP == 1)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
            Fight.BeforeTurnStopped -= OnTurnStopped;
        }
    }
}
