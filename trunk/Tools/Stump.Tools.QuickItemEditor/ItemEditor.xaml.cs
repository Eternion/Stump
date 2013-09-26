using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.I18n;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Items;
using Stump.Tools.QuickItemEditor.Models;

namespace Stump.Tools.QuickItemEditor
{
    /// <summary>/// Interaction logic for ItemEditor.xaml
    /// </summary>
    public partial class ItemEditor : UserControl
    {
        private readonly DatabaseAccessor m_dbAccessor;
        private readonly Languages m_language;

        public static readonly DependencyProperty SearchValidityProperty =
            DependencyProperty.Register("SearchValidity", typeof (bool), typeof (ItemEditor),
                                        new UIPropertyMetadata(false));


        public ItemEditor()
        {
            InitializeComponent();
        }

        public ItemEditor(DatabaseAccessor dbAccessor, Languages language)
        {
            m_dbAccessor = dbAccessor;
            m_language = language;
            InitializeComponent();
            itemsList.ItemsSource = Items;
        }

        public ItemTemplateModel[] Items
        {
            get;
            set;
        }

        public bool SearchValidity
        {
            get { return (bool) GetValue(SearchValidityProperty); }
            set { SetValue(SearchValidityProperty, value); }
        }

        public ItemTemplateModel SelectedItem
        {
            get
            {
                return itemsList.SelectedItem as ItemTemplateModel;
            }
        }

        private void OnSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            saveButton.IsEnabled = false;
            m_dbAccessor.Database.Update(SelectedItem.Template);
            saveButton.IsEnabled = true;

            MessageBox.Show(string.Format("Item '{0}' saved !", SelectedItem.Name), "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool Search(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                Items = new ItemTemplateModel[0];
            }
            else
            {

                var SQuery = string.Format("SELECT * FROM langs WHERE {0} COLLATE UTF8_GENERAL_CI LIKE '%{1}%'",
                                           m_language, s.Replace("'", "\\'"));

                var texts = m_dbAccessor.Database.Query<LangText>(SQuery)
                    .ToDictionary(entry => entry.Id);

                if (texts.Count == 0)
                    Items = new ItemTemplateModel[0];
                else
                {
                    var items =
                        m_dbAccessor.Database.Query<ItemTemplate>(
                            string.Format("SELECT * FROM items_templates WHERE NameId IN ({0})",
                                          string.Join(",", texts.Keys)));
                    items =
                        items.Concat(
                            m_dbAccessor.Database.Query<WeaponTemplate>(
                                string.Format("SELECT * FROM items_templates_weapons WHERE NameId IN ({0})",
                                              string.Join(",", texts.Keys))));

                    Items =
                        items.Select(
                            entry =>
                            new ItemTemplateModel(entry, TextManager.Instance.GetText(texts[entry.NameId], m_language)))
                             .ToArray();


                }
            }

            itemsList.ItemsSource = Items;

            return Items.Length > 0;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void OnSearchBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchValidity = Search(searchTextBox.Text);
            }
        }

        private void OnSearchButtonClicked(object sender, RoutedEventArgs e)
        {
            SearchValidity = Search(searchTextBox.Text);
        }
    }
}