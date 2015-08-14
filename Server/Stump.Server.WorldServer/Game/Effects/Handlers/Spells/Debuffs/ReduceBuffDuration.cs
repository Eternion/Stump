using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_ReduceEffectsDuration)]
    public class ReduceBuffDuration : SpellEffectHandler
    {
        public ReduceBuffDuration(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                foreach (var buff in actor.GetBuffs().ToArray().Where(buff => buff.Dispellable).Where(buff => buff.Duration > 0))
                {
                    buff.Duration -= integerEffect.Value;

                    if (buff.Duration <= 0)
                        actor.RemoveAndDispellBuff(buff);
                }

                ContextHandler.SendGameActionFightModifyEffectsDurationMessage(Fight.Clients, Caster, actor,
                    (short)-integerEffect.Value);
            }

            return true;
        }
    }
}