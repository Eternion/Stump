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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Tools.QuickItemEditor.Models;

namespace Stump.Tools.QuickItemEditor
{
    /// <summary>
    /// Interaction logic for ItemEditor.xaml
    /// </summary>
    public partial class ItemEditor : UserControl
    {
        private ItemTemplate[] m_items;

        public ItemEditor()
        {
            InitializeComponent();
        }

        public ItemEditor(DatabaseAccessor dbAccessor)
        {
            InitializeComponent();
            m_items = ItemManager.Instance.GetTemplates().ToArray();
            itemsList.ItemsSource = m_items;        
        }

        public ItemTemplate SelectedItem
        {
            get { return itemsList.SelectedItem as ItemTemplate; }
        }

        private void OnSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            SelectedItem.Save();
        }
    }
}
