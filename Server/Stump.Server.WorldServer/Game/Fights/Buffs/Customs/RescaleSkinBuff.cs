using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class RescaleSkinBuff : Buff
    {
        public RescaleSkinBuff(int id, FightActor target, FightActor caster, SpellEffectHandler effectHandler, Spell spell, bool critical, FightDispellableEnum dispelable, double rescaleFactor, int priority = 0, short? customActionId = null)
            : base(id, target, caster, effectHandler, spell, critical, dispelable, priority, customActionId)
        {
            RescaleFactor = rescaleFactor;
        }

        public double RescaleFactor
        {
            get;
            private set;
        }

        public override void Apply()
        {
            base.Apply();
            Target.UpdateLook(Caster);
        }

        public override void Dispell()
        {
            base.Dispell();
            Target.UpdateLook(Caster);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            if (Delay == 0)
                return new AbstractFightDispellableEffect();

            return new FightTriggeredEffect(Id, Target.Id, Delay,
                (sbyte)Dispellable,
                (short)Spell.Id, Effect.Id, 0,
                (values.Length > 0 ? Convert.ToInt32(values[0]) : 0),
                (values.Length > 1 ? Convert.ToInt32(values[1]) : 0),
                (values.Length > 2 ? Convert.ToInt32(values[2]) : 0),
                Delay);
        }
    }
}