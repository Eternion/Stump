// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Stump.Tools.DataLoader
{
    public partial class FormMain : Form
    {
        private static readonly Dictionary<string, Type> m_adapters = new Dictionary<string, Type>();
        private ToolStripItem m_currentMenuItem;
        private ToolStripItem m_firstMenuItem;

        static FormMain()
        {
            foreach (
                Type type in
                    typeof (IFileAdapter).Assembly.GetTypes().Where(
                        entry => entry.GetInterfaces().Contains(typeof (IFileAdapter))))
            {
                var adapter = (IFileAdapter) Activator.CreateInstance(type);

                m_adapters.Add(adapter.ExtensionSupport, type);
            }
        }

        public FormMain()
        {
            InitializeComponent();

            m_firstMenuItem = menuStrip1.Items[0];
        }

        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            string filter = m_adapters.Aggregate("",
                                                 (current, adapter) =>
                                                 current + string.Format("{0} files (*.{1})|*.{1}|", adapter.Key, adapter.Key));

            filter = filter.Remove(filter.Length - 1);

            var dialog = new OpenFileDialog
                {
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Filter = filter,
                    Multiselect = false
                };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                OpenFile(dialog.FileName);
            }
        }

        private void OpenFile(string fileName)
        {
            try
            {
                string ext = Path.GetExtension(fileName).Remove(0, 1).ToLower();

                if (!m_adapters.ContainsKey(ext))
                    throw new Exception(ext + " is not a valid extension");

                var adapter = (IFileAdapter)Activator.CreateInstance(m_adapters[ext], fileName);

                adapter.Open();

                adapter.Form.MdiParent = this;
                adapter.Form.Show();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, "Cannot open this file : " + e.Message, "Cannot open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormMainMdiChildActivate(object sender, EventArgs e)
        {
            if (m_currentMenuItem != null && m_currentMenuItem.Tag is IFileAdapter)
                menuStrip1.Items.Remove(m_currentMenuItem);

            if (ActiveMdiChild != null)
            {
                m_currentMenuItem = ((IFormAdapter) ActiveMdiChild).Adapter.MenuItem;
                m_currentMenuItem.MergeAction = MergeAction.Insert;
                menuStrip1.Items.Insert(menuStrip1.Items.Count - 2, m_currentMenuItem);
                m_currentMenuItem.Tag = ( (IFormAdapter)ActiveMdiChild ).Adapter;
            }
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormMainDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void FormMainDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                var files = (object[])e.Data.GetData(DataFormats.FileDrop, false);

                foreach (var file in files)
                {
                    OpenFile(file.ToString());
                }
            }
        }
    }
}