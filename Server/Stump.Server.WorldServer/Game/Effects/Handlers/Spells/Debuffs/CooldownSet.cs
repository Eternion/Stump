using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.History;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_CooldownSet)]
    public class CooldownSet : SpellEffectHandler
    {
        public CooldownSet(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var spellId = Dice.DiceNum;
            var cooldown = Dice.Value;

            foreach (var actor in GetAffectedActors())
            {
                var spell = actor.GetSpell(spellId);
                if (spell == null)
                    continue;

                actor.SpellHistory.RegisterCastedSpell(new SpellHistoryEntry(actor.SpellHistory, spell.CurrentSpellLevel, Caster, actor, Fight.TimeLine.RoundNumber, cooldown));
                ActionsHandler.SendGameActionFightSpellCooldownVariationMessage(actor.Fight.Clients, Caster, actor, spell, cooldown);
            }

            return true;
        }
    }
}
