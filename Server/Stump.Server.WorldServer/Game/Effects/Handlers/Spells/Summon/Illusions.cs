using System;
using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon
{
    [EffectHandler(EffectsEnum.Effect_Illusions)]
    public class Illusions : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Illusions(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var distance = CastPoint.ManhattanDistanceTo(TargetedPoint);
            var direction = CastPoint.OrientationTo(TargetedPoint, false);
            var isEven = (short)direction%2 == 0;

            Caster.Position.Cell = TargetedCell;

            Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, Caster, Caster, TargetedCell));

            foreach (var dir in (DirectionsEnum[])Enum.GetValues(typeof(DirectionsEnum)))
            {
                if (isEven != ((short)dir % 2 == 0))
                    continue;

                if (direction == dir)
                    continue;

                var cell = CastPoint.GetCellInDirection(dir, (short) distance);
                if (cell == null)
                    continue;

                var dstCell = Map.GetCell(cell.CellId);

                if (dstCell == null)
                    continue;

                if (!Fight.IsCellFree(dstCell) || !dstCell.Walkable)
                    continue;

                var summon = new SummonedImage(Fight.GetNextContextualId(), Caster, dstCell);

                ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, summon);

                Caster.AddSummon(summon);
                Caster.Team.AddFighter(summon);

                Fight.TriggerMarks(summon.Cell, summon, TriggerType.MOVE);
            }

            return true;
        }
    }
}
