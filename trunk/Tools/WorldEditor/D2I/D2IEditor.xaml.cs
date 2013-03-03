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
using System.Windows.Shapes;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;

namespace WorldEditor.D2I
{
    /// <summary>
    /// Interaction logic for D2IEditor.xaml
    /// </summary>
    public partial class D2IEditor : Window
    {
        public D2IEditor(D2IFile file)
        {
            InitializeComponent();
            ModelView = new D2IEditorModelView(this, file);
            DataContext = ModelView;
        }

        public D2IEditorModelView ModelView
        {
            get;
            private set;
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            ExpanderRow.Height = GridLength.Auto;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpanderRow.Height = GridLength.Auto;
        }

        private void TextsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModelView.RemoveRowCommand.RaiseCanExecuteChanged();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ModelView.FindCommand.RaiseCanExecuteChanged();
        }
    }
}
