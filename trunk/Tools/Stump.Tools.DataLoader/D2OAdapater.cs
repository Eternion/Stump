using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.DataProvider.Data.D2oTool;

namespace Stump.Tools.DataLoader
{
    public class D2OAdapater : IFileAdapter
    {
        private static I18NFile m_lastI18NFile;

        private static readonly Dictionary<string, Type> m_typeByFileName = new Dictionary<string, Type>();

        private D2OFile m_d2oFile;
        private FormD2O m_form;

            static D2OAdapater()
        {
            foreach (Type type in typeof (AttributeAssociatedFile).Assembly.GetTypes())
            {
                var attribute = type.GetCustomAttributes(typeof (AttributeAssociatedFile), false).FirstOrDefault() as AttributeAssociatedFile;

                if (attribute != null)
                {
                    m_typeByFileName.Add(attribute.FilesName.FirstOrDefault(), type);
                }
            }
        }

        public D2OAdapater()
        {
            MenuItem = new ToolStripMenuItem("D2O");
            MenuItem.DropDownItems.Add("Convert Name's ID by Text...", null, AttachToI18N);
            MenuItem.DropDownItems.Add("Extract to JSON file...", null, ToJson);
        }

        public D2OAdapater(string file)
            : this()
        {
            FileName = file;
        }

        #region IFileAdapter Members

        public string FileName { get; private set; }

        public string ExtensionSupport
        {
            get { return "d2o"; }
        }

        public Form Form
        {
            get { return m_form; }
        }

        public ToolStripMenuItem MenuItem { get; private set; }

        public void Open()
        {
            string file = Path.GetFileNameWithoutExtension(FileName);

            if (!m_typeByFileName.ContainsKey(file))
                throw new ArgumentException(string.Format("'{0}' is not a valid D2O file", FileName));

            m_form = new FormD2O(this)
                         {
                             Text = Path.GetFileName(FileName),
                             Adapter = this
                         };
            FillDataView();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        #endregion

        private void FillDataView()
        {
            m_d2oFile = new D2OFile(FileName);

            Dictionary<int, D2OClassDefinition> classes = m_d2oFile.GetClasses();
            string[] columns = classes.Values.First().Fields.Select(entry => entry.Key).ToArray();
            m_form.DefineColumns(columns);

            var copy = new ConcurrentStack<object>();
            foreach (var @class in classes)
            {
                try
                {
                    object data = m_d2oFile.ReadObject(@class.Key);

                    if (data != null)
                    {
                        var fields =
                            data.GetType().GetFields(BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance)
                                .
                                ToDictionary(entry => entry.Name, entry => entry.GetValue(data));

                        var values = new object[columns.Length];
                        for (int i = 0; i < columns.Length; i++)
                        {
                            values[i] = fields[columns[i]];
                        }

                        var row = m_form.AddRow(values);
                        row.Tag = data;
                    }
                    else
                    {
                        m_form.AddRow("(null)");
                    }
                }
                catch
                {
                    m_form.AddRow(string.Format("Error thrown when parsing (?) <id:{0}>", @class.Key));
                }
            }
        }

        private void AttachToI18N(object sender, EventArgs eventArgs)
        {
            string[] columns = (from DataGridViewColumn entry in m_form.m_dataView.Columns
                                select entry.HeaderText).ToArray();

            var dialogSelect = new FormSelect(columns, columns.Where(entry => entry.ToLower().Contains("nameid")))
                                   {Text = @"Select columns to convert..."};

            if (dialogSelect.ShowDialog(Form) == DialogResult.OK)
            {
                var dialog = new OpenFileDialog
                                 {
                                     Title = @"Select the d2i file used to found the text by the name's id...",
                                     CheckFileExists = true,
                                     CheckPathExists = true,
                                     Filter = @"d2i files (*.d2i)|*.d2i",
                                     Multiselect = false
                                 };

                if (m_lastI18NFile != null || dialog.ShowDialog(Form) == DialogResult.OK)
                {
                    if (m_lastI18NFile == null)
                        m_lastI18NFile = new I18NFile(dialog.FileName);

                    for (int i = 0; i < m_form.m_dataView.Rows.Count; i++)
                    {
                        foreach (string column in dialogSelect.SelectedStrings)
                        {
                            if (m_form.m_dataView.Rows[i].Cells[column].Value is int)
                            {
                                string text =
                                    m_lastI18NFile.ReadText((int) m_form.m_dataView.Rows[i].Cells[column].Value);

                                m_form.m_dataView.Rows[i].Cells[column].Tag = Tuple.Create(
                                    (int) m_form.m_dataView.Rows[i].Cells[column].Value, text);

                                m_form.m_dataView.Rows[i].Cells[column].Value = text;
                            }
                            else if (m_form.m_dataView.Rows[i].Cells[column].Value is uint)
                            {
                                string text =
                                    m_lastI18NFile.ReadText((int) ((uint) m_form.m_dataView.Rows[i].Cells[column].Value));


                                m_form.m_dataView.Rows[i].Cells[column].Tag = Tuple.Create(
                                    (uint) m_form.m_dataView.Rows[i].Cells[column].Value, text);

                                m_form.m_dataView.Rows[i].Cells[column].Value = text;
                            }
                        }
                    }
                }
            }
        }

        private void ToJson(object sender, EventArgs eventArgs)
        {
            var dialog = new SaveFileDialog
                             {
                                 Title = @"Create the output JSON file",
                                 FileName = Path.GetFileNameWithoutExtension(FileName) + ".json",
                             };

            if (dialog.ShowDialog(Form) == DialogResult.OK)
            {
                var items = new List<Dictionary<string, object>>();

                foreach (DataGridViewRow row in m_form.m_dataView.Rows)
                {
                    var cells = new DataGridViewCell[row.Cells.Count];
                    row.Cells.CopyTo(cells, 0);

                    items.Add(cells.ToDictionary(
                        entry => entry.OwningColumn.HeaderText,
                        entry =>
                            {
                                if (entry.Tag is Tuple<int, string>)
                                {
                                    return (entry.Tag as Tuple<int, string>).Item2;
                                }

                                if (entry.Tag is Tuple<uint, string>)
                                {
                                    return ( entry.Tag as Tuple<uint, string> ).Item2;
                                }

                                return entry.Tag;
                            }));
                }

                var serializer = new JavaScriptSerializer();
                File.WriteAllText(dialog.FileName, serializer.Serialize(items.ToArray()));
            }
        }
    }
}