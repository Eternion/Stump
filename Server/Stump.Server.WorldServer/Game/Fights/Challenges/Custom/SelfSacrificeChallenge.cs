using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.ABNÉGATION)]
    public class SelfSacrificeChallenge : DefaultChallenge
    {
        public SelfSacrificeChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 10;
            BonusMax = 25;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
                fighter.LifePointsChanged += OnLifePointsChanged;
        }

        void OnLifePointsChanged(FightActor fighter, int delta, int shieldDamages, int permanentDamages, FightActor from, EffectSchoolEnum school)
        {
            if (delta > 0 && Fight.FighterPlaying == fighter)
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
