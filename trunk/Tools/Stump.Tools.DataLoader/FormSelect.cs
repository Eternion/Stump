using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stump.Tools.DataLoader
{
    public partial class FormSelect : Form
    {
        public FormSelect(string[] items)
        {
            InitializeComponent();

            m_checkedListBox.Items.AddRange(items);
        }

        public FormSelect(string[] items, IEnumerable<string> selectedItems)
            : this(items)
        {
            foreach (var selectedItem in selectedItems)
            {
                int index = Array.FindIndex(items, entry => entry == selectedItem);

                if (index != -1)
                    m_checkedListBox.SetItemChecked(index, true);
            }
        }

        public string[] SelectedStrings
        {
            get
            {
                return (from object item in m_checkedListBox.CheckedItems
                       select item.ToString()).ToArray();
            }
        }
    }
}
