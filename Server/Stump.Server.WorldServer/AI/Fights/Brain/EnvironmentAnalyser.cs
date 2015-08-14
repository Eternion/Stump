using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Threading;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
    public class EnvironmentAnalyser
    {
        public EnvironmentAnalyser(AIFighter fighter)
        {
            Fighter = fighter;
            CellInformationProvider = new AIFightCellsInformationProvider(Fighter.Fight, Fighter);
        }

        public AIFightCellsInformationProvider CellInformationProvider
        {
            get;
            private set;
        }

        public AIFighter Fighter
        {
            get;
            private set;
        }

        public IFight Fight
        {
            get { return Fighter.Fight; }
        }

        public Cell GetFreeAdjacentCell()
        {
            var cell = Fighter.Position.Point.GetAdjacentCells(CellInformationProvider.IsCellWalkable).FirstOrDefault();

            return cell != null ? CellInformationProvider.GetCellInformation(cell.CellId).Cell : null;
        }

        public Cell GetCellToCastSpell(Cell target, Spell spell, bool nearFirst = true)
        {
            var moveZone = new LozengeSet(Fighter.Position.Point, Fighter.MP);
            var castRange = new LozengeSet(MapPoint.GetPoint(target), Fighter.GetSpellRange(spell.CurrentSpellLevel));

            var intersection = new Intersection(moveZone, castRange);

            var closestPoint = intersection.EnumerateValidPoints().Where(x => Fight.Cells[x.CellId].Walkable).
                OrderBy(x => (nearFirst ? 1 : -1)*x.ManhattanDistanceTo(Fighter.Position.Point)).FirstOrDefault();

            return closestPoint != null ? Fighter.Fight.Cells[closestPoint.CellId] : null;
        }

        public IEnumerable<Cell> GetCellsWithLoS(Cell target)
        {
            return GetCellsWithLoS(target, new LozengeSet(Fighter.Position.Point, Fighter.MP));
        }

        public IEnumerable<Cell> GetCellsWithLoS(Cell target, Set searchZone)
        {            
            foreach (var cell in searchZone.EnumerateSet())
            {
                if (Fight.CanBeSeen(cell, MapPoint.GetPoint(target)))
                    yield return Fight.Cells[cell.CellId];
            }
        }

        public Cell GetCellToFlee()
        {
            var rand = new AsyncRandom();
            var movementsCells = GetMovementCells();
            var fighters = Fight.GetAllFighters(entry => entry.IsEnnemyWith(Fighter));

            var currentCellIndice = fighters.Sum(entry => entry.Position.Point.ManhattanDistanceTo(Fighter.Position.Point)); 
            Cell betterCell = null;
            long betterCellIndice = 0;
            foreach (var c in movementsCells)
            {
                if (!CellInformationProvider.IsCellWalkable(c.Id))
                    continue;

                var indice = fighters.Sum(entry => entry.Position.Point.ManhattanDistanceTo(new MapPoint(c)));

                if (betterCellIndice < indice)
                {
                    betterCellIndice = indice;
                    betterCell = c;
                }
                else if (betterCellIndice == indice && rand.Next(2) == 0)
                    // random factory
                {
                    betterCellIndice = indice;
                    betterCell = c;
                }
            }

            return currentCellIndice == betterCellIndice ? Fighter.Cell : betterCell;
        }

        public Cell[] GetMovementCells()
        {
            return GetMovementCells(Fighter.MP);
        }

        public Cell[] GetMovementCells(int mp)
        {
            if (mp <= 0)
                return new Cell[0];

            if (mp > 63)
                return Fight.Map.Cells;

            var circle = new Lozenge(0, (byte) mp);

            return circle.GetCells(Fighter.Cell, Fight.Map);
        }

        public FightActor GetNearestFighter()
        {
            return GetNearestFighter(entry => true);
        }

        public FightActor GetNearestAlly()
        {
            return GetNearestFighter(entry => entry.IsFriendlyWith(Fighter));
        }

        public FightActor GetNearestEnemy()
        {
            return GetNearestFighter(entry => entry.IsEnnemyWith(Fighter));
        }

        public FightActor GetNearestFighter(Predicate<FightActor> predicate)
        {
            return Fight.GetAllFighters(entry => predicate(entry) && Fighter.CanSee(entry)).
                OrderBy(entry => entry.Position.Point.ManhattanDistanceTo(Fighter.Position.Point)).FirstOrDefault();
        }

        public bool IsReachable(FightActor actor)
        {
            var adjacents = actor.Position.Point.GetAdjacentCells(entry => 
                Fight.Map.Cells[entry].Walkable && !Fight.Map.Cells[entry].NonWalkableDuringFight &&
                Fight.IsCellFree(Fight.Map.Cells[entry]));

            return adjacents.Any();
        }
    }
}