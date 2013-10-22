using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DBSynchroniser.Records;
using Stump.DofusProtocol.D2oClasses;
using WorldEditor.Database;
using WorldEditor.Editors.Items;
using WorldEditor.Loaders.D2O;

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
            var items = DatabaseManager.Instance.Database.Query<ItemRecord>("SELECT * FROM items").Select(x => new ItemWrapper(x));
            items = items.Concat(DatabaseManager.Instance.Database.Query<WeaponRecord>("SELECT * FROM weapons").Select(x => new WeaponWrapper(x)));

            ModelView = new ItemSearchDialogModelView(typeof (ItemWrapper), new ObservableCollection<object>(items.ToArray()));
            DataContext = ModelView;
        }

        public SearchDialogModelView ModelView
        {
            get;
            set;
        }
    }
}
