using System;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class HealOrMultiplyBuff : Buff
    {
        public HealOrMultiplyBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
        }

        public override void Apply()
        {

        }

        public override void Dispell()
        {

        }

        public int GetDamages(int damage)
        {
            var valueDamage = Convert.ToInt32(Effect.GetValues()[0]);
            var valueHeal = Convert.ToInt32(Effect.GetValues()[1]);

            if (new Random().Next(0, 2) == 0)
                return (damage * valueDamage);

            return -(damage * valueHeal);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Duration, (sbyte)(Dispellable ? 0 : 1), (short)Spell.Id, 0, (short)values[0], (short)values[1], (short)values[2], 0);
        }
    }
}