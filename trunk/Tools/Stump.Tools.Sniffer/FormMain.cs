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
using System.Threading;
using System.Linq;
using System.Windows.Forms;
using Message = Stump.DofusProtocol.Messages.Message;

namespace Stump.Tools.Sniffer
{
    public partial class FormMain : Form
    {
        private readonly Sniffer m_sniffer;

        public FormMain()
        {
            InitializeComponent();
            m_sniffer = new Sniffer(this);
        }

        //Start Procedure
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (m_sniffer.Running)
            {
                (sender as ToolStripButton).Text = "Start";
                m_sniffer.Stop();
            }
            else
            {
                if (m_sniffer.Start())
                    (sender as ToolStripButton).Text = "Stop";
            }
        }


        /// <summary>
        ///   Adds the message to list view.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "sender">The sender.</param>
        public void AddMessageToListView(Message message, string sender)
        {
            var identifiedMessage = new IdentifiedMessage(message, sender);

            if (messageListView.InvokeRequired)
            {
                messageListView.Invoke(new ParameterizedThreadStart(AddToListView), identifiedMessage);
            }
            else
            {
                AddToListView(identifiedMessage);
            }
        }

        private void AddToListView(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("obj is null");

            var identifiedMessage = obj as IdentifiedMessage;

            var item =
                new ListViewItem(new string[3]
                                     {
                                         identifiedMessage.Message.getMessageId().ToString(),
                                         identifiedMessage.Message.GetType().Name, identifiedMessage.Sender
                                     });
            item.Tag = identifiedMessage.Message;

            messageListView.Items.Add(item);
        }

        private void messageListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lv = sender as ListView;

            if (lv.SelectedItems.Count == 0)
                return;

            messageTreeView.Nodes.Clear();

            foreach (ListViewItem item in lv.SelectedItems)
            {
                Parser.ToTreeView(messageTreeView, item.Tag as Message);

                Application.DoEvents();
            }
        }

        //Export Selected ListViewItems message to xml
        private void BtnExportMessage_Click(object sender, EventArgs e)
        {
            if (messageListView.SelectedItems.Count == 0)
                return;

            var treeView = new TreeView();

            foreach (ListViewItem selectedItem in messageListView.SelectedItems)
            {
                Parser.ToTreeView(treeView, selectedItem.Tag as Message);
            }
            
            string folder = GetFolderDestination();
            if (folder != "")
                if (treeView.Nodes.Count>1)
                   Parser.TreeNodeToXml(Parser.AddParentNode(new TreeNode("Messages"),treeView.Nodes),folder);
                else
                    Parser.TreeNodeToXml(treeView.Nodes[0], folder);
        }

        //Remove Selected ListViewItems message
        private void BtnRemoveMessage_Click(object sender, EventArgs e)
        {
            if (messageListView.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem selectedItem in messageListView.SelectedItems)
                selectedItem.Remove();
        }

        //Remove all listViewItems message
        private void BtnRemoveAllMessage_Click(object sender, EventArgs e)
        {
            messageListView.Items.Clear();
        }

        //Export selected TreeView message
        private void BtnExportClass_Click(object sender, EventArgs e)
        {
            if (messageTreeView.SelectedNode == null)
                return;

            string folder = GetFolderDestination();
            if (folder != "")
                Parser.TreeNodeToXml(messageTreeView.SelectedNode, folder);
        }

        /// <summary>
        ///   Gets the folder destination.
        /// </summary>
        /// <returns></returns>
        private static string GetFolderDestination()
        {
            var sfd = new SaveFileDialog
                          {
                              AddExtension = true,
                              AutoUpgradeEnabled = true,
                              CheckPathExists = true,
                              DefaultExt = "xml",
                              Filter = @"XML files (*.xml)|*.xml"
                          };

            sfd.ShowDialog();
            return sfd.FileName;
        }

        #region Nested type: IdentifiedMessage

        public class IdentifiedMessage
        {
            public Message Message;
            public string Sender;

            public IdentifiedMessage(Message message, string sender)
            {
                Message = message;
                Sender = sender;
            }
        }

        #endregion

        private void FormMainFormClosing(object sender, FormClosingEventArgs e)
        {
            m_sniffer.StopLogs();
        }
    }
}