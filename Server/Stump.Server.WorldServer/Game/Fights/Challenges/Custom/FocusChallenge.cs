using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.FOCUS)]
    public class FocusChallenge : DefaultChallenge
    {
        public FocusChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 30;
            BonusMax = 50;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.DamageInflicted += OnDamageInflicted;
                fighter.Dead += OnDead;
            }
        }

        private void OnDead(FightActor fighter, FightActor killer)
        {
            if (fighter == Target)
                Target = null;
        }

        private void OnDamageInflicted(FightActor fighter, Damage damage)
        {
            if (!(damage.Source is CharacterFighter))
                return;

            if (Target == null || Target == fighter)
                Target = fighter;
            else
                UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
        }
    }
}
