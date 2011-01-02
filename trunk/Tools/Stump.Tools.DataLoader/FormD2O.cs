using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Stump.Tools.DataLoader
{
    public partial class FormD2O : Form, IFormAdapter
    {
        public FormD2O(D2OAdapater adapter)
        {
            InitializeComponent();

            Adapter = adapter;
        }

        public void DefineColumns(params string[] columns)
        {
            m_dataView.ColumnCount = columns.Length;
            for (int i = 0; i < columns.Length; i++)
            {
                m_dataView.Columns[i].HeaderText = columns[i];
                m_dataView.Columns[i].Name = columns[i];
            }
        }

        public void AddRow(params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] is IEnumerable<IEnumerable>)
                {
                    values[i] = "{" +
                        string.Join("}, {",
                                    ( values[i] as IEnumerable<IEnumerable> ).Select(
                                       entry => string.Join(", ", entry.Cast<object>().Select(subentry => subentry.ToString()).ToArray())).ToArray()) + "}";
                }
                else if (values[i] is IEnumerable && !(values[i] is string))
                {
                    values[i] = "{" + string.Join(", ", ( (IEnumerable)values[i] ).Cast<object>().Select(entry => entry.ToString()).ToArray()) + "}";
                }
            }

            m_dataView.Rows.Add(values);
        }

        IFileAdapter IFormAdapter.Adapter
        {
            get { return Adapter; }
        }

        public D2OAdapater Adapter
        {
            get;
            set;
        }
    }
}
