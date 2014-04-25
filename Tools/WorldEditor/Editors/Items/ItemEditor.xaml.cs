using System.Windows;
using System.Windows.Controls;
using DBSynchroniser.Records;
using Stump.DofusProtocol.D2oClasses;

namespace WorldEditor.Editors.Items
{
    /// <summary>
    /// Interaction logic for ItemEditor.xaml
    /// </summary>
    public partial class ItemEditor
    {
        public ItemEditor(ItemRecord item)
        {
            InitializeComponent();
            DataContext = ModelView = new ItemEditorModelView(item);
        }

        public ItemEditor(ItemWrapper wrapper)
        {
            InitializeComponent();
            DataContext = ModelView = new ItemEditorModelView(wrapper);
        }

        public ItemEditorModelView ModelView
        {
            get;
            set;
        }

        private void EffectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModelView.RemoveEffectCommand.RaiseCanExecuteChanged();
            ModelView.EditEffectCommand.RaiseCanExecuteChanged();
        }

        private void ItemIdEdit_OnClick(object sender, RoutedEventArgs e)
        {
            ItemIdField.IsEnabled = !ItemIdField.IsEnabled;
        }
    }
}
