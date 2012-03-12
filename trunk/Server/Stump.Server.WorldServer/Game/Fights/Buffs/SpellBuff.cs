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

        public SpellBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, Spell boostedSpell, short boost, bool critical, bool dispelable, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
            BoostedSpell = boostedSpell;
            Boost = boost;
        }

        public Spell BoostedSpell
        {
            get;
            private set;
        }

        public short Boost
        {
            get;
            private set;
        }

        public override void Apply()
        {
            Target.BuffSpell(BoostedSpell, Boost);
        }

        public override void Dispell()
        {
            Target.UnBuffSpell(BoostedSpell, Boost);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporarySpellBoostEffect(Id, Target.Id, Duration, (sbyte)(Dispelable ? 1 : 0), (short) Spell.Id, 0, Boost, (short) BoostedSpell.Id);
        }
    }
}