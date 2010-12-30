namespace Stump.Tools.DataLoader
{
    partial class FormSelect
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
            this.m_checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_checkedListBox
            // 
            this.m_checkedListBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_checkedListBox.FormattingEnabled = true;
            this.m_checkedListBox.Location = new System.Drawing.Point(0, 0);
            this.m_checkedListBox.Name = "m_checkedListBox";
            this.m_checkedListBox.Size = new System.Drawing.Size(144, 154);
            this.m_checkedListBox.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonOk.Location = new System.Drawing.Point(0, 153);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(144, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "&Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // FormSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(144, 176);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.m_checkedListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSelect";
            this.Text = "Select ...";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox m_checkedListBox;
        private System.Windows.Forms.Button buttonOk;
    }
}