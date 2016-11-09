using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class WallsBinding
    {
        public event Action<WallsBinding> Removed;

        protected virtual void OnRemoved()
        {
            var handler = Removed;
            if (handler != null) handler(this);
        }

        readonly Color m_color;
        readonly List<Wall> m_walls = new List<Wall>();
        readonly List<WallsBinding> m_intersections = new List<WallsBinding>(); 

        public WallsBinding(SummonedBomb bomb1, SummonedBomb bomb2, Color color)
        {
            m_color = color;
            Bomb1 = bomb1;
            Bomb2 = bomb2;
        }

        public SummonedBomb Bomb1
        {
            get;
        }

        public SummonedBomb Bomb2
        {
            get;
        }

        public int Length
        {
            get;
            private set;
        }

        public ReadOnlyCollection<WallsBinding> Intersections => m_intersections.AsReadOnly();

        public bool IsValid() => Bomb1.IsBoundWith(Bomb2);

        public bool Contains(Cell cell) => m_walls.Any(x => x.CenterCell == cell);

        public bool MustBeAdjusted()
        {
            var dist = Bomb1.Position.Point.ManhattanDistanceTo(Bomb2.Position.Point);

            return dist != Length + 1;
        }

        public void AdjustWalls()
        {
            var dist = Bomb1.Position.Point.ManhattanDistanceTo(Bomb2.Position.Point);
            // we assume it's valid

            if (dist == Length + 1)
                return; // nothing to change

            var cells = Bomb1.Position.Point.GetCellsOnLineBetween(Bomb2.Position.Point).Select(y => y.CellId);

            var wallsToRemove = m_walls.Where(x => !cells.Contains(x.CenterCell.Id)).ToArray();

            foreach (var wall in wallsToRemove)
            {
                wall.Remove();
                m_walls.Remove(wall);
            }

            foreach (var cellId in cells)
            {
                if (m_walls.Any(x => x.CenterCell.Id == cellId))
                    continue;

                var cell = Bomb1.Fight.Cells[cellId];
                var existantWall = Bomb1.Fight.GetTriggers(cell).OfType<Wall>().FirstOrDefault(x => x.Caster == Bomb1.Summoner);

                if (existantWall == null)
                {
                    var wall = new Wall((short) Bomb1.Fight.PopNextTriggerId(), Bomb1.Summoner, Bomb1.WallSpell, null,
                        cell, this,
                        new MarkShape(Bomb1.Fight, cell, GameActionMarkCellsTypeEnum.CELLS_CIRCLE, 0, 0, m_color));

                    Bomb1.Fight.AddTriger(wall);
                    m_walls.Add(wall);
                }
                else
                {
                    if (m_intersections.Contains(existantWall.WallBinding))
                        continue;

                    m_intersections.Add(existantWall.WallBinding);
                    existantWall.WallBinding.Removed += OnIntersectionRemoved;
                }

            }

            foreach (var wall in m_walls.ToArray())
            {
                var fighter = Bomb1.Fight.GetOneFighter(wall.CenterCell);
                if (fighter != null)
                    Bomb1.Fight.TriggerMarks(wall.CenterCell, fighter, TriggerType.MOVE);
            }

            Length = dist > 0 ? (int)dist - 1 : 0;
        }

        void OnIntersectionRemoved(WallsBinding obj)
        {
            obj.Removed -= OnIntersectionRemoved;
            m_intersections.Remove(obj);

            AdjustWalls();
        }

        public void Delete()
        {
            foreach (var wall in m_walls.ToArray())
            {  
                wall.Remove();
            }

            m_walls.Clear();
            OnRemoved();
        }
    }

}