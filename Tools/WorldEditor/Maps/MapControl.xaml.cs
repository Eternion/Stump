using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Stump.DofusProtocol.D2oClasses.Tools.Dlm;

namespace WorldEditor.Maps
{
    /// <summary>
    /// Interaction logic for MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        private bool m_panning;
        private Point m_origContentMouseDownPoint;

        public MapControl(DlmReader map)
        {
            InitializeComponent();
            ModelView = new MapModelView(this, map);
            DataContext = ModelView;
        }

        public MapModelView ModelView
        {
            get;
            private set;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            zoomAndPanControl.ScaleToFit();
        }

        private void ZoomAndPanControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed)
                return;

            m_origContentMouseDownPoint = e.GetPosition(MapElements);
            m_panning = true;

            zoomAndPanControl.CaptureMouse();
            e.Handled = true;
        }

        private void ZoomAndPanControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            zoomAndPanControl.ReleaseMouseCapture();
            m_panning = false;
            e.Handled = true;
        }

        private void ZoomAndPanControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            e.Handled = true;

            if (e.Delta > 0)
            {
                ZoomIn();
            }
            else if (e.Delta < 0)
            {
                ZoomOut();
            }
        }

        private void ZoomAndPanControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_panning)
                return;

            Point curContentMousePoint = e.GetPosition(MapElements);
            Vector dragOffset = curContentMousePoint - m_origContentMouseDownPoint;

            zoomAndPanControl.ContentOffsetX -= dragOffset.X;
            zoomAndPanControl.ContentOffsetY -= dragOffset.Y;

            e.Handled = true;
        }

        /// <summary>
        /// Zoom the viewport out, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomOut()
        {
            if (zoomAndPanControl.ContentScale <= 0.3)
                return;

            zoomAndPanControl.ZoomTo(zoomAndPanControl.ContentScale - 0.1);
        }

        /// <summary>
        /// Zoom the viewport in, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomIn()
        {
            if (zoomAndPanControl.ContentScale >= 3.0)
                return;

            zoomAndPanControl.ZoomTo(zoomAndPanControl.ContentScale + 0.1);
        }
    }
}
