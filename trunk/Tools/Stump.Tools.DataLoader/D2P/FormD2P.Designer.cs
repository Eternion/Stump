namespace Stump.Tools.DataLoader
{
    partial class FormD2P
    {/*
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && ( components != null ))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormD2P));
            this.m_iconFilesList = new System.Windows.Forms.ImageList(this.components);
            this.m_toolStrip = new System.Windows.Forms.ToolStrip();
            this.m_buttonAdd = new System.Windows.Forms.ToolStripButton();
            this.m_buttonDelete = new System.Windows.Forms.ToolStripButton();
            this.m_buttonExtract = new System.Windows.Forms.ToolStripButton();
            this.m_buttonExtractAll = new System.Windows.Forms.ToolStripButton();
            this.m_columnFileName = ( (System.Windows.Forms.ColumnHeader)( new System.Windows.Forms.ColumnHeader() ) );
            this.m_columnSize = ( (System.Windows.Forms.ColumnHeader)( new System.Windows.Forms.ColumnHeader() ) );
            this.m_columnIndex = ( (System.Windows.Forms.ColumnHeader)( new System.Windows.Forms.ColumnHeader() ) );
            this.m_columnLinkedFile = ( (System.Windows.Forms.ColumnHeader)( new System.Windows.Forms.ColumnHeader() ) );
            this.m_filesView = new System.Windows.Forms.ListView();
            this.m_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_contextMenuExtract = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStrip.SuspendLayout();
            this.m_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_iconFilesList
            // 
            this.m_iconFilesList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.m_iconFilesList.ImageSize = new System.Drawing.Size(16, 16);
            this.m_iconFilesList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // m_toolStrip
            // 
            this.m_toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.m_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_buttonAdd,
            this.m_buttonDelete,
            this.m_buttonExtract,
            this.m_buttonExtractAll});
            this.m_toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.m_toolStrip.Location = new System.Drawing.Point(0, 0);
            this.m_toolStrip.Name = "m_toolStrip";
            this.m_toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.m_toolStrip.Size = new System.Drawing.Size(342, 25);
            this.m_toolStrip.Stretch = true;
            this.m_toolStrip.TabIndex = 1;
            this.m_toolStrip.Text = "toolStrip1";
            // 
            // m_buttonAdd
            // 
            this.m_buttonAdd.Enabled = false;
            this.m_buttonAdd.Image = ( (System.Drawing.Image)( resources.GetObject("m_buttonAdd.Image") ) );
            this.m_buttonAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_buttonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_buttonAdd.Name = "m_buttonAdd";
            this.m_buttonAdd.Size = new System.Drawing.Size(46, 22);
            this.m_buttonAdd.Text = "Add";
            this.m_buttonAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_buttonDelete
            // 
            this.m_buttonDelete.Enabled = false;
            this.m_buttonDelete.Image = ( (System.Drawing.Image)( resources.GetObject("m_buttonDelete.Image") ) );
            this.m_buttonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_buttonDelete.Name = "m_buttonDelete";
            this.m_buttonDelete.Size = new System.Drawing.Size(58, 22);
            this.m_buttonDelete.Text = "Delete";
            // 
            // m_buttonExtract
            // 
            this.m_buttonExtract.Enabled = false;
            this.m_buttonExtract.Image = ( (System.Drawing.Image)( resources.GetObject("m_buttonExtract.Image") ) );
            this.m_buttonExtract.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_buttonExtract.Name = "m_buttonExtract";
            this.m_buttonExtract.Size = new System.Drawing.Size(62, 22);
            this.m_buttonExtract.Text = "Extract";
            this.m_buttonExtract.Click += new System.EventHandler(this.ButtonExtractClick);
            // 
            // m_buttonExtractAll
            // 
            this.m_buttonExtractAll.Enabled = false;
            this.m_buttonExtractAll.Image = ( (System.Drawing.Image)( resources.GetObject("m_buttonExtractAll.Image") ) );
            this.m_buttonExtractAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_buttonExtractAll.Name = "m_buttonExtractAll";
            this.m_buttonExtractAll.Size = new System.Drawing.Size(76, 22);
            this.m_buttonExtractAll.Text = "Extract All";
            this.m_buttonExtractAll.Click += new System.EventHandler(this.ButtonExtractAllClick);
            // 
            // m_columnFileName
            // 
            this.m_columnFileName.Text = "Name";
            this.m_columnFileName.Width = 138;
            // 
            // m_columnSize
            // 
            this.m_columnSize.Text = "Size";
            // 
            // m_columnIndex
            // 
            this.m_columnIndex.Text = "Index";
            // 
            // m_columnLinkedFile
            // 
            this.m_columnLinkedFile.Text = "Container";
            this.m_columnLinkedFile.Width = 80;
            // 
            // m_filesView
            // 
            this.m_filesView.AllowColumnReorder = true;
            this.m_filesView.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.m_filesView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_columnFileName,
            this.m_columnSize,
            this.m_columnIndex,
            this.m_columnLinkedFile});
            this.m_filesView.ContextMenuStrip = this.m_contextMenuStrip;
            this.m_filesView.FullRowSelect = true;
            this.m_filesView.Location = new System.Drawing.Point(0, 28);
            this.m_filesView.Name = "m_filesView";
            this.m_filesView.ShowItemToolTips = true;
            this.m_filesView.Size = new System.Drawing.Size(342, 288);
            this.m_filesView.SmallImageList = this.m_iconFilesList;
            this.m_filesView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.m_filesView.TabIndex = 0;
            this.m_filesView.UseCompatibleStateImageBehavior = false;
            this.m_filesView.View = System.Windows.Forms.View.Details;
            this.m_filesView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.FilesViewItemSelectionChanged);
            this.m_filesView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FilesViewMouseDoubleClick);
            // 
            // m_contextMenuStrip
            // 
            this.m_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_contextMenuExtract});
            this.m_contextMenuStrip.Name = "m_contextMenuStrip";
            this.m_contextMenuStrip.Size = new System.Drawing.Size(121, 26);
            // 
            // m_contextMenuExtract
            // 
            this.m_contextMenuExtract.Image = ( (System.Drawing.Image)( resources.GetObject("m_contextMenuExtract.Image") ) );
            this.m_contextMenuExtract.Name = "m_contextMenuExtract";
            this.m_contextMenuExtract.Size = new System.Drawing.Size(120, 22);
            this.m_contextMenuExtract.Text = "Extract";
            this.m_contextMenuExtract.Click += new System.EventHandler(this.ContextMenuExtractClick);
            // 
            // FormD2P
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 316);
            this.Controls.Add(this.m_filesView);
            this.Controls.Add(this.m_toolStrip);
            this.Name = "FormD2P";
            this.ShowIcon = false;
            this.Text = "FormD2P";
            this.m_toolStrip.ResumeLayout(false);
            this.m_toolStrip.PerformLayout();
            this.m_contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip m_toolStrip;
        private System.Windows.Forms.ToolStripButton m_buttonAdd;
        private System.Windows.Forms.ToolStripButton m_buttonDelete;
        private System.Windows.Forms.ImageList m_iconFilesList;
        private System.Windows.Forms.ToolStripButton m_buttonExtract;
        private System.Windows.Forms.ToolStripButton m_buttonExtractAll;
        private System.Windows.Forms.ColumnHeader m_columnFileName;
        private System.Windows.Forms.ColumnHeader m_columnSize;
        private System.Windows.Forms.ColumnHeader m_columnIndex;
        private System.Windows.Forms.ColumnHeader m_columnLinkedFile;
        private System.Windows.Forms.ListView m_filesView;
        private System.Windows.Forms.ContextMenuStrip m_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem m_contextMenuExtract;
        */
    }
}