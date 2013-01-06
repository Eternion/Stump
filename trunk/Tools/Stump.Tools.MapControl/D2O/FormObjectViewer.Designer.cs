namespace Stump.Tools.DataLoader.D2O
{
    partial class FormObjectViewer
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
            this.stateBrowser = new sliver.Windows.Forms.StateBrowser();
            this.SuspendLayout();
            // 
            // stateBrowser
            // 
            this.stateBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateBrowser.Location = new System.Drawing.Point(0, 0);
            this.stateBrowser.Name = "stateBrowser";
            this.stateBrowser.Object = null;
            this.stateBrowser.ObjectToBrowse = null;
            this.stateBrowser.Size = new System.Drawing.Size(284, 262);
            this.stateBrowser.TabIndex = 0;
            // 
            // FormObjectViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.stateBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormObjectViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormObjectViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private sliver.Windows.Forms.StateBrowser stateBrowser;
    }
}