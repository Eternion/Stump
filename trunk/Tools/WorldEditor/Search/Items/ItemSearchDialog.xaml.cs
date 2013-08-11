using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using WorldEditor.Editors.Items;
using WorldEditor.Loaders.D2O;
using WorldEditor.Loaders.I18N;
using WorldEditor.Loaders.Icons;

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
