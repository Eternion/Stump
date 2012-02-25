using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stump.Tools.MapControl;

namespace Stump.Tools.CellPatternBuilder
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            mapControl.CellClicked += MapControlCellClicked;

            foreach (var cell in mapControl.Cells)
            {
                cell.Text = cell.Id.ToString();
                cell.TextBrush = Brushes.Brown;
            }
        }

        private void MapControlCellClicked(MapControl.MapControl control, MapCell cell, MouseButtons buttons, bool hold)
        {
            if (buttons == MouseButtons.Left)
            {
                cell.State = ( radioButtonBlue.Checked ? CellState.BluePlacement : CellState.RedPlacement );
            }
            else if (buttons == MouseButtons.Right)
            {
                cell.State = CellState.None;
            }

            control.Invalidate(cell);
        }

        private void CheckBoxLowQualityCheckStateChanged(object sender, EventArgs e)
        {
            mapControl.LesserQuality = checkBoxLowQuality.Checked;
        }

        private void ButtonOpenClick(object sender, EventArgs e)
        {

        }

        private void ButtonSaveClick(object sender, EventArgs e)
        {

        }
    }
}
