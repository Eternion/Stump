using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.MAINS_PROPRES)]
    public class CleanHandsChallenge : DefaultChallenge
    {
        public CleanHandsChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 25;
            BonusMax = 25;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.DamageInflicted += OnDamageInflicted;
            }
        }

        private void OnDamageInflicted(FightActor fighter, Damage damage)
        {
            if (fighter.IsAlive())
                return;

            if (!(damage.Source is CharacterFighter))
                return;

            if (damage.Spell == null)
            {
                UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
                return;
            }

            if (fighter.IsIndirectSpellCast(damage.Spell) || fighter.IsPoisonSpellCast(damage.Spell))
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
        }
    }
}
