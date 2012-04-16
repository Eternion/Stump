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

namespace Stump.Tools.QuickItemEditor
{
    /// <summary>
    /// Interaction logic for YesNoComboBox.xaml
    /// </summary>
    public partial class YesNoComboBox : ComboBox
    {
        public YesNoComboBox()
        {
            InitializeComponent();

            SelectedValuePath = "Key";
            Items.Add(new KeyValuePair<bool, string>(true, "Yes"));
            Items.Add(new KeyValuePair<bool, string>(false, "No"));
        }
    }
}
