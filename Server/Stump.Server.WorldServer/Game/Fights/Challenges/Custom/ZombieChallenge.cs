using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.ZOMBIE)]
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

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            OnTurnStopped(fight, fight.FighterPlaying);

            base.OnWinnersDetermined(fight, winners, losers, draw);
        }
    }
}
