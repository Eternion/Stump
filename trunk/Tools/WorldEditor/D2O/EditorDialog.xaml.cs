using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WorldEditor.D2O
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
