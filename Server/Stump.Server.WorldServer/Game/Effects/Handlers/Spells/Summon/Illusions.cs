using System;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon
{
    [EffectHandler(EffectsEnum.Effect_Illusions)]
    public class Illusions : SpellEffectHandler
    {
        public Illusions(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var distance = CastPoint.ManhattanDistanceTo(TargetedPoint);
            var direction = CastPoint.OrientationTo(TargetedPoint, false);
            var isEven = (short)direction % 2 == 0;

            Caster.Position.Cell = TargetedCell;

            Caster.SetInvisibilityState(GameActionFightInvisibilityStateEnum.INVISIBLE);

            Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, Caster, Caster,
                entry.Fighter.IsFriendlyWith(Caster) ? TargetedCell : new Cell { Id = -1 }), true);

            foreach (var dir in (DirectionsEnum[])Enum.GetValues(typeof(DirectionsEnum)))
            {
                if (isEven != ((short)dir % 2 == 0))
                    continue;

                var cell = CastPoint.GetCellInDirection(dir, (short)distance);
                if (cell == null)
                    continue;

                var dstCell = Map.GetCell(cell.CellId);

                if (dstCell == null)
                    continue;

                if ((!Fight.IsCellFree(dstCell) && dstCell != TargetedCell) || !dstCell.Walkable)
                    continue;

                var summon = new SummonedImage(Fight.GetNextContextualId(), Caster, dstCell) { SummoningEffect = this };

                foreach (var character in Fight.GetAllCharacters(true))
                {
                    if (direction != dir || !character.Fighter.IsFriendlyWith(Caster))
                        ActionsHandler.SendGameActionFightSummonMessage(character.Client, summon);
                }

                Caster.AddSummon(summon);
                Caster.Team.AddFighter(summon);

                Fight.TriggerMarks(summon.Cell, summon, TriggerType.MOVE);
            }

            return true;
        }

        public override bool RequireSilentCast() => true;
    }
}
