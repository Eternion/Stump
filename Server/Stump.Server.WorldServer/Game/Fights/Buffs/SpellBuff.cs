using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public class SpellBuff : Buff
    {
        public SpellBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, Spell boostedSpell, short boost, bool critical, bool dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            BoostedSpell = boostedSpell;
            Boost = boost;
        }

        public SpellBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, Spell boostedSpell, short boost, bool critical, bool dispelable, int priority, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, priority, customActionId)
        {
            BoostedSpell = boostedSpell;
            Boost = boost;
        }

        public Spell BoostedSpell
        {
            get;
        }

        public short Boost
        {
            get;
        }

        public override void Apply()
        {
            base.Apply();
            Target.BuffSpell(BoostedSpell, Boost);
        }

        public override void Dispell()
        {
            base.Dispell();
            Target.UnBuffSpell(BoostedSpell, Boost);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            if (Delay == 0)
                return new FightTemporarySpellBoostEffect(Id, Target.Id, Duration, (sbyte)(Dispellable ? 1 : 0), (short) Spell.Id, Effect.Id, 0, Boost, (short) BoostedSpell.Id);

            var values = Effect.GetValues();
            return new FightTriggeredEffect(Id, Target.Id, (short)(Duration + Delay), (sbyte)(Dispellable ? 0 : 1), (short)Spell.Id, Effect.Id, 0, (short)values[0], (short)values[1], (short)values[2], Delay);
        }
    }
}