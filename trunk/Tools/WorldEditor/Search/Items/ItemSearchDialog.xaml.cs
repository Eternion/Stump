using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Stump.DofusProtocol.D2oClasses;
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
            var items = ObjectDataManager.Instance.EnumerateObjects<Item>().Select(x => x is Weapon ? new WeaponWrapper((Weapon)x) : new ItemWrapper(x));
            ModelView = new ItemSearchDialogModelView(typeof (ItemWrapper), new ObservableCollection<object>(items));
            DataContext = ModelView;
        }

        public SearchDialogModelView ModelView
        {
            get;
            set;
        }
    }
}
