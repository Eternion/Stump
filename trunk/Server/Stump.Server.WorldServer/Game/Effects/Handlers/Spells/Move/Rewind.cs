using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_Rewind)]
    public class Rewind : SpellEffectHandler
    {
        public Rewind(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                AddTriggerBuff(actor, true, BuffTriggerType.TURN_END, TriggerBuffApply);
            }

            return true;
        }

        public void TriggerBuffApply(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            buff.Target.Position.Cell = buff.Target.TurnStartPosition.Cell;
            ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(buff.Target.Fight.Clients, Caster, buff.Target, buff.Target.Position.Cell);
        }
    }
}
