using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stump.Tools.DataLoader.D2O;

namespace Stump.Tools.DataLoader
{
    public partial class FormD2O : Form, IFormAdapter
    {
        public FormD2O(D2OAdapater adapter)
        {
            InitializeComponent();

            m_dataView.CellContentClick += CellContentClick;

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


        private void CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            var cell = m_dataView[e.ColumnIndex, e.RowIndex];

            if (cell is DataGridViewButtonCell)
            {
                var form = new FormObjectViewer();
                form.ViewedObject = cell.Tag;

                form.Show(this);
            }
        }

        public DataGridViewRow[] AddRows(IEnumerable<IDictionary<string, object>> values)
        {
            var rows = new List<DataGridViewRow>();
            var columnKeys = m_dataView.Columns.Cast<DataGridViewColumn>().ToDictionary(column => column.HeaderText, column => column.Index);

            foreach (var value in values)
            {
                var row = new DataGridViewRow();
                row.CreateCells(m_dataView, Enumerable.Repeat(string.Empty, m_dataView.Columns.Count));

                foreach (var obj in value)
                {
                    if (obj.Value is IEnumerable && !( obj.Value is string ) &&
                        obj.Value.GetType().GetGenericArguments().All(entry => entry.IsPrimitive || entry == typeof(string)))
                    {
                        var cell = new DataGridViewTextBoxCell
                        {
                            Value = GetEnumerableValue(obj.Value as IEnumerable),
                            Tag = obj.Value,
                        };
                        row.Cells[columnKeys[obj.Key]] = cell;
                    }
                    else if (obj.Value.GetType().IsPrimitive || obj.Value is string)
                    {
                        var cell = new DataGridViewTextBoxCell
                        {
                            Value = obj.Value,
                            Tag = obj.Value,
                        };
                        row.Cells[columnKeys[obj.Key]] = cell;
                        
                    }
                    else
                    {
                        var cell = new DataGridViewButtonCell
                        {
                            Value = obj.Value.GetType().Name,
                            Tag = obj.Value,
                        };

                        row.Cells[columnKeys[obj.Key]] = cell;
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
            return string.Join(", ", enumerable.Cast<object>().Select(entry => entry.ToString()));
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
