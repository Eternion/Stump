using System;
using System.Collections.Generic;
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

namespace WorldEditor.Editors.Files.D2O
{
    /// <summary>
    /// Interaction logic for SelectFieldsDialog.xaml
    /// </summary>
    public partial class SelectFieldsDialog : Window
    {
        public SelectFieldsDialog()
        {
            InitializeComponent();
            SelectedItems = new List<string>();
        }

        public List<string> FieldsSource
        {
            get;
            set;
        }

        public List<string> SelectedItems
        {
            get;
            set;
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox) sender;
            var field = (string) checkbox.Content;

            if (checkbox.IsChecked == true)
            {
                if (!SelectedItems.Contains(field))
                    SelectedItems.Add(field);
            }
            else
                SelectedItems.Remove(field);
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
