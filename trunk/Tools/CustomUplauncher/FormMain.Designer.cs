namespace CustomUplauncher
{
    partial class FormMain
    {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.listBoxServers = new System.Windows.Forms.ListBox();
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.labelAddressValue = new System.Windows.Forms.Label();
            this.labelPatchUrlValue = new System.Windows.Forms.Label();
            this.labelPortsValue = new System.Windows.Forms.Label();
            this.labelIpValue = new System.Windows.Forms.Label();
            this.labelNameValue = new System.Windows.Forms.Label();
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelPatchUrl = new System.Windows.Forms.Label();
            this.labelPorts = new System.Windows.Forms.Label();
            this.labelIp = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.buttonApplyPatch = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxServers
            // 
            this.listBoxServers.FormattingEnabled = true;
            this.listBoxServers.Location = new System.Drawing.Point(12, 32);
            this.listBoxServers.Name = "listBoxServers";
            this.listBoxServers.Size = new System.Drawing.Size(220, 160);
            this.listBoxServers.TabIndex = 0;
            this.listBoxServers.SelectedIndexChanged += new System.EventHandler(this.ListBoxServersSelectedIndexChanged);
            // 
            // groupBoxServer
            // 
            this.groupBoxServer.Controls.Add(this.labelAddressValue);
            this.groupBoxServer.Controls.Add(this.labelPatchUrlValue);
            this.groupBoxServer.Controls.Add(this.labelPortsValue);
            this.groupBoxServer.Controls.Add(this.labelIpValue);
            this.groupBoxServer.Controls.Add(this.labelNameValue);
            this.groupBoxServer.Controls.Add(this.labelAddress);
            this.groupBoxServer.Controls.Add(this.labelPatchUrl);
            this.groupBoxServer.Controls.Add(this.labelPorts);
            this.groupBoxServer.Controls.Add(this.labelIp);
            this.groupBoxServer.Controls.Add(this.labelName);
            this.groupBoxServer.Controls.Add(this.buttonApplyPatch);
            this.groupBoxServer.Controls.Add(this.buttonEdit);
            this.groupBoxServer.Location = new System.Drawing.Point(12, 226);
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.Size = new System.Drawing.Size(220, 119);
            this.groupBoxServer.TabIndex = 3;
            this.groupBoxServer.TabStop = false;
            this.groupBoxServer.Text = "Serveur";
            // 
            // labelAddressValue
            // 
            this.labelAddressValue.Location = new System.Drawing.Point(106, 68);
            this.labelAddressValue.Name = "labelAddressValue";
            this.labelAddressValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelAddressValue.Size = new System.Drawing.Size(105, 13);
            this.labelAddressValue.TabIndex = 12;
            // 
            // labelPatchUrlValue
            // 
            this.labelPatchUrlValue.Location = new System.Drawing.Point(106, 55);
            this.labelPatchUrlValue.Name = "labelPatchUrlValue";
            this.labelPatchUrlValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelPatchUrlValue.Size = new System.Drawing.Size(105, 13);
            this.labelPatchUrlValue.TabIndex = 11;
            // 
            // labelPortsValue
            // 
            this.labelPortsValue.Location = new System.Drawing.Point(106, 42);
            this.labelPortsValue.Name = "labelPortsValue";
            this.labelPortsValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelPortsValue.Size = new System.Drawing.Size(105, 13);
            this.labelPortsValue.TabIndex = 10;
            // 
            // labelIpValue
            // 
            this.labelIpValue.Location = new System.Drawing.Point(106, 29);
            this.labelIpValue.Name = "labelIpValue";
            this.labelIpValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelIpValue.Size = new System.Drawing.Size(105, 13);
            this.labelIpValue.TabIndex = 9;
            // 
            // labelNameValue
            // 
            this.labelNameValue.Location = new System.Drawing.Point(106, 16);
            this.labelNameValue.Name = "labelNameValue";
            this.labelNameValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelNameValue.Size = new System.Drawing.Size(105, 13);
            this.labelNameValue.TabIndex = 8;
            // 
            // labelAddress
            // 
            this.labelAddress.Location = new System.Drawing.Point(8, 68);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(51, 13);
            this.labelAddress.TabIndex = 7;
            this.labelAddress.Text = "Adresse :";
            // 
            // labelPatchUrl
            // 
            this.labelPatchUrl.Location = new System.Drawing.Point(8, 55);
            this.labelPatchUrl.Name = "labelPatchUrl";
            this.labelPatchUrl.Size = new System.Drawing.Size(66, 13);
            this.labelPatchUrl.TabIndex = 6;
            this.labelPatchUrl.Text = "Patch URL :";
            // 
            // labelPorts
            // 
            this.labelPorts.Location = new System.Drawing.Point(8, 42);
            this.labelPorts.Name = "labelPorts";
            this.labelPorts.Size = new System.Drawing.Size(37, 13);
            this.labelPorts.TabIndex = 5;
            this.labelPorts.Text = "Ports :";
            // 
            // labelIp
            // 
            this.labelIp.Location = new System.Drawing.Point(8, 28);
            this.labelIp.Name = "labelIp";
            this.labelIp.Size = new System.Drawing.Size(22, 13);
            this.labelIp.TabIndex = 4;
            this.labelIp.Text = "Ip :";
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(8, 16);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 3;
            this.labelName.Text = "Nom :";
            // 
            // buttonApplyPatch
            // 
            this.buttonApplyPatch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonApplyPatch.Image = ( (System.Drawing.Image)( resources.GetObject("buttonApplyPatch.Image") ) );
            this.buttonApplyPatch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonApplyPatch.Location = new System.Drawing.Point(96, 90);
            this.buttonApplyPatch.Name = "buttonApplyPatch";
            this.buttonApplyPatch.Size = new System.Drawing.Size(118, 23);
            this.buttonApplyPatch.TabIndex = 0;
            this.buttonApplyPatch.Text = "Appliquer le patch";
            this.buttonApplyPatch.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonApplyPatch.UseVisualStyleBackColor = true;
            this.buttonApplyPatch.Click += new System.EventHandler(this.ButtonApplyPatchClick);
            // 
            // buttonEdit
            // 
            this.buttonEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonEdit.Image = ( (System.Drawing.Image)( resources.GetObject("buttonEdit.Image") ) );
            this.buttonEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonEdit.Location = new System.Drawing.Point(11, 90);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(72, 23);
            this.buttonEdit.TabIndex = 1;
            this.buttonEdit.Text = "Editer ...";
            this.buttonEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.ButtonEditClick);
            // 
            // buttonDelete
            // 
            this.buttonDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonDelete.Image = ( (System.Drawing.Image)( resources.GetObject("buttonDelete.Image") ) );
            this.buttonDelete.Location = new System.Drawing.Point(205, 196);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(29, 24);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.ButtonDeleteClick);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Image = ( (System.Drawing.Image)( resources.GetObject("buttonAdd.Image") ) );
            this.buttonAdd.Location = new System.Drawing.Point(170, 196);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(29, 24);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.ButtonAddClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Sélectionnez un serveur : ";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(244, 354);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBoxServer);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listBoxServers);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.Text = "Custom Dofus Uplauncher";
            this.groupBoxServer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxServers;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.Label labelPorts;
        private System.Windows.Forms.Label labelIp;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonApplyPatch;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.Label labelPatchUrl;
        private System.Windows.Forms.Label labelAddressValue;
        private System.Windows.Forms.Label labelPatchUrlValue;
        private System.Windows.Forms.Label labelPortsValue;
        private System.Windows.Forms.Label labelIpValue;
        private System.Windows.Forms.Label labelNameValue;
        private System.Windows.Forms.Label label1;
    }
}

