using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.INTOUCHABLE)]
    public class UntouchableChallenge : DefaultChallenge
    {
        public UntouchableChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 40;
            BonusMax = 70;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
                fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;
        }

        void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            UpdateStatus(ChallengeStatusEnum.FAILED, fighter);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
                fighter.BeforeDamageInflicted -= OnBeforeDamageInflicted;
        }
    }
}