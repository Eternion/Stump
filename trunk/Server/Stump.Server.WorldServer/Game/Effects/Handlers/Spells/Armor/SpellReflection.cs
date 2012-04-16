using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Armor
{
    [EffectHandler(EffectsEnum.Effect_ReflectSpell)]
    public class SpellReflection : SpellEffectHandler
    {
        public SpellReflection(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                if (Effect.Duration <= 0)
                    return false;

                int buffId = actor.PopNextBuffId();
                var buff = new SpellReflectionBuff(buffId, actor, Caster, Dice, Spell, Critical, true);

                actor.AddAndApplyBuff(buff);
            }

            return true;
        }
    }
}