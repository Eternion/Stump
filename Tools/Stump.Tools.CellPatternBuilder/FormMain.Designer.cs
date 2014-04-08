namespace Stump.Tools.CellPatternBuilder
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
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.radioButtonBlue = new System.Windows.Forms.RadioButton();
            this.radioButtonRed = new System.Windows.Forms.RadioButton();
            this.checkBoxLowQuality = new System.Windows.Forms.CheckBox();
            this.mapControl = new Stump.Tools.MapControl.MapControl();
            this.checkBoxRelativeIds = new System.Windows.Forms.CheckBox();
            this.labelComplexity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(12, 5);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(80, 23);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "Open ...";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.ButtonOpenClick);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(98, 5);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(80, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save ...";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSaveClick);
            // 
            // radioButtonBlue
            // 
            this.radioButtonBlue.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.radioButtonBlue.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonBlue.AutoSize = true;
            this.radioButtonBlue.Checked = true;
            this.radioButtonBlue.ForeColor = System.Drawing.Color.DodgerBlue;
            this.radioButtonBlue.Location = new System.Drawing.Point(330, 5);
            this.radioButtonBlue.Name = "radioButtonBlue";
            this.radioButtonBlue.Size = new System.Drawing.Size(90, 23);
            this.radioButtonBlue.TabIndex = 3;
            this.radioButtonBlue.TabStop = true;
            this.radioButtonBlue.Text = "Blue placement";
            this.radioButtonBlue.UseVisualStyleBackColor = true;
            // 
            // radioButtonRed
            // 
            this.radioButtonRed.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.radioButtonRed.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonRed.AutoSize = true;
            this.radioButtonRed.ForeColor = System.Drawing.Color.Firebrick;
            this.radioButtonRed.Location = new System.Drawing.Point(426, 5);
            this.radioButtonRed.Name = "radioButtonRed";
            this.radioButtonRed.Size = new System.Drawing.Size(89, 23);
            this.radioButtonRed.TabIndex = 4;
            this.radioButtonRed.Text = "Red placement";
            this.radioButtonRed.UseVisualStyleBackColor = true;
            // 
            // checkBoxLowQuality
            // 
            this.checkBoxLowQuality.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.checkBoxLowQuality.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxLowQuality.AutoSize = true;
            this.checkBoxLowQuality.Location = new System.Drawing.Point(252, 5);
            this.checkBoxLowQuality.Name = "checkBoxLowQuality";
            this.checkBoxLowQuality.Size = new System.Drawing.Size(72, 23);
            this.checkBoxLowQuality.TabIndex = 5;
            this.checkBoxLowQuality.Text = "Low Quality";
            this.checkBoxLowQuality.UseVisualStyleBackColor = true;
            this.checkBoxLowQuality.CheckStateChanged += new System.EventHandler(this.CheckBoxLowQualityCheckStateChanged);
            // 
            // mapControl
            // 
            this.mapControl.ActiveCellColor = System.Drawing.Color.Transparent;
            this.mapControl.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.mapControl.BorderColorOnOver = System.Drawing.Color.FromArgb(( (int)( ( (byte)( 255 ) ) ) ), ( (int)( ( (byte)( 128 ) ) ) ), ( (int)( ( (byte)( 0 ) ) ) ));
            this.mapControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapControl.CommonCellHeight = 43D;
            this.mapControl.CommonCellWidth = 86D;
            this.mapControl.DrawMode = ( (Stump.Tools.MapControl.DrawMode)( ( ( ( Stump.Tools.MapControl.DrawMode.Movements | Stump.Tools.MapControl.DrawMode.Fights )
                        | Stump.Tools.MapControl.DrawMode.Triggers )
                        | Stump.Tools.MapControl.DrawMode.Others ) ) );
            this.mapControl.InactiveCellColor = System.Drawing.Color.DarkGray;
            this.mapControl.LesserQuality = false;
            this.mapControl.Location = new System.Drawing.Point(12, 31);
            this.mapControl.MapHeight = 20;
            this.mapControl.MapWidth = 14;
            this.mapControl.Name = "mapControl";
            this.mapControl.Size = new System.Drawing.Size(582, 451);
            this.mapControl.TabIndex = 0;
            this.mapControl.TraceOnOver = true;
            this.mapControl.ViewGrid = true;
            // 
            // checkBoxRelativeIds
            // 
            this.checkBoxRelativeIds.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.checkBoxRelativeIds.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRelativeIds.AutoSize = true;
            this.checkBoxRelativeIds.Checked = true;
            this.checkBoxRelativeIds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRelativeIds.Location = new System.Drawing.Point(521, 5);
            this.checkBoxRelativeIds.Name = "checkBoxRelativeIds";
            this.checkBoxRelativeIds.Size = new System.Drawing.Size(73, 23);
            this.checkBoxRelativeIds.TabIndex = 6;
            this.checkBoxRelativeIds.Text = "Relative Ids";
            this.checkBoxRelativeIds.UseVisualStyleBackColor = true;
            this.checkBoxRelativeIds.CheckedChanged += new System.EventHandler(this.CheckBoxRelativeIdsCheckedChanged);
            // 
            // labelComplexity
            // 
            this.labelComplexity.AutoSize = true;
            this.labelComplexity.BackColor = System.Drawing.Color.Transparent;
            this.labelComplexity.Location = new System.Drawing.Point(550, 459);
            this.labelComplexity.Name = "labelComplexity";
            this.labelComplexity.Size = new System.Drawing.Size(35, 13);
            this.labelComplexity.TabIndex = 7;
            this.labelComplexity.Text = "label1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 494);
            this.Controls.Add(this.labelComplexity);
            this.Controls.Add(this.checkBoxRelativeIds);
            this.Controls.Add(this.checkBoxLowQuality);
            this.Controls.Add(this.radioButtonRed);
            this.Controls.Add(this.radioButtonBlue);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.mapControl);
            this.Name = "FormMain";
            this.Text = "Stump Placement Patterns Builder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapControl.MapControl mapControl;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.RadioButton radioButtonBlue;
        private System.Windows.Forms.RadioButton radioButtonRed;
        private System.Windows.Forms.CheckBox checkBoxLowQuality;
        private System.Windows.Forms.CheckBox checkBoxRelativeIds;
        private System.Windows.Forms.Label labelComplexity;



    }
}