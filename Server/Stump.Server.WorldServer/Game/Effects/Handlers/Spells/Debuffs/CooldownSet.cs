using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.History;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Game.Spells.Casts;
using Stump.Server.WorldServer.Game.Fights.Buffs;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_CooldownSet)]
    public class CooldownSet : SpellEffectHandler
    {
        public CooldownSet(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var spellId = Dice.DiceNum;
            var cooldown = Dice.Value;

            foreach (var actor in GetAffectedActors())
            {
                var spell = actor.GetSpell(spellId);
                if (spell == null)
                    continue;

                if (IsTriggerBuff())
                {
                    AddTriggerBuff(actor, TriggerBuff);
                }
                else
                {
                    actor.SpellHistory.RegisterCastedSpell(new SpellHistoryEntry(actor.SpellHistory, spell.CurrentSpellLevel,
                        Caster, actor, Fight.TimeLine.RoundNumber, cooldown));
                    ActionsHandler.SendGameActionFightSpellCooldownVariationMessage(actor.Fight.Clients, Caster, actor, spell, cooldown);
                }
            }

            return true;
        }

        private void TriggerBuff(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var spellId = Dice.DiceNum;
            var cooldown = Dice.Value;

            var spell = buff.Target.GetSpell(spellId);
            if (spell == null)
                return;

            buff.Target.SpellHistory.RegisterCastedSpell(new SpellHistoryEntry(buff.Target.SpellHistory, spell.CurrentSpellLevel,
                Caster, buff.Target, Fight.TimeLine.RoundNumber, cooldown));
            ActionsHandler.SendGameActionFightSpellCooldownVariationMessage(buff.Target.Fight.Clients, Caster, buff.Target, spell, cooldown);
        }
    }
}
