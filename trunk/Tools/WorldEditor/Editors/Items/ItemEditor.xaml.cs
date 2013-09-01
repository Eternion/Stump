using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Stump.DofusProtocol.D2oClasses;
using WorldEditor.Helpers;
using Xceed.Wpf.Toolkit;

namespace WorldEditor.Editors.Items
{
    /// <summary>
    /// Interaction logic for ItemEditor.xaml
    /// </summary>
    public partial class ItemEditor : Window
    {
        public ItemEditor(Item item)
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
    }
}
