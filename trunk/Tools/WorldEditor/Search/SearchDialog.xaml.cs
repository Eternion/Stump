using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorldEditor.Search
{
    /// <summary>
    /// Interaction logic for SearchDialog.xaml
    /// </summary>
    public partial class SearchDialog : UserControl 
    {
        public SearchDialog()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ResultItemTemplateProperty =
            DependencyProperty.Register("ResultItemTemplate", typeof (DataTemplate), typeof (SearchDialog), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ResultItemTemplate
        {
            get { return (DataTemplate) GetValue(ResultItemTemplateProperty); }
            set { SetValue(ResultItemTemplateProperty, value); }
        }

        private void ResultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
