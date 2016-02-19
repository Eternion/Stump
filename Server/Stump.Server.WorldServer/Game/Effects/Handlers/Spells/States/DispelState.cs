using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States
{
    [EffectHandler(EffectsEnum.Effect_DispelState)]
    [EffectHandler(EffectsEnum.Effect_DisableState)]
    public class DispelState : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DispelState(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var affectedActor in GetAffectedActors())
            {
                var state = SpellManager.Instance.GetSpellState((uint) Dice.Value);

                if (state == null)
                {
                    logger.Error("Spell state {0} not found", Dice.Value);
                    return false;
                }

                if (Effect.EffectId == EffectsEnum.Effect_DisableState)
                {
                    var stateBuff = affectedActor.GetBuffs(x => x is StateBuff && ((StateBuff)x).State.Id == state.Id).FirstOrDefault();
                    if (stateBuff == null)
                        return false;

                    stateBuff.Delay = (short)Dice.Duration;
                    affectedActor.AddBuff(stateBuff);
                }

                RemoveStateBuff(affectedActor, (SpellStatesEnum)state.Id);
            }

            return true;
        }
    }
}
