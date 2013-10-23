using System.Windows;
using System.Windows.Controls;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;

namespace WorldEditor.Editors.Langs
{
    /// <summary>
    /// Interaction logic for D2IEditor.xaml
    /// </summary>
    public partial class LangEditor : Window
    {
        public LangEditor()
        {
            InitializeComponent();
            ModelView = new LangEditorModelView(this);
            DataContext = ModelView;
        }

        public LangEditorModelView ModelView
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
