using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Armor
{
    [EffectHandler(EffectsEnum.Effect_ReflectSpell)]
    public class SpellReflection : SpellEffectHandler
    {
        public SpellReflection(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                if (Effect.Duration == 0 || Effect.Delay != 0)
                    return false;

                var buffId = actor.PopNextBuffId();
                var buff = new SpellReflectionBuff(buffId, actor, Caster, this, Spell, Critical, FightDispellableEnum.DISPELLABLE);

                actor.AddBuff(buff);
            }

            return true;
        }
    }
}