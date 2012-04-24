using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Tools.QuickItemEditor
{
    /// <summary>
    /// Interaction logic for ItemEditor.xaml
    /// </summary>
    public partial class ItemEditor : UserControl
    {
        public static readonly DependencyProperty SearchValidityProperty =
            DependencyProperty.Register("SearchValidity", typeof (bool), typeof (ItemEditor),
                                        new UIPropertyMetadata(false));


        private readonly ItemTemplate[] m_items;

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

        public bool SearchValidity
        {
            get { return (bool) GetValue(SearchValidityProperty); }
            set { SetValue(SearchValidityProperty, value); }
        }

        public ItemTemplate SelectedItem
        {
            get { return itemsList.SelectedItem as ItemTemplate; }
        }

        private void OnSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            saveButton.IsEnabled = false;
            SelectedItem.Save();
            saveButton.IsEnabled = true;
        }

        public bool Search(string s)
        {
            int skip = itemsList.SelectedItem != null ? itemsList.SelectedIndex + 1 : 0;
            ItemTemplate item = m_items.Skip(skip).FirstOrDefault(entry => entry.Name.Contains(s));

            if (item == null)
            {
                item = m_items.FirstOrDefault(entry => entry.Name.Contains(s));

                if (item == null)
                    return false;
            }

            itemsList.SelectedItem = item;
            itemsList.ScrollIntoView(item);

            return true;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            SearchValidity = Search(searchTextBox.Text);
        }

        private void OnSearchBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchValidity = Search(searchTextBox.Text);
            }
        }
    }
}