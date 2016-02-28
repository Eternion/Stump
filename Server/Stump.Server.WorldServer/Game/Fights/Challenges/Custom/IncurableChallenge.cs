using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.INCURABLE)]
    public class IncurableChallenge : DefaultChallenge
    {
        public IncurableChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 20;
            BonusMax = 40;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
                fighter.LifePointsChanged += OnLifePointsChanged;
        }

        public override bool IsEligible() => Fight.GetAllCharacters().Any(x => x.BreedId != PlayableBreedEnum.Pandawa);

        void OnLifePointsChanged(FightActor fighter, int delta, int shieldDamages, int permanentDamages, FightActor from, EffectSchoolEnum school)
        {
            if (delta > 0)
                UpdateStatus(ChallengeStatusEnum.FAILED, fighter);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
                fighter.LifePointsChanged -= OnLifePointsChanged;
        }
    }
}
