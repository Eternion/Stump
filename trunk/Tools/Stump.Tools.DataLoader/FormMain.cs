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
                    throw new Exception(ext + " is not a valide extension");

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
            if (menuStrip1.Items[1].Tag is IFileAdapter)
                menuStrip1.Items.RemoveAt(1);

            if (ActiveMdiChild != null)
            {
                menuStrip1.Items.Insert(1, ((FormAdapter) ActiveMdiChild).Adapter.MenuItem);
                menuStrip1.Items[1].Tag = ((FormAdapter) ActiveMdiChild).Adapter;
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