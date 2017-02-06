using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_SpellBoost)]
    public class SpellBoost : SpellEffectHandler
    {
        public SpellBoost(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null || !actor.HasSpell(Dice.DiceNum))
                    return false;

                var boostedSpell = actor.GetSpell(Dice.DiceNum);

                if (boostedSpell == null)
                    return false;

                var buff = new SpellBuff(actor.PopNextBuffId(), actor, Caster, this, Spell, boostedSpell, Dice.Value, false, FightDispellableEnum.DISPELLABLE_BY_DEATH);

                actor.AddBuff(buff);
            }

            return true;
        }
    }
}