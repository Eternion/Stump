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
            BonusMin = 30;
            BonusMax = 50;
        }

        public override void Initialize()
        {
            base.Initialize();

            Fight.BeforeTurnStopped += OnBeforeTurnStopped;
        }

        private void OnBeforeTurnStopped(IFight fight, FightActor fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            if (fighter.UsedMP == 1)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
            Fight.BeforeTurnStopped -= OnBeforeTurnStopped;
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            OnBeforeTurnStopped(fight, fight.FighterPlaying);

            base.OnWinnersDetermined(fight, winners, losers, draw);
        }
    }
}
