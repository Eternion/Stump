using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;
using WPF.MDI;

namespace WorldEditor.D2P
{
    /// <summary>
    /// Interaction logic for D2PEditor.xaml
    /// </summary>
    public partial class D2PEditor : Window
    {
        public D2PEditor(D2pFile file)
        {
            InitializeComponent();
            ModelView = new D2PEditorModelView(this, file);
            DataContext = ModelView;
        }

        public D2PEditorModelView ModelView
        {
            get;
            private set;
        }

        private void MouseDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            ModelView.ExploreFolderCommand.Execute(((DataGridRow)e.Source).Item);
        }

        private void FilesGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ModelView.RemoveFileCommand.RaiseCanExecuteChanged();
            ModelView.ExtractCommand.RaiseCanExecuteChanged();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ModelView.Dispose();
        }

        private void FilesGrid_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                if (filenames.Any(File.Exists))
                    e.Effects = DragDropEffects.Copy;
            }

            e.Handled = true; 
        }

        private void FilesGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                foreach (var filename in filenames)
                {
                    if (File.Exists(filename))
                    {
                        ModelView.AddFile(filename);
                    }
                }
            }

            e.Handled = true; 
        }
    }
}
