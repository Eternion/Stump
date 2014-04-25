namespace BotVote
{
    partial class MainView
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
            if (disposing && (components != null))
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
            this.infoConsole = new System.Windows.Forms.TextBox();
            this.errorConsole = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.proxyCount = new System.Windows.Forms.Label();
            this.voteCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // infoConsole
            // 
            this.infoConsole.Location = new System.Drawing.Point(12, 35);
            this.infoConsole.Multiline = true;
            this.infoConsole.Name = "infoConsole";
            this.infoConsole.Size = new System.Drawing.Size(381, 297);
            this.infoConsole.TabIndex = 0;
            // 
            // errorConsole
            // 
            this.errorConsole.Location = new System.Drawing.Point(399, 35);
            this.errorConsole.Multiline = true;
            this.errorConsole.Name = "errorConsole";
            this.errorConsole.Size = new System.Drawing.Size(425, 297);
            this.errorConsole.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(107, 6);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(649, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // proxyCount
            // 
            this.proxyCount.AutoSize = true;
            this.proxyCount.Location = new System.Drawing.Point(794, 16);
            this.proxyCount.Name = "proxyCount";
            this.proxyCount.Size = new System.Drawing.Size(30, 13);
            this.proxyCount.TabIndex = 3;
            this.proxyCount.Text = "0 / 0";
            // 
            // voteCount
            // 
            this.voteCount.AutoSize = true;
            this.voteCount.Location = new System.Drawing.Point(13, 15);
            this.voteCount.Name = "voteCount";
            this.voteCount.Size = new System.Drawing.Size(46, 13);
            this.voteCount.TabIndex = 4;
            this.voteCount.Text = "Votes: 0";
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 344);
            this.Controls.Add(this.voteCount);
            this.Controls.Add(this.proxyCount);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.errorConsole);
            this.Controls.Add(this.infoConsole);
            this.Name = "MainView";
            this.Text = "BotVote";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox infoConsole;
        private System.Windows.Forms.TextBox errorConsole;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label proxyCount;
        private System.Windows.Forms.Label voteCount;
    }
}

