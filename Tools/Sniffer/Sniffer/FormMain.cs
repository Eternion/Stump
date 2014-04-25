using System;
using System.Threading;
using System.Windows.Forms;
using Message = Stump.DofusProtocol.Messages.Message;

//using Sniffer;
//using Message = Sniffer.SnifferMessage;

namespace Stump.Tools.Sniffer
{
    public enum Category { Errors, Map, Move, Fight, Connexion, Chat, Others };
    
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

        public int GetImageId(IdentifiedMessage message)
        {
            switch (message.MsgCategory())
            {
                case Category.Errors:
                    return 1;
                case Category.Chat:
                    return 55;
                case Category.Connexion:
                    return 82;
                case Category.Fight:
                    return 84;
                case Category.Map:
                    return 32;
                case Category.Move:
                    return 95;
                case Category.Others:
                    return 149 + ((int)message.GetMsgId % 9);
            }
            return 0;
        }

        private void AddToListView(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("obj is null");

            var identifiedMessage = obj as IdentifiedMessage;

            ListViewItem item;
            if (String.IsNullOrEmpty(identifiedMessage.Error))
                // No error
                item = new ListViewItem(new string[3]
                                     {
                                         identifiedMessage.GetMsgId.ToString(),
                                         identifiedMessage.Message.ToString()/*.GetType().Name*/, identifiedMessage.Sender
                                     },
                                     GetImageId(identifiedMessage));
            else
                // Message with error (total or partial)
                item = new ListViewItem(new string[3]
                                     {
                                         identifiedMessage.GetMsgId.ToString(),
                                         identifiedMessage.Message.ToString()/*.GetType().Name*/, identifiedMessage.Sender
                                     }, GetImageId(identifiedMessage),
                                     System.Drawing.Color.Red, System.Drawing.Color.Gray, null);
            item.Tag = identifiedMessage.Message;

            messageListView.Items.Add(item);
        }

        private void messageListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lv = sender as ListView;

            if (lv.SelectedItems.Count == 0)
                return;
            messageListView.BeginUpdate();
            messageTreeView.BeginUpdate();
            messageTreeView.Nodes.Clear();
            foreach (ListViewItem item in lv.SelectedItems)
            {
                Parser.ToTreeView(messageTreeView, item.Tag as Message);

                Application.DoEvents();
            }
            messageTreeView.EndUpdate();
            messageListView.EndUpdate();            
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
            messageListView.BeginUpdate();
            foreach (ListViewItem selectedItem in messageListView.SelectedItems)
                selectedItem.Remove();
            messageListView.EndUpdate();            
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
                Error = null;
            }

            public uint GetMsgId
            {
                get { return Message.MessageId; }
            }

            public string Error { get; set; }

            public Category MsgCategory()
            {
                if (!String.IsNullOrEmpty(Error)) return Category.Errors;
                if (Message.GetType().Name.IndexOf("Movement", StringComparison.InvariantCultureIgnoreCase) >= 0) return Category.Move;
                if (Message.GetType().Name.IndexOf("Map", StringComparison.InvariantCultureIgnoreCase) >= 0) return Category.Map;
                if ((Message.GetType().Name.IndexOf("Fight", StringComparison.InvariantCultureIgnoreCase) >= 0) ||
                     (Message.GetType().Name == "LifePointsRegenEndMessage") ||
                     (Message.GetType().Name == "GameEntitiesDispositionMessage") ||
                     (Message.GetType().Name == "ChallengeInfoMessage") ||
                     (Message.GetType().Name == "CharacterStatsListMessage") ||
                     (Message.GetType().Name == "SequenceEndMessage") ||
                     (Message.GetType().Name == "SequenceStartMessage") ||
                     (Message.GetType().Name == "GameActionAcknowledgementMessage"))
                    return Category.Fight;
                if ((Message.GetType().Name.IndexOf("Chat", StringComparison.InvariantCultureIgnoreCase) >= 0) ||
                    (Message.GetType().Name.IndexOf("Text", StringComparison.InvariantCultureIgnoreCase) >= 0))
                    return Category.Chat;
                if ((Message.GetType().Name == "ProtocolRequired") ||
                    (Message.GetType().Name == "HelloConnectMessage") ||
                    (Message.GetType().Name == "IdentificationMessage") ||
                    (Message.GetType().Name == "IdentificationSuccessMessage") ||
                    (Message.GetType().Name == "SelectedServerDataMessage") ||
                    (Message.GetType().Name == "ProtocolRequired") ||
                    (Message.GetType().Name == "HelloGameMessage") ||
                    (Message.GetType().Name == "AuthenticationTicketMessage") ||
                    (Message.GetType().Name == "AuthenticationTicketAcceptedMessage") ||
                    (Message.GetType().Name == "BasicTimeMessage") ||
                    (Message.GetType().Name == "QuestListMessage"))
                    return Category.Connexion;


                return Category.Others;
            }

        }

        #endregion

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_sniffer.OnClose();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            m_sniffer.Send(cBSendTo.SelectedItem as IdentifiedClient, Activator.CreateInstance(cbMessageType.SelectedItem as Type) as Message);
        }
    }
}