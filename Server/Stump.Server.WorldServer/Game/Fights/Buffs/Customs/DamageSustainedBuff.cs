using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class DamageSustainedBuff : Buff
    {
        public DamageSustainedBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, short value)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Value = value;
        }

        public short Value
        {
            get;
            private set;
        }

        public override void Apply(BuffTriggerType type, object token)
        {
        }

        public override void Dispell()
        {
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Duration, (sbyte)( Dispellable ? 0 : 1 ), (short)Spell.Id, Effect.Id, 0, (short)values[0], (short)values[1], (short)values[2], 0);
        }
    }
}
