using System;
using System.Collections.Generic;
using System.IO;
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
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;
using Stump.DofusProtocol.D2oClasses.Tools.Dlm;
using WorldEditor.Config;
using WorldEditor.Editors.Files.D2I;
using WorldEditor.Editors.Files.D2O;
using WorldEditor.Editors.Files.D2P;
using WorldEditor.Helpers;
using WorldEditor.Maps;
using WorldEditor.Meta;
using WorldEditor.Search.Items;

namespace WorldEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new StartModelView();
        }

        private void MdiContainer_PreviewDragEnter(object sender, DragEventArgs e)
        {
            bool isCorrect = false;

            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                foreach (string filename in filenames)
                {
                    var ext = System.IO.Path.GetExtension(filename);
                    if ((ext == ".d2p" || ext == ".d2i" || ext==".d2o" || ext==".d2os" || ext==".meta" || ext==".dlm") && File.Exists(filename))
                    {
                        isCorrect = true;
                        break;
                    }
                }
            }

            e.Effects = isCorrect ? DragDropEffects.All : DragDropEffects.None;
            e.Handled = true; 
        }

        private void MdiContainer_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                foreach (string filename in filenames)
                {
                    var position = PointToScreen(Mouse.GetPosition(this));
                    var newWindowThread = new Thread(() => ThreadStartingPoint(filename, position));
                    newWindowThread.SetApartmentState(ApartmentState.STA);
                    newWindowThread.IsBackground = true;
                    newWindowThread.Start();
                }
            }
        }

        private void ThreadStartingPoint(string filename, Point mousePosition)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            if (!File.Exists(filename))
                return;

            Window window = null;
            if (System.IO.Path.GetExtension(filename) == ".d2p")
            {
                window = new D2PEditor(new D2pFile(filename));
            }
            else if (System.IO.Path.GetExtension(filename) == ".d2i")
            {
                window = new D2IEditor(new D2IFile(filename));
            }
            else if (System.IO.Path.GetExtension(filename) == ".d2o")
            {
                window = new D2OEditor(filename);
            }
            else if (System.IO.Path.GetExtension(filename) == ".d2os")
            {
                window = new D2OEditor(filename);
            }
            else if (System.IO.Path.GetExtension(filename) == ".meta")
            {
                window = new MetaEditor(new MetaFile(filename));
            }
            else if (System.IO.Path.GetExtension(filename) == ".dlm")
            {
                window = new MapEditor(new DlmReader(filename, Settings.LoaderSettings.GenericMapDecryptionKey));
            }
            else
                return;


            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = mousePosition.X - window.Width / 2;
            window.Top = mousePosition.Y - window.Height / 2;
            window.Show();
            System.Windows.Threading.Dispatcher.Run();
        }
    }
}
