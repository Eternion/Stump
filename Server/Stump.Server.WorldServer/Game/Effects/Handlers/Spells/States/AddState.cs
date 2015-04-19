using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States
{
    [EffectHandler(EffectsEnum.Effect_AddState)]
    public class AddState : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AddState(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                var dispel = false;

                if (state.Id == (int)SpellStatesEnum.Invulnerable || state.Id == (int)SpellStatesEnum.Drunk)
                {
                    dispel = true;

                    if (Spell.Id == (int)SpellIdEnum.INIMOUTH || Spell.Id == (int)SpellIdEnum.GLOURSOMPTUEUX || Spell.Id == (int)SpellIdEnum.MANSOMURE)
                        dispel = false;
                }

                AddStateBuff(affectedActor, dispel, (state.Id == (int)SpellStatesEnum.Unload || state.Id == (int)SpellStatesEnum.Overload || state.Id == (int)SpellStatesEnum.Weakened), state);
            }

            return true;
        }
    }
}