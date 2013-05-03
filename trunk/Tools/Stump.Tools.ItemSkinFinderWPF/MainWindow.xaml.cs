using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stump.Tools.ItemSkinFinderWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ModelView = new ModelView(this);
            DataContext = ModelView;
        }

        public ModelView ModelView
        {
            get;
            set;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ModelView.LoadBitmapsAsync();
        }

        private void ImageB_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition((Image)sender);
            var pixel = ModelView.ImageB.GetPixelColor((int) pos.X, (int) pos.Y);

            ModelView.DebugString = string.Format("R:{0} G:{1} B:{2} A:{3}", pixel.R, pixel.G, pixel.B, pixel.A);
            ModelView.ColorB = pixel;
        }

        private void ImageA_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition((Image)sender);
            var pixel = ModelView.ImageA.GetPixelColor((int)pos.X, (int)pos.Y);

            ModelView.DebugString = string.Format("R:{0} G:{1} B:{2} A:{3}", pixel.R, pixel.G, pixel.B, pixel.A);
            ModelView.ColorA = pixel;
        }
    }
}
