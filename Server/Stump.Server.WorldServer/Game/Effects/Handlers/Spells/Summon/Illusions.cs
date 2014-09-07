using System;
using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells;
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
            var distance = CastPoint.DistanceToCell(TargetedPoint);
            var direction = CastPoint.OrientationTo(TargetedPoint, false);
            var isEven = (short)direction%2 == 0;

            Caster.Position.Cell = TargetedCell;

            Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, Caster, Caster, TargetedCell));

            foreach (var summon in from dir in (DirectionsEnum[])Enum.GetValues(typeof(DirectionsEnum)) where isEven == ((short)dir%2 == 0) where direction != dir select Map.GetCell(CastPoint.GetCellInDirection(dir, (short)distance).CellId) into dstCell where dstCell != null where Fight.IsCellFree(dstCell) && dstCell.Walkable select new SummonedImage(Fight.GetNextContextualId(), Caster, dstCell))
            {
                ActionsHandler.SendGameActionFightSummonMessage(Fight.Clients, summon);

                Caster.AddSummon(summon);
                Caster.Team.AddFighter(summon);
            }

            return true;
        }
    }
}
