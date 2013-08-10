using System.ComponentModel;
using System.Windows;

namespace WorldEditor.Editors.Files.D2O
{
    /// <summary>
    /// Interaction logic for EditorDialog.xaml
    /// </summary>
    public partial class EditorDialog : Window, INotifyPropertyChanged
    {
        private readonly FrameworkElement m_editor;

        public EditorDialog(FrameworkElement editor)
        {
            m_editor = editor;
            InitializeComponent();
            Container.Content = editor;
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ( (IPersistableChanged)m_editor ).PersistChanges();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
