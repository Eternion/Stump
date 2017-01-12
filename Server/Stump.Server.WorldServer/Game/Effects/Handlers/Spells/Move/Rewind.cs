using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_Rewind)]
    public class Rewind : SpellEffectHandler
    {
        public Rewind(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                AddTriggerBuff(actor, TriggerBuffApply);
            }

            return true;
        }

        public void TriggerBuffApply(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var dstCell = buff.Target.TurnStartPosition.Cell;
            var fighter = Fight.GetOneFighter(dstCell);

            if (fighter != null)
            {
                if (!fighter.IsImmuneToSpell(Spell.Id))
                    buff.Target.Telefrag(Caster, fighter);
            }
            else
            {
                buff.Target.Position.Cell = dstCell;

                ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(buff.Target.Fight.Clients, Caster, buff.Target, buff.Target.Position.Cell);
            }
        }
    }
}
