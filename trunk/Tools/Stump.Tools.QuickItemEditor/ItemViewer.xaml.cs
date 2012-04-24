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
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Tools.QuickItemEditor.Models;

namespace Stump.Tools.QuickItemEditor
{
    /// <summary>
    /// Interaction logic for ItemViewer.xaml
    /// </summary>
    public partial class ItemViewer : UserControl
    {
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
            "Item", typeof(ItemTemplateModel), typeof(ItemViewer));


        public ItemViewer()
        {
            DataContext = this;
            InitializeComponent();
        }

        public ItemTemplateModel Item
        {
            get { return (ItemTemplateModel)GetValue(ItemProperty); }
            set
            {
                SetValue(ItemProperty, value);
            }
        }

        private void OnAddClicked(object sender, RoutedEventArgs e)
        {
            Item.Effects.Add(new EffectDice());
            effectsList.Items.Refresh();
        }

        private void OnRemoveClicked(object sender, RoutedEventArgs e)
        {
            Item.Effects.Remove(effectsList.SelectedItem as EffectBase);
            effectsList.Items.Refresh();
        }
    }
}
