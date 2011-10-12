namespace Stump.Tools.SpellsExplorer
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
            this.richTextBoxConsoleLogs = new System.Windows.Forms.RichTextBox();
            this.groupBoxConsoleLogs = new System.Windows.Forms.GroupBox();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.listBoxSpells = new System.Windows.Forms.ListBox();
            this.propertyGridSpell = new System.Windows.Forms.PropertyGrid();
            this.groupBoxSelection = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelSelection = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxSpellInfo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelGlobal = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxConsoleLogs.SuspendLayout();
            this.groupBoxSelection.SuspendLayout();
            this.tableLayoutPanelSelection.SuspendLayout();
            this.groupBoxSpellInfo.SuspendLayout();
            this.tableLayoutPanelGlobal.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxConsoleLogs
            // 
            this.richTextBoxConsoleLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxConsoleLogs.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxConsoleLogs.Name = "richTextBoxConsoleLogs";
            this.richTextBoxConsoleLogs.ReadOnly = true;
            this.richTextBoxConsoleLogs.Size = new System.Drawing.Size(582, 119);
            this.richTextBoxConsoleLogs.TabIndex = 0;
            this.richTextBoxConsoleLogs.Text = "";
            // 
            // groupBoxConsoleLogs
            // 
            this.tableLayoutPanelGlobal.SetColumnSpan(this.groupBoxConsoleLogs, 2);
            this.groupBoxConsoleLogs.Controls.Add(this.richTextBoxConsoleLogs);
            this.groupBoxConsoleLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxConsoleLogs.Location = new System.Drawing.Point(3, 338);
            this.groupBoxConsoleLogs.Name = "groupBoxConsoleLogs";
            this.groupBoxConsoleLogs.Size = new System.Drawing.Size(588, 138);
            this.groupBoxConsoleLogs.TabIndex = 1;
            this.groupBoxConsoleLogs.TabStop = false;
            this.groupBoxConsoleLogs.Text = "Console Logs";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSearch.Location = new System.Drawing.Point(3, 3);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(151, 20);
            this.textBoxSearch.TabIndex = 2;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSearch.Location = new System.Drawing.Point(160, 3);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(62, 23);
            this.buttonSearch.TabIndex = 3;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.OnSearch);
            // 
            // listBoxSpells
            // 
            this.tableLayoutPanelSelection.SetColumnSpan(this.listBoxSpells, 2);
            this.listBoxSpells.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSpells.FormattingEnabled = true;
            this.listBoxSpells.Location = new System.Drawing.Point(3, 32);
            this.listBoxSpells.Name = "listBoxSpells";
            this.listBoxSpells.Size = new System.Drawing.Size(219, 276);
            this.listBoxSpells.TabIndex = 4;
            this.listBoxSpells.SelectedIndexChanged += new System.EventHandler(this.OnSpellSelected);
            // 
            // propertyGridSpell
            // 
            this.propertyGridSpell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridSpell.Location = new System.Drawing.Point(3, 16);
            this.propertyGridSpell.Name = "propertyGridSpell";
            this.propertyGridSpell.Size = new System.Drawing.Size(345, 310);
            this.propertyGridSpell.TabIndex = 1;
            // 
            // groupBoxSelection
            // 
            this.groupBoxSelection.Controls.Add(this.tableLayoutPanelSelection);
            this.groupBoxSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSelection.Location = new System.Drawing.Point(3, 3);
            this.groupBoxSelection.Name = "groupBoxSelection";
            this.groupBoxSelection.Size = new System.Drawing.Size(231, 329);
            this.groupBoxSelection.TabIndex = 5;
            this.groupBoxSelection.TabStop = false;
            this.groupBoxSelection.Text = "Selection";
            // 
            // tableLayoutPanelSelection
            // 
            this.tableLayoutPanelSelection.ColumnCount = 2;
            this.tableLayoutPanelSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanelSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelSelection.Controls.Add(this.textBoxSearch, 0, 0);
            this.tableLayoutPanelSelection.Controls.Add(this.listBoxSpells, 0, 1);
            this.tableLayoutPanelSelection.Controls.Add(this.buttonSearch, 1, 0);
            this.tableLayoutPanelSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSelection.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelSelection.Name = "tableLayoutPanelSelection";
            this.tableLayoutPanelSelection.RowCount = 2;
            this.tableLayoutPanelSelection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSelection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSelection.Size = new System.Drawing.Size(225, 310);
            this.tableLayoutPanelSelection.TabIndex = 5;
            // 
            // groupBoxSpellInfo
            // 
            this.groupBoxSpellInfo.Controls.Add(this.propertyGridSpell);
            this.groupBoxSpellInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSpellInfo.Location = new System.Drawing.Point(240, 3);
            this.groupBoxSpellInfo.Name = "groupBoxSpellInfo";
            this.groupBoxSpellInfo.Size = new System.Drawing.Size(351, 329);
            this.groupBoxSpellInfo.TabIndex = 6;
            this.groupBoxSpellInfo.TabStop = false;
            this.groupBoxSpellInfo.Text = "Spell Informations";
            // 
            // tableLayoutPanelGlobal
            // 
            this.tableLayoutPanelGlobal.ColumnCount = 2;
            this.tableLayoutPanelGlobal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelGlobal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelGlobal.Controls.Add(this.groupBoxSelection, 0, 0);
            this.tableLayoutPanelGlobal.Controls.Add(this.groupBoxConsoleLogs, 0, 1);
            this.tableLayoutPanelGlobal.Controls.Add(this.groupBoxSpellInfo, 1, 0);
            this.tableLayoutPanelGlobal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGlobal.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelGlobal.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelGlobal.Name = "tableLayoutPanelGlobal";
            this.tableLayoutPanelGlobal.RowCount = 2;
            this.tableLayoutPanelGlobal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanelGlobal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelGlobal.Size = new System.Drawing.Size(594, 479);
            this.tableLayoutPanelGlobal.TabIndex = 7;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 479);
            this.Controls.Add(this.tableLayoutPanelGlobal);
            this.Name = "FormMain";
            this.Text = "Spells Explorer";
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnShown);
            this.groupBoxConsoleLogs.ResumeLayout(false);
            this.groupBoxSelection.ResumeLayout(false);
            this.tableLayoutPanelSelection.ResumeLayout(false);
            this.tableLayoutPanelSelection.PerformLayout();
            this.groupBoxSpellInfo.ResumeLayout(false);
            this.tableLayoutPanelGlobal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxConsoleLogs;
        private System.Windows.Forms.GroupBox groupBoxConsoleLogs;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGlobal;
        private System.Windows.Forms.GroupBox groupBoxSelection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSelection;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.ListBox listBoxSpells;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.GroupBox groupBoxSpellInfo;
        private System.Windows.Forms.PropertyGrid propertyGridSpell;
    }
}

