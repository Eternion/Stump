using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public class SpellImmunityBuff : Buff
    {
        public SpellImmunityBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, int spellImmune, bool critical, bool dispelable)
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
                return new FightTemporarySpellImmunityEffect(Id, Target.Id, Duration, (sbyte)(Dispellable ? 1 : 0), (short)Spell.Id, Effect.Id, 0, SpellImmune);

            var values = Effect.GetValues();
            return new FightTriggeredEffect(Id, Target.Id, (short)(Duration + Delay), (sbyte)(Dispellable ? 0 : 1), (short)Spell.Id, Effect.Id, 0, (short)values[0], (short)values[1], (short)values[2], Delay);
        }
    }
}