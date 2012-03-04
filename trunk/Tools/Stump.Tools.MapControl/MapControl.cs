using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Stump.Tools.MapControl
{
    [Serializable]
    [Flags]
    public enum CellState
    {
        None = 0,
        Walkable = 1,
        NonWalkable = 2,
        BluePlacement = 4,
        RedPlacement = 8,
        Trigger = 16,
        Road = 32,
    }

    [Serializable]
    [Flags]
    public enum DrawMode
    {
        None = 0,
        Movements = 1,
        Fights = 2,
        Triggers = 4,
        Others = 8,
        All = 0xF,
    }

    [Serializable]
    public partial class MapControl : UserControl
    {
        public delegate void CellClickedHandler(MapControl control, MapCell cell, MouseButtons buttons, bool hold);
        public event CellClickedHandler CellClicked;

        protected void OnCellClicked(MapCell cell, MouseButtons buttons, bool hold)
        {
            CellClickedHandler handler = CellClicked;
            if (handler != null) handler(this, cell, buttons, hold);
        }

        public event Action<MapControl, MapCell, MapCell> CellOver;

        protected void OnCellOver(MapCell cell, MapCell last)
        {
            var handler = CellOver;
            if (handler != null) handler(this, cell, last);
        }

        private bool m_lesserQuality;
        private bool m_mouseDown;
        private MapCell m_holdedCell;
        private MapCell m_cellOnDown;

        public MapControl()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            MapHeight = 20;
            MapWidth = 14;
            CommonCellHeight = 43;
            CommonCellWidth = 86;
            ViewGrid = true;
            DrawMode = DrawMode.All;
            TraceOnOver = false;
            InactiveCellColor = Color.DarkGray;
            ActiveCellColor = Color.Transparent;
            StatesColors = new Dictionary<CellState, Color>
                               {
                                   {CellState.Walkable, Color.LimeGreen},
                                   {CellState.NonWalkable, Color.Maroon},
                                   {CellState.BluePlacement, Color.DodgerBlue},
                                   {CellState.RedPlacement, Color.Red},
                                   {CellState.Trigger, Color.Orange},
                                   {CellState.Road, Color.LightGoldenrodYellow},
                               };
            SetCellNumber();
            BuildMap();
            InitializeComponent();
        }

        private int m_mapHeight;

        public int MapHeight
        {
            get { return m_mapHeight; }
            set
            {
                m_mapHeight = value; 
                SetCellNumber();
            }
        }

        private int m_mapWidth;

        public int MapWidth
        {
            get { return m_mapWidth; }
            set
            {
                m_mapWidth = value; 
                SetCellNumber();
            }
        }

        public double CommonCellHeight
        {
            get;
            set;
        }

        public double CommonCellWidth
        {
            get;
            set;
        }

        [Browsable(false)]
        public int RealCellHeight
        {
            get;
            private set;
        }

        [Browsable(false)]
        public int RealCellWidth
        {
            get;
            private set;
        }

        public Color InactiveCellColor
        {
            get;
            set;
        }

        public Color ActiveCellColor
        {
            get;
            set;
        }

        public DrawMode DrawMode
        {
            get;
            set;
        }

        public bool ViewGrid
        {
            get;
            set;
        }

        public bool TraceOnOver
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public MapCell CurrentCellOver
        {
            get;
            set;
        }

        public Color BorderColorOnOver
        {
            get;
            set;
        } 

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Dictionary<CellState, Color> StatesColors
        {
            get;
            set;    
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public MapCell[] Cells
        {
            get;
            set;
        }

        public bool LesserQuality
        {
            get { return m_lesserQuality; }
            set
            {
                m_lesserQuality = value;
                Invalidate();
            }
        }

        private void ApplyQuality(Graphics g)
        {
            if (m_lesserQuality)
            {
                g.CompositingMode = CompositingMode.SourceOver;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.InterpolationMode = InterpolationMode.Low;
                g.SmoothingMode = SmoothingMode.HighSpeed;
            }
            else
            {
                g.CompositingMode = CompositingMode.SourceOver;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
            }
        }

        private void SetCellNumber()
        {
            Cells = new MapCell[2 * MapHeight * MapWidth];

            int cellId = 0;
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth * 2; x++)
                {
                    var cell = new MapCell(cellId++);
                    Cells[cell.Id] = cell;
                }
            }
        }

        private double GetMaxScaling()
        {
            double cellWidth = Width/(double) (MapWidth + 1);
            double cellHeight = Height/(double) (MapHeight + 1);
            cellWidth = Math.Min(cellHeight*2, cellWidth);
            return cellWidth;
        }

        public void BuildMap()
        {
            int cellId = 0;
            double cellWidth = GetMaxScaling();
            double cellHeight = Math.Ceiling(cellWidth/2);

            var offsetX = (int) ((Width - ((MapWidth + 0.5) * cellWidth))/2);
            var offsetY = (int) ((Height - ((MapHeight + 0.5) * cellHeight))/2);

            double midCellHeight = cellHeight/2;
            double midCellWidth = cellWidth/2;

            for (int y = 0; y < 2*MapHeight; y++)
            {
                if (y%2 == 0)
                    for (int x = 0; x < MapWidth; x++)
                    {
                        var left = new Point((int) (offsetX + x*cellWidth), (int) (offsetY + y*midCellHeight + midCellHeight));
                        var top = new Point((int) (offsetX + x*cellWidth + midCellWidth), (int) (offsetY + y*midCellHeight));
                        var right = new Point((int) (offsetX + x*cellWidth + cellWidth), (int) (offsetY + y*midCellHeight + midCellHeight));
                        var down = new Point((int) (offsetX + x*cellWidth + midCellWidth), (int) (offsetY + y*midCellHeight + cellHeight));
                        Cells[cellId++].Points = new[] {left, top, right, down};
                    }
                else
                    for (int x = 0; x < MapWidth; x++)
                    {
                        var left = new Point((int) (offsetX + x*cellWidth + midCellWidth), (int) (offsetY + y*midCellHeight + midCellHeight));
                        var top = new Point((int) (offsetX + x*cellWidth + cellWidth), (int) (offsetY + y*midCellHeight));
                        var right = new Point((int) (offsetX + x*cellWidth + cellWidth + midCellWidth), (int) (offsetY + y*midCellHeight + midCellHeight));
                        var down = new Point((int) (offsetX + x*cellWidth + cellWidth), (int) (offsetY + y*midCellHeight + cellHeight));
                        Cells[cellId++].Points = new[] {left, top, right, down};
                    }
            }

            RealCellHeight = (int)cellHeight;
            RealCellWidth = (int)cellWidth;
        }

        public void Draw(Graphics g)
        {
            ApplyQuality(g);

            g.Clear(BackColor);

            var pen = new Pen(ForeColor);

            foreach (MapCell cell in Cells)
            {
                if (cell.IsInRectange(g.ClipBounds))
                {
                    cell.DrawBackground(this, g, DrawMode);
                    cell.DrawForeground(this, g, DrawMode);
                }
            }

            if (ViewGrid)
                foreach (MapCell cell in Cells)
                    if (cell.IsInRectange(g.ClipBounds))
                        cell.DrawBorder(g, pen);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e.Graphics);

            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            BuildMap();
            Invalidate();

            base.OnResize(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_mouseDown)
            {
                var cell = GetCell(e.Location);
                if (m_holdedCell != null && m_holdedCell != cell)
                {
                    OnCellClicked(m_holdedCell, e.Button, true);
                    m_holdedCell = cell;
                }
                if (cell != null)
                    OnCellClicked(cell, e.Button, true);
            }

            if (TraceOnOver)
            {
                var cell = GetCell(e.Location);
                Rectangle rect = Rectangle.Empty;
                MapCell last = null;

                if (CurrentCellOver != null && CurrentCellOver != cell)
                {
                    CurrentCellOver.MouseOverPen = null;

                    rect = CurrentCellOver.Rectangle;
                    last = CurrentCellOver;
                }

                if (cell != null)
                {
                    cell.MouseOverPen = new Pen(BorderColorOnOver, 1);

                    rect = rect != Rectangle.Empty ? Rectangle.Union(rect, cell.Rectangle) : cell.Rectangle;

                    CurrentCellOver = cell;
                }

                OnCellOver(cell, last);

                if (rect != Rectangle.Empty)
                {
                    Invalidate(rect);
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var cell = GetCell(e.Location);

            if (cell != null)
            {
                m_holdedCell = m_cellOnDown = cell;
            }

            m_mouseDown = true;

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_mouseDown = false;

            var cell = GetCell(e.Location);

            if (m_holdedCell != null)
            {
                OnCellClicked(m_holdedCell, e.Button, cell != m_cellOnDown);
                m_holdedCell = null;
            }

            base.OnMouseUp(e);
        }

        public MapCell GetCell(Point p)
        {
            var searchRect = new Rectangle(p.X - RealCellWidth, p.Y - RealCellHeight, RealCellWidth, RealCellHeight);

            return Cells.FirstOrDefault(cell => cell.IsInRectange(searchRect) && PointInPoly(p, cell.Points));
        }

        public MapCell GetCell(int id)
        {
            return Cells.FirstOrDefault(cell => cell.Id == id);
        }

        public void Invalidate(MapCell cell)
        {
            Invalidate(cell.Rectangle);
        }

        public void Invalidate(params MapCell[] cells)
        {
            if (cells.Length == 0)
                base.Invalidate();
            else
                Invalidate(cells as IEnumerable<MapCell>);
        }

        public void Invalidate(IEnumerable<MapCell> cells)
        {
            Invalidate(cells.Select(entry => entry.Rectangle).Aggregate(Rectangle.Union));
        }

        public static bool PointInPoly(Point p, Point[] poly)
        {
            int xnew, ynew;
            int xold, yold;
            int x1, y1;
            int x2, y2;
            bool inside = false;

            if (poly.Length < 3)
                return false;

            xold = poly[poly.Length - 1].X;
            yold = poly[poly.Length - 1].Y;

            foreach (Point t in poly)
            {
                xnew = t.X;
                ynew = t.Y;

                if (xnew > xold)
                {
                    x1 = xold;
                    x2 = xnew;
                    y1 = yold;
                    y2 = ynew;
                }
                else
                {
                    x1 = xnew;
                    x2 = xold;
                    y1 = ynew;
                    y2 = yold;
                }

                if ((xnew < p.X) == (p.X <= xold) && (p.Y - (long) y1)*(x2 - x1) < (y2 - (long) y1)*(p.X - x1))
                {
                    inside = !inside;
                }
                xold = xnew;
                yold = ynew;
            }
            return inside;
        }
    }

    public class MapCell
    {
        public static CellState HighestState = Enum.GetValues(typeof(CellState)).Cast<CellState>().Max();

        public int Id;

        private Point[] m_points;

        public MapCell(int id)
        {
            Id = id;
            Active = true;
        }

        public Point[] Points
        {
            get { return m_points; }
            set
            {
                m_points = value;

                RefreshBounds();
            }
        }

        public bool Active
        {
            get;
            set;
        }

        public CellState State
        {
            get;
            set;
        }

        public Brush CustomBrush
        {
            get;
            set;
        }

        public Pen CustomBorderPen
        {
            get;
            set;
        }

        public Pen MouseOverPen
        {
            get;
            set;
        }

        private List<Image> m_overlayImages = new List<Image>();

        public List<Image> OverlayImages
        {
            get { return m_overlayImages; }
            set
            {
                m_overlayImages = value;
                RefreshBounds();
            }
        }

        public string Text
        {
            get;
            set;
        }

        public Brush TextBrush
        {
            get;
            set;
        }

        public Point Center
        {
            get { return new Point((Points[0].X + Points[2].X)/2, (Points[1].Y + Points[3].Y)/2); }
        }

        public int Height
        {
            get { return Points[3].Y - Points[1].Y; }
        }

        public int Width
        {
            get
            {
                return Points[2].X - Points[0].X;
            }
        }

        public Rectangle Rectangle
        {
            get;
            private set;
        }

        public void RefreshBounds()
        {
            int x = Points.Min(entry => entry.X);
            int y = Points.Min(entry => entry.Y);

            int width = Points.Max(entry => entry.X) - x;
            int height = Points.Max(entry => entry.Y) - y;

            Rectangle = new Rectangle(x, y, width, height);

            if (OverlayImages != null)
            {
                foreach (var image in OverlayImages)
                {
                    var rect = new Rectangle(Center.X - image.Width / 2, Center.Y - image.Height / 2, image.Width, image.Height);
                    Rectangle = Rectangle.Union(Rectangle, rect);
                }
            }
        }

        public virtual void DrawBorder(Graphics g, Pen pen)
        {
            if (Points != null)
            {
                g.DrawPolygon(MouseOverPen ?? (CustomBorderPen ?? pen), Points);
            }
        }

        public virtual void DrawBackground(MapControl parent, Graphics g, DrawMode mode)
        {
            Brush brush = GetDefaultBrush(parent);

            if (!Active)
            {
                brush = new SolidBrush(parent.InactiveCellColor);
            }
            else if (CustomBrush != null)
            {
                brush = CustomBrush;
            }
            else
            {
                // draw the less important states first
                for (CellState state = HighestState; state > CellState.None; state = (CellState)( (int)state >> 1 ))
                {
                    if (State.HasFlag(state) && IsStateValid(state, mode) && parent.StatesColors.ContainsKey(state))
                        brush = new SolidBrush(parent.StatesColors[state]);
                }
            }

            if (Points != null)
                g.FillPolygon(brush, Points);
        }

        public virtual void DrawForeground(MapControl parent, Graphics g, DrawMode mode)
        {
            if (mode == DrawMode.All && OverlayImages != null)
            {
                foreach (var image in OverlayImages)
                {
                    g.DrawImage(image, Center.X - image.Width / 2, Center.Y - image.Height / 2);
                }
            }

            var format = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
            g.DrawString(Text, parent.Font, TextBrush ?? Brushes.Black, new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height), format);
        }

        protected virtual bool IsStateValid(CellState state, DrawMode mode)
        {
            if (mode == DrawMode.None)
                return false;

            if (mode == DrawMode.All)
                return true;

            if ((state.HasFlag(CellState.Walkable) || state.HasFlag(CellState.NonWalkable)) && mode.HasFlag(DrawMode.Movements))
                return true;

            if ((state.HasFlag(CellState.BluePlacement) || state.HasFlag(CellState.RedPlacement)) && mode.HasFlag(DrawMode.Fights))
                return true;

            if (state.HasFlag(CellState.Trigger) && mode.HasFlag(DrawMode.Triggers))
                return true;

            if (state.HasFlag(CellState.Road) && mode.HasFlag(DrawMode.Others))
                return true;

            return false;
        }

        public virtual Brush GetDefaultBrush(MapControl parent)
        {
            return new SolidBrush(Active ? parent.ActiveCellColor : parent.InactiveCellColor);
        }

        public bool IsInRectange(Rectangle rect)
        {
            return Rectangle.IntersectsWith(rect);
        }

        public bool IsInRectange(RectangleF rect)
        {
            return Rectangle.IntersectsWith(Rectangle.Ceiling(rect));
        }
    }
}