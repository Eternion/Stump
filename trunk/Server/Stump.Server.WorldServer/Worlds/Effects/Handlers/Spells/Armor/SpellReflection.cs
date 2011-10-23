using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Fights.Buffs;
using Stump.Server.WorldServer.Worlds.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells.Armor
{
    [EffectHandler(EffectsEnum.Effect_ReflectSpell)]
    public class SpellReflection : SpellEffectHandler
    {
        public SpellReflection(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override void Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                if (Effect.Duration <= 0)
                    return;

                int buffId = actor.PopNextBuffId();
                var buff = new SpellReflectionBuff(buffId, actor, Caster, Dice, Spell, Critical, true);

                actor.AddAndApplyBuff(buff);
            }
        }
    }
}