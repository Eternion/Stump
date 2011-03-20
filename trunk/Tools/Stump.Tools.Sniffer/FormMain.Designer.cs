namespace Stump.Tools.Sniffer
{
    partial class FormMain
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.messageTreeView = new System.Windows.Forms.TreeView();
            this.treeViewMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.BtnExportClass = new System.Windows.Forms.ToolStripMenuItem();
            this.messageListView = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PacketName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.From = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.BtnExportMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnRemoveMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnRemoveAllMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.topToolStrip = new System.Windows.Forms.ToolStrip();
            this.BtnStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.LbByteNumber = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.LbPacketNumber = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.treeViewMenuStrip.SuspendLayout();
            this.listViewMenuStrip.SuspendLayout();
            this.topToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageTreeView
            // 
            this.messageTreeView.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.messageTreeView.ContextMenuStrip = this.treeViewMenuStrip;
            this.messageTreeView.FullRowSelect = true;
            this.messageTreeView.Location = new System.Drawing.Point(319, 28);
            this.messageTreeView.Name = "messageTreeView";
            this.messageTreeView.Size = new System.Drawing.Size(269, 421);
            this.messageTreeView.TabIndex = 1;
            // 
            // treeViewMenuStrip
            // 
            this.treeViewMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnExportClass});
            this.treeViewMenuStrip.Name = "messageMenuStrip";
            this.treeViewMenuStrip.ShowImageMargin = false;
            this.treeViewMenuStrip.Size = new System.Drawing.Size(83, 26);
            // 
            // BtnExportClass
            // 
            this.BtnExportClass.Name = "BtnExportClass";
            this.BtnExportClass.Size = new System.Drawing.Size(82, 22);
            this.BtnExportClass.Text = "Export";
            this.BtnExportClass.Click += new System.EventHandler(this.BtnExportClass_Click);
            // 
            // messageListView
            // 
            this.messageListView.AllowColumnReorder = true;
            this.messageListView.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.messageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.PacketName,
            this.From});
            this.messageListView.ContextMenuStrip = this.listViewMenuStrip;
            this.messageListView.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.messageListView.FullRowSelect = true;
            this.messageListView.Location = new System.Drawing.Point(1, 26);
            this.messageListView.Name = "messageListView";
            this.messageListView.Size = new System.Drawing.Size(314, 423);
            this.messageListView.TabIndex = 3;
            this.messageListView.UseCompatibleStateImageBehavior = false;
            this.messageListView.View = System.Windows.Forms.View.Details;
            this.messageListView.SelectedIndexChanged += new System.EventHandler(this.messageListView_SelectedIndexChanged);
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 80;
            // 
            // PacketName
            // 
            this.PacketName.Text = "Name";
            this.PacketName.Width = 147;
            // 
            // From
            // 
            this.From.Text = "From";
            this.From.Width = 82;
            // 
            // listViewMenuStrip
            // 
            this.listViewMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnExportMessage,
            this.BtnRemoveMessage,
            this.BtnRemoveAllMessage});
            this.listViewMenuStrip.Name = "listViewMenuStrip";
            this.listViewMenuStrip.ShowImageMargin = false;
            this.listViewMenuStrip.Size = new System.Drawing.Size(110, 70);
            // 
            // BtnExportMessage
            // 
            this.BtnExportMessage.Name = "BtnExportMessage";
            this.BtnExportMessage.Size = new System.Drawing.Size(109, 22);
            this.BtnExportMessage.Text = "Export";
            this.BtnExportMessage.Click += new System.EventHandler(this.BtnExportMessage_Click);
            // 
            // BtnRemoveMessage
            // 
            this.BtnRemoveMessage.Name = "BtnRemoveMessage";
            this.BtnRemoveMessage.Size = new System.Drawing.Size(109, 22);
            this.BtnRemoveMessage.Text = "Remove";
            this.BtnRemoveMessage.Click += new System.EventHandler(this.BtnRemoveMessage_Click);
            // 
            // BtnRemoveAllMessage
            // 
            this.BtnRemoveAllMessage.Name = "BtnRemoveAllMessage";
            this.BtnRemoveAllMessage.Size = new System.Drawing.Size(109, 22);
            this.BtnRemoveAllMessage.Text = "Remove All";
            this.BtnRemoveAllMessage.Click += new System.EventHandler(this.BtnRemoveAllMessage_Click);
            // 
            // topToolStrip
            // 
            this.topToolStrip.BackColor = System.Drawing.SystemColors.ControlLight;
            this.topToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.topToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnStart,
            this.toolStripSeparator1,
            this.toolStripLabel3,
            this.LbByteNumber,
            this.toolStripSeparator5,
            this.toolStripSeparator6,
            this.toolStripLabel1,
            this.LbPacketNumber,
            this.toolStripSeparator2});
            this.topToolStrip.Location = new System.Drawing.Point(0, 0);
            this.topToolStrip.Name = "topToolStrip";
            this.topToolStrip.Size = new System.Drawing.Size(593, 25);
            this.topToolStrip.TabIndex = 4;
            this.topToolStrip.Text = "toolStrip1";
            // 
            // BtnStart
            // 
            this.BtnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(35, 22);
            this.BtnStart.Text = "Start";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(75, 22);
            this.toolStripLabel3.Text = "Bytes Reçus :";
            // 
            // LbByteNumber
            // 
            this.LbByteNumber.Name = "LbByteNumber";
            this.LbByteNumber.Size = new System.Drawing.Size(13, 22);
            this.LbByteNumber.Text = "0";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(89, 22);
            this.toolStripLabel1.Text = "Paquets Reçus :";
            // 
            // LbPacketNumber
            // 
            this.LbPacketNumber.Name = "LbPacketNumber";
            this.LbPacketNumber.Size = new System.Drawing.Size(13, 22);
            this.LbPacketNumber.Text = "0";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(593, 456);
            this.Controls.Add(this.topToolStrip);
            this.Controls.Add(this.messageListView);
            this.Controls.Add(this.messageTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StumpSniffer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMainFormClosing);
            this.treeViewMenuStrip.ResumeLayout(false);
            this.listViewMenuStrip.ResumeLayout(false);
            this.topToolStrip.ResumeLayout(false);
            this.topToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView messageTreeView;
        public System.Windows.Forms.ListView messageListView;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader PacketName;
        private System.Windows.Forms.ColumnHeader From;
        private System.Windows.Forms.ToolStrip topToolStrip;
        private System.Windows.Forms.ToolStripButton BtnStart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip listViewMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem BtnRemoveMessage;
        private System.Windows.Forms.ToolStripMenuItem BtnExportMessage;
        private System.Windows.Forms.ContextMenuStrip treeViewMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem BtnExportClass;
        private System.Windows.Forms.ToolStripMenuItem BtnRemoveAllMessage;
        public System.Windows.Forms.ToolStripLabel LbByteNumber;
        public System.Windows.Forms.ToolStripLabel LbPacketNumber;
    }
}

