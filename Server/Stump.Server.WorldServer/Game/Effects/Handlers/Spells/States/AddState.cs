using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States
{
    [EffectHandler(EffectsEnum.Effect_AddState)]
    public class AddState : SpellEffectHandler
    {
        private static readonly SpellStatesEnum[] DISPELABLE_STATES = 
        {
            SpellStatesEnum.INVULNÉRABLE_56,
            SpellStatesEnum.SAOUL
        };

        private static readonly SpellStatesEnum[] BYPASSMAXSTACK_STATES =
        {
            SpellStatesEnum.DÉCHARGE,
            SpellStatesEnum.SURCHARGE,
            SpellStatesEnum.INÉBRANLABLE,
            SpellStatesEnum.AFFAIBLI,
        };

        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AddState(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        public bool? Dispelable
        {
            get;
            set;
        }

        public bool? BypassMaxStack
        {
            get;
            set;
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

                if (state.Id == (int)SpellStatesEnum.TÉLÉFRAG_244 || state.Id == (int)SpellStatesEnum.TÉLÉFRAG_251)
                    affectedActor.NeedTelefragState = false;

                AddStateBuff(affectedActor, DISPELABLE_STATES.Contains((SpellStatesEnum)state.Id), BYPASSMAXSTACK_STATES.Contains((SpellStatesEnum)state.Id), state);
            }

            return true;
        }
    }
}