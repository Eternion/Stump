using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes
{
    public class Zone : IShape
    {
        public const int EFFECTSHAPE_DEFAULT_EFFICIENCY = 10;
        public const int EFFECTSHAPE_DEFAULT_MAX_EFFICIENCY_APPLY = 4;

        private IShape m_shape;

        private SpellShapeEnum m_shapeType;

        public Zone(SpellShapeEnum shape, byte radius)
        {
            Radius = radius;
            ShapeType = shape;
        }

        public Zone(SpellShapeEnum shape, byte radius, byte minRadius)
        {
            Radius = radius;
            MinRadius = minRadius;
            ShapeType = shape;
        }

        public Zone(SpellShapeEnum shape, byte radius, byte minRadius, DirectionsEnum direction, int efficiencyMalus, int maxEfficiency)
        {
            Radius = radius;
            MinRadius = minRadius;
            Direction = direction;
            ShapeType = shape;
            EfficiencyMalus = efficiencyMalus > 0 ? efficiencyMalus : EFFECTSHAPE_DEFAULT_EFFICIENCY;
            MaxEfficiency = maxEfficiency > 0 ? maxEfficiency : EFFECTSHAPE_DEFAULT_MAX_EFFICIENCY_APPLY;
        }

        public SpellShapeEnum ShapeType
        {
            get { return m_shapeType; }
            set
            {
                m_shapeType = value;
                InitializeShape();
            }
        }

        public IShape Shape
        {
            get { return m_shape; }
        }

        #region IShape Members

        public uint Surface
        {
            get { return m_shape.Surface; }
        }

        public byte MinRadius
        {
            get { return m_minRadius; }
            set { m_minRadius = value;

                if (m_shape != null)
                    m_shape.MinRadius = value;
            }
        }

        public int EfficiencyMalus
        {
            get;
            set;
        }

        public int MaxEfficiency
        {
            get;
            set;
        }

        public DirectionsEnum Direction
        {
            get
            {
                return m_direction;
            }
            set
            {
                m_direction = value;
                if (m_shape != null)
                    m_shape.Direction = value;
            }
        }

        private byte m_radius;
        private DirectionsEnum m_direction;
        private byte m_minRadius;

        public byte Radius
        {
            get { return m_radius; }
            set
            {
                m_radius = value; 
                if (m_shape != null)
                    m_shape.Radius = value;
            }
        }

        public Cell[] GetCells(Cell centerCell, Map map) => m_shape.GetCells(centerCell, map);

        #endregion

        private void InitializeShape()
        {
            switch (ShapeType)
            {
                case SpellShapeEnum.X:
                    m_shape = new Cross(MinRadius, Radius);
                    break;
                case SpellShapeEnum.L:
                    m_shape = new Line(Radius, false);
                    break;
                case SpellShapeEnum.l:
                    m_shape = new Line(Radius, true);
                    break;
                case SpellShapeEnum.T:
                    m_shape = new Cross(0, Radius)
                    {
                        OnlyPerpendicular = true
                    };
                    break;
                case SpellShapeEnum.D:
                    m_shape = new Cross(0, Radius);
                    break;
                case SpellShapeEnum.C:
                    m_shape = new Lozenge(MinRadius, Radius);
                    break;
                case SpellShapeEnum.I:
                    m_shape = new Lozenge(Radius, 63);
                    break;
                case SpellShapeEnum.O:
                    m_shape = new Lozenge(Radius, Radius);
                    break;
                case SpellShapeEnum.Q:
                    m_shape = new Cross(MinRadius > 0 ? MinRadius : (byte)1, Radius);
                    break;
                case SpellShapeEnum.G:
                    m_shape = new Square(0, Radius);
                    break;
                case SpellShapeEnum.V:
                    m_shape = new Cone(0, Radius);
                    break;
                case SpellShapeEnum.W:
                    m_shape = new Square(0, Radius)
                    {
                        DiagonalFree = true
                    };
                    break;
                case SpellShapeEnum.plus:
                    m_shape = new Cross(0, Radius)
                    {
                        Diagonal = true
                    };
                    break;
                case SpellShapeEnum.sharp:
                    m_shape = new Cross(MinRadius > 0 ? MinRadius : (byte)1, Radius)
                    {
                        Diagonal = true
                    };
                    break;
                case SpellShapeEnum.star:
                    m_shape = new Cross(0, Radius)
                    {
                        AllDirections = true
                    };
                    break;
                case SpellShapeEnum.slash:
                    m_shape = new Line(Radius, false);
                    break;
                case SpellShapeEnum.U:
                    m_shape = new HalfLozenge(0, Radius);
                    break;
                case SpellShapeEnum.A:
                case SpellShapeEnum.a:
                    m_shape = new Lozenge(0, 63);
                    break;
                case SpellShapeEnum.P:
                    m_shape = new Single();
                    break;
                case SpellShapeEnum.minus:
                    m_shape = new Cross(0, Radius)
                    {
                        Diagonal = true,
                        OnlyPerpendicular = true
                    };
                    break;
                default:
                    m_shape = new Cross(MinRadius, Radius);
                    break;
            }

            m_shape.Direction = Direction;
        }
    }
}