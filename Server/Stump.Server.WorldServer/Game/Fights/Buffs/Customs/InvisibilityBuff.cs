using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class InvisibilityBuff : Buff
    {
        public InvisibilityBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
        }

        public InvisibilityBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, int priority, bool dispelable, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, priority, customActionId)
        {
        }

        public override void Apply()
        {
            Target.SetInvisibilityState(GameActionFightInvisibilityStateEnum.INVISIBLE);
            base.Apply();
        }

        public override void Dispell()
        {
            base.Dispell();
            Target.SetInvisibilityState(GameActionFightInvisibilityStateEnum.VISIBLE);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            if (Delay == 0)
                return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)( Dispellable ? 1 : 0 ), (short)Spell.Id, Effect.Id, 0, 1);

            var values = Effect.GetValues();
            return new FightTriggeredEffect(Id, Target.Id, (short)(Duration + Delay), (sbyte)(Dispellable ? 0 : 1), (short)Spell.Id, Effect.Id, 0, (short)values[0], (short)values[1], (short)values[2], Delay);
        }
    }
}