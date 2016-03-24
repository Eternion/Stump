using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SkipTurn)]
    public class SkipTurn : SpellEffectHandler
    {
        public SkipTurn(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical) : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var buff = new SkipTurnBuff(actor.PopNextBuffId(), actor, Caster, this, Spell, false, FightDispellableEnum.DISPELLABLE_BY_DEATH);
                actor.AddBuff(buff);
            }

            return true;
        }
    }

    [EffectHandler(EffectsEnum.Effect_SkipTurn_1031)]
    public class SkipTurnNow : SpellEffectHandler
    {
        public SkipTurnNow(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                actor.PassTurn();
            }

            return true;
        }
    }
}