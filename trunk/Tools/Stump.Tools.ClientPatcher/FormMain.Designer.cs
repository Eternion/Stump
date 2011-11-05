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
            this.buttonPatch = new System.Windows.Forms.Button();
            this.buttonUnpatch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPatch
            // 
            this.buttonPatch.Location = new System.Drawing.Point(70, 18);
            this.buttonPatch.Name = "buttonPatch";
            this.buttonPatch.Size = new System.Drawing.Size(104, 23);
            this.buttonPatch.TabIndex = 0;
            this.buttonPatch.Text = "Patch !";
            this.buttonPatch.UseVisualStyleBackColor = true;
            this.buttonPatch.Click += new System.EventHandler(this.ButtonPatchClick);
            // 
            // buttonUnpatch
            // 
            this.buttonUnpatch.Location = new System.Drawing.Point(70, 47);
            this.buttonUnpatch.Name = "buttonUnpatch";
            this.buttonUnpatch.Size = new System.Drawing.Size(104, 23);
            this.buttonUnpatch.TabIndex = 1;
            this.buttonUnpatch.Text = "Remove Patch";
            this.buttonUnpatch.UseVisualStyleBackColor = true;
            this.buttonUnpatch.Click += new System.EventHandler(this.ButtonUnpatchClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 89);
            this.Controls.Add(this.buttonUnpatch);
            this.Controls.Add(this.buttonPatch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormMain";
            this.Text = "Client Patcher";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPatch;
        private System.Windows.Forms.Button buttonUnpatch;
    }
}

