using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using System;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public class SpellImmunityBuff : Buff
    {
        public SpellImmunityBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, int spellImmune, bool critical, FightDispellableEnum dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            SpellImmune = spellImmune;
        }

        public int SpellImmune
        {
            get;
        }

        public override void Apply()
        {
        }

        public override void Dispell()
        {
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            if (Delay == 0)
                return new FightTemporarySpellImmunityEffect(Id, Target.Id, Duration, (sbyte)Dispellable, (short)Spell.Id, Effect.Id, 0, SpellImmune);

            var values = Effect.GetValues();

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