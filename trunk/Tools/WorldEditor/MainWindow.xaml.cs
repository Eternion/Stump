using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;
using Stump.DofusProtocol.D2oClasses.Tools.Dlm;
using WorldEditor.Config;
using WorldEditor.Editors.Files.D2I;
using WorldEditor.Editors.Files.D2O;
using WorldEditor.Editors.Files.D2P;
using WorldEditor.Maps;
using WorldEditor.Meta;

namespace WorldEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new StartModelView();
        }

        private void MdiContainer_PreviewDragEnter(object sender, DragEventArgs e)
        {
            var isCorrect = false;

            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                if ((from filename in filenames let ext = Path.GetExtension(filename) where (ext == ".d2p" || ext == ".d2i" || ext==".d2o" || ext==".d2os" || ext==".meta" || ext==".dlm") && File.Exists(filename) select filename).Any())
                {
                    isCorrect = true;
                }
            }

            e.Effects = isCorrect ? DragDropEffects.All : DragDropEffects.None;
            e.Handled = true; 
        }

        private void MdiContainer_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop, true))
                return;

            var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            foreach (var newWindowThread in from filename in filenames let position = PointToScreen(Mouse.GetPosition(this)) select new Thread(() => ThreadStartingPoint(filename, position)))
            {
                newWindowThread.SetApartmentState(ApartmentState.STA);
                newWindowThread.IsBackground = true;
                newWindowThread.Start();
            }
        }

        private static void ThreadStartingPoint(string filename, Point mousePosition)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            if (!File.Exists(filename))
                return;

            Window window;
            switch (Path.GetExtension(filename))
            {
                case ".d2p":
                    window = new D2PEditor(new D2pFile(filename));
                    break;
                case ".d2i":
                    window = new D2IEditor(new D2IFile(filename));
                    break;
                case ".d2o":
                    window = new D2OEditor(filename);
                    break;
                case ".d2os":
                    window = new D2OEditor(filename);
                    break;
                case ".meta":
                    window = new MetaEditor(new MetaFile(filename));
                    break;
                case ".dlm":
                    window = new MapEditor(new DlmReader(filename, Settings.LoaderSettings.GenericMapDecryptionKey));
                    break;
                default:
                    return;
            }


            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = mousePosition.X - window.Width / 2;
            window.Top = mousePosition.Y - window.Height / 2;
            window.Show();
            System.Windows.Threading.Dispatcher.Run();
        }
    }
}
