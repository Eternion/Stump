namespace Stump.Tools.ClientPatcher
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
            this.buttonPatch = new System.Windows.Forms.Button();
            this.buttonUnpatch = new System.Windows.Forms.Button();
            this.textBoxDofusPath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonProxyPatch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPatch
            // 
            resources.ApplyResources(this.buttonPatch, "buttonPatch");
            this.buttonPatch.Name = "buttonPatch";
            this.buttonPatch.UseVisualStyleBackColor = true;
            this.buttonPatch.Click += new System.EventHandler(this.ButtonPatchClick);
            // 
            // buttonUnpatch
            // 
            resources.ApplyResources(this.buttonUnpatch, "buttonUnpatch");
            this.buttonUnpatch.Name = "buttonUnpatch";
            this.buttonUnpatch.UseVisualStyleBackColor = true;
            this.buttonUnpatch.Click += new System.EventHandler(this.ButtonUnpatchClick);
            // 
            // textBoxDofusPath
            // 
            resources.ApplyResources(this.textBoxDofusPath, "textBoxDofusPath");
            this.textBoxDofusPath.Name = "textBoxDofusPath";
            // 
            // buttonBrowse
            // 
            resources.ApplyResources(this.buttonBrowse, "buttonBrowse");
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.ButtonBrowseClick);
            // 
            // buttonProxyPatch
            // 
            resources.ApplyResources(this.buttonProxyPatch, "buttonProxyPatch");
            this.buttonProxyPatch.Name = "buttonProxyPatch";
            this.buttonProxyPatch.UseVisualStyleBackColor = true;
            this.buttonProxyPatch.Click += new System.EventHandler(this.ButtonProxyPatchClick);
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonProxyPatch);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxDofusPath);
            this.Controls.Add(this.buttonUnpatch);
            this.Controls.Add(this.buttonPatch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormMain";
            this.Load += new System.EventHandler(this.OnFormMainLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonPatch;
        private System.Windows.Forms.Button buttonUnpatch;
        private System.Windows.Forms.TextBox textBoxDofusPath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonProxyPatch;
    }
}

