using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DBSynchroniser.Records;
using WorldEditor.Database;
using WorldEditor.Editors.Items;

namespace WorldEditor.Search.Items
{
    /// <summary>
    /// Interaction logic for ItemSearchDialog.xaml
    /// </summary>
    public partial class ItemSearchDialog : Window
    {
        public ItemSearchDialog()
        {
            InitializeComponent();
            ModelView = new ItemSearchDialogModelView();
            DataContext = ModelView;
        }

        public ItemSearchDialogModelView ModelView
        {
            get;
            set;
        }
    }
}
