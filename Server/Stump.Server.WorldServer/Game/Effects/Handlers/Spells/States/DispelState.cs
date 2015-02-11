using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States
{
    [EffectHandler(EffectsEnum.Effect_DispelState)]
    [EffectHandler(EffectsEnum.Effect_DisableState)]
    public class DispelState : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DispelState(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var affectedActor in GetAffectedActors())
            {
                var state = SpellManager.Instance.GetSpellState((uint) Dice.Value);

                if (state == null)
                {
                    logger.Error("Spell state {0} not found", Dice.Value);
                    return false;
                }

                RemoveStateBuff(affectedActor, (SpellStatesEnum)state.Id);
            }

            return true;
        }
    }
}
