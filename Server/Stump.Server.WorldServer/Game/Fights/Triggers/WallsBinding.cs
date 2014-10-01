using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class WallsBinding
    {
        public event Action<WallsBinding> Removed;

        protected virtual void OnRemoved()
        {
            Action<WallsBinding> handler = Removed;
            if (handler != null) handler(this);
        }

        private readonly Color m_color;
        private List<Wall> m_walls = new List<Wall>();

        public WallsBinding(SummonedBomb bomb1, SummonedBomb bomb2, Color color)
        {
            m_color = color;
            Bomb1 = bomb1;
            Bomb2 = bomb2;
        }

        public SummonedBomb Bomb1
        {
            get;
            private set;
        }

        public SummonedBomb Bomb2
        {
            get;
            private set;
        }

        public int Length
        {
            get;
            private set;
        }

        public bool IntersectOtherWalls // set to true when the walls intersect another wall collection
        {
            get;
            private set;
        }

        public bool IsValid()
        {
            return Bomb1.IsBoundWith(Bomb2);
        }

        public bool MustBeAdjusted()
        {
            var dist = Bomb1.Position.Point.DistanceToCell(Bomb2.Position.Point);

            return dist != Length + 1;
        }

        public void AdjustWalls()
        {
            var dist = Bomb1.Position.Point.DistanceToCell(Bomb2.Position.Point);
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

            IntersectOtherWalls = false;
            foreach (var cellId in cells)
            {
                if (m_walls.Any(x => x.CenterCell.Id == cellId))
                    continue;

                var cell = Bomb1.Fight.Cells[cellId];

                if (Bomb1.Fight.GetTriggers(cell).OfType<Wall>().All(x => x.Caster != Bomb1.Summoner))
                {
                    var wall = new Wall((short) Bomb1.Fight.PopNextTriggerId(), Bomb1.Summoner, Bomb1.WallSpell, null,
                        cell,
                        new MarkShape(Bomb1.Fight, cell, GameActionMarkCellsTypeEnum.CELLS_CIRCLE, 0, m_color));

                    Bomb1.Fight.AddTriger(wall);

                    var fighter = Bomb1.Fight.GetOneFighter(wall.CenterCell);
                    if (fighter != null)
                        Bomb1.Fight.TriggerMarks(wall.CenterCell, fighter, TriggerType.MOVE);
                }
                else IntersectOtherWalls = true;

            }
            Length = dist > 0 ? (int)dist - 1 : 0;
        }

        public void Delete()
        {
            foreach (var wall in m_walls)
            {
                wall.Remove();
            }
            m_walls.Clear();
            OnRemoved();
        }
    }
}