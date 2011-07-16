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

        public DataGridViewRow AddRow(params object[] values)
        {
            var row = new DataGridViewRow();

            foreach (object value in values)
            {
                if (value is IEnumerable && !(value is string))
                {
                    var cell = new DataGridViewTextBoxCell
                        {
                            Value = GetEnumerableValue(value as IEnumerable),
                            Tag = value,
                        };
                    row.Cells.Add(cell);
                }
                else
                {
                    var cell = new DataGridViewTextBoxCell
                        {
                            Value = value,
                            Tag = value,
                        };
                    row.Cells.Add(cell);
                }
            }

            m_dataView.Rows.Add(row);

            return row;
        }

        public DataGridViewRow[] AddRows(IEnumerable<object[]> values)
        {
            var rows = new List<DataGridViewRow>();

            foreach (var value in values)
            {
                var row = new DataGridViewRow();

                foreach (object obj in value)
                {
                    if (obj is IEnumerable && !(obj is string))
                    {
                        var cell = new DataGridViewTextBoxCell
                        {
                            Value = GetEnumerableValue(obj as IEnumerable),
                            Tag = obj,
                        };
                        row.Cells.Add(cell);
                    }
                    else
                    {
                        var cell = new DataGridViewTextBoxCell
                        {
                            Value = obj,
                            Tag = obj,
                        };
                        row.Cells.Add(cell);
                    }
                }

                rows.Add(row);
            }

            var result = rows.ToArray();

            m_dataView.Rows.AddRange(result);
            return result;
        }

        private static string GetEnumerableValue(IEnumerable enumerable)
        {
            string result = "{";

            foreach (var value in enumerable)
            {
                if (value is IEnumerable && !( value is string ))
                    result += value + ", ";
                else
                    result += value + ", ";
            }

            return result + "}";
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
