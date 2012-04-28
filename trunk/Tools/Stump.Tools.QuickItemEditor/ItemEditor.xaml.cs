using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using NHibernate.Criterion;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.I18n;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Items;
using Stump.Tools.QuickItemEditor.Models;

namespace Stump.Tools.QuickItemEditor
{
    /// <summary>
    /// Interaction logic for ItemEditor.xaml
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
            SelectedItem.Template.Update();
            saveButton.IsEnabled = true;
        }

        public bool Search(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                Items = new ItemTemplateModel[0];
            }
            else
            {
                var texts = TextRecord.FindAll(Restrictions.InsensitiveLike(m_language.ToString(),
                                                                            string.Format("%{0}%", s)))
                    .ToDictionary(entry => entry.Id);

                Items = ItemTemplate.FindAll(Restrictions.In("NameId", texts.Keys))
                    .Select(
                        entry =>
                        new ItemTemplateModel(entry, TextManager.Instance.GetText(texts[entry.NameId], m_language))).
                    ToArray();
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