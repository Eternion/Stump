using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_Frikt)]
    public class Frikt : SpellEffectHandler
    {
        public Frikt(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var buffId = actor.PopNextBuffId();
                var buff = new TriggerBuff(buffId, actor, Caster, Dice, Spell, false, false,
                    BuffTriggerType.AFTER_ATTACKED, FriktBuffTrigger);

                actor.AddAndApplyBuff(buff);
            }

            return true;
        }

        private static void FriktBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            if (damage.Spell == null)
                return;

            if (damage.Spell.Id == 0)
                return;

            if (damage.Source == null)
                return;

            var source = damage.Source;
            var target = buff.Target;

            if (damage.Source == target)
                return;

            if (!target.Position.Point.IsOnSameLine(source.Position.Point))
                return;

            var casterDirection = target.Position.Point.OrientationTo(source.Position.Point, false);
            var targetedCell = target.Map.Cells[target.Position.Point.GetNearestCellInDirection(casterDirection).CellId];

            if (!target.Fight.IsCellFree(targetedCell))
                return;

            target.Position.Cell = targetedCell;
            target.Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, source, target, targetedCell));
        }
    }
}
