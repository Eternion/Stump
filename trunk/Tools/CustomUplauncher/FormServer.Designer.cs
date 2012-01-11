namespace CustomUplauncher
{
    partial class FormServer
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
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelPatchUrl = new System.Windows.Forms.Label();
            this.labelPorts = new System.Windows.Forms.Label();
            this.labelIp = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.groupBoxSever = new System.Windows.Forms.GroupBox();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.textBoxPatchUrl = new System.Windows.Forms.TextBox();
            this.textBoxPorts = new System.Windows.Forms.TextBox();
            this.textBoxIp = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxSever.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Location = new System.Drawing.Point(6, 132);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(51, 13);
            this.labelAddress.TabIndex = 12;
            this.labelAddress.Text = "Adresse :";
            // 
            // labelPatchUrl
            // 
            this.labelPatchUrl.AutoSize = true;
            this.labelPatchUrl.Location = new System.Drawing.Point(6, 103);
            this.labelPatchUrl.Name = "labelPatchUrl";
            this.labelPatchUrl.Size = new System.Drawing.Size(66, 13);
            this.labelPatchUrl.TabIndex = 11;
            this.labelPatchUrl.Text = "Patch URL :";
            // 
            // labelPorts
            // 
            this.labelPorts.AutoSize = true;
            this.labelPorts.Location = new System.Drawing.Point(6, 74);
            this.labelPorts.Name = "labelPorts";
            this.labelPorts.Size = new System.Drawing.Size(37, 13);
            this.labelPorts.TabIndex = 10;
            this.labelPorts.Text = "Ports :";
            // 
            // labelIp
            // 
            this.labelIp.AutoSize = true;
            this.labelIp.Location = new System.Drawing.Point(6, 44);
            this.labelIp.Name = "labelIp";
            this.labelIp.Size = new System.Drawing.Size(22, 13);
            this.labelIp.TabIndex = 9;
            this.labelIp.Text = "Ip :";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(6, 16);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 8;
            this.labelName.Text = "Nom :";
            // 
            // groupBoxSever
            // 
            this.groupBoxSever.Controls.Add(this.textBoxAddress);
            this.groupBoxSever.Controls.Add(this.textBoxPatchUrl);
            this.groupBoxSever.Controls.Add(this.textBoxPorts);
            this.groupBoxSever.Controls.Add(this.textBoxIp);
            this.groupBoxSever.Controls.Add(this.textBoxName);
            this.groupBoxSever.Controls.Add(this.labelName);
            this.groupBoxSever.Controls.Add(this.labelAddress);
            this.groupBoxSever.Controls.Add(this.labelIp);
            this.groupBoxSever.Controls.Add(this.labelPatchUrl);
            this.groupBoxSever.Controls.Add(this.labelPorts);
            this.groupBoxSever.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSever.Name = "groupBoxSever";
            this.groupBoxSever.Size = new System.Drawing.Size(261, 160);
            this.groupBoxSever.TabIndex = 13;
            this.groupBoxSever.TabStop = false;
            this.groupBoxSever.Text = "Server";
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(111, 129);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(144, 20);
            this.textBoxAddress.TabIndex = 17;
            this.textBoxAddress.TextChanged += new System.EventHandler(this.TextBoxAddressTextChanged);
            // 
            // textBoxPatchUrl
            // 
            this.textBoxPatchUrl.Location = new System.Drawing.Point(111, 100);
            this.textBoxPatchUrl.Name = "textBoxPatchUrl";
            this.textBoxPatchUrl.Size = new System.Drawing.Size(144, 20);
            this.textBoxPatchUrl.TabIndex = 16;
            this.textBoxPatchUrl.TextChanged += new System.EventHandler(this.TextBoxPatchUrlTextChanged);
            // 
            // textBoxPorts
            // 
            this.textBoxPorts.Location = new System.Drawing.Point(111, 71);
            this.textBoxPorts.Name = "textBoxPorts";
            this.textBoxPorts.Size = new System.Drawing.Size(144, 20);
            this.textBoxPorts.TabIndex = 15;
            this.textBoxPorts.TextChanged += new System.EventHandler(this.TextBoxPortsTextChanged);
            // 
            // textBoxIp
            // 
            this.textBoxIp.Location = new System.Drawing.Point(111, 41);
            this.textBoxIp.Name = "textBoxIp";
            this.textBoxIp.Size = new System.Drawing.Size(144, 20);
            this.textBoxIp.TabIndex = 14;
            this.textBoxIp.TextChanged += new System.EventHandler(this.TextBoxIpTextChanged);
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(111, 13);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(144, 20);
            this.textBoxName.TabIndex = 13;
            this.textBoxName.TextChanged += new System.EventHandler(this.TextBoxNameTextChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(60, 178);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 14;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(150, 178);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Annuler";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 211);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxSever);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormServer";
            this.Text = "Modifier le serveur ...";
            this.groupBoxSever.ResumeLayout(false);
            this.groupBoxSever.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.Label labelPatchUrl;
        private System.Windows.Forms.Label labelPorts;
        private System.Windows.Forms.Label labelIp;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.GroupBox groupBoxSever;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.TextBox textBoxPatchUrl;
        private System.Windows.Forms.TextBox textBoxPorts;
        private System.Windows.Forms.TextBox textBoxIp;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}