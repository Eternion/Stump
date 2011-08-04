namespace Stump.Tools.DataLoader
{
    partial class FormD2O
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
            this.m_dataView = new System.Windows.Forms.DataGridView();
            ( (System.ComponentModel.ISupportInitialize)( this.m_dataView ) ).BeginInit();
            this.SuspendLayout();
            // 
            // m_dataView
            // 
            this.m_dataView.AllowUserToAddRows = false;
            this.m_dataView.AllowUserToDeleteRows = false;
            this.m_dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dataView.Location = new System.Drawing.Point(0, 0);
            this.m_dataView.Name = "m_dataView";
            this.m_dataView.ReadOnly = true;
            this.m_dataView.Size = new System.Drawing.Size(492, 216);
            this.m_dataView.TabIndex = 0;
            // 
            // FormD2O
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 216);
            this.Controls.Add(this.m_dataView);
            this.Name = "FormD2O";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormD2O";
            ( (System.ComponentModel.ISupportInitialize)( this.m_dataView ) ).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView m_dataView;

    }
}