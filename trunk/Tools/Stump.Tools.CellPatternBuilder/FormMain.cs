using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stump.Tools.MapControl;
using Stump.Core.Xml;
using Stump.Plugins.DefaultPlugin.Global.Placements;

namespace Stump.Tools.CellPatternBuilder
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            mapControl.CellClicked += MapControlCellClicked;
            mapControl.CellOver += (e, current, last) => { if (current != null) current.Text = current.Id.ToString(); if (last != null) last.Text = string.Empty; };

            foreach (var cell in mapControl.Cells)
            {
                cell.Text = cell.Id.ToString();
            }

            mapControl.Invalidate();
        }

        private PlacementPattern GetPattern()
        {
            var blueCells = mapControl.Cells.Where(entry => entry.State == CellState.BluePlacement).Select(entry => entry.GetPlanLocation(mapControl)).ToArray();
            var redCells = mapControl.Cells.Where(entry => entry.State == CellState.RedPlacement).Select(entry => entry.GetPlanLocation(mapControl)).ToArray();
            Point[] blues;
            Point[] reds;

            // get center
            var centerX = (blueCells.Sum(entry => entry.X) + redCells.Sum(entry => entry.X)) / (blueCells.Length + redCells.Length);
            var centerY = (blueCells.Sum(entry => entry.Y) + redCells.Sum(entry => entry.Y)) / (blueCells.Length + redCells.Length);

            if (checkBoxRelativeIds.Checked)
            {
                blues = blueCells.Select(entry => new Point(entry.X - centerX, entry.Y - centerY)).ToArray();
                reds = redCells.Select(entry => new Point(entry.X - centerX, entry.Y - centerY)).ToArray();
            }
            else
            {
                blues = blueCells;
                reds = redCells;
            }

            return new PlacementPattern()
            {
                Blues = blues,
                Reds = reds,
                Center = new Point(centerX, centerY),
                Relativ = checkBoxRelativeIds.Checked
            };
        }

        private void SetPattern(PlacementPattern pattern)
        {
            Point[] blues;
            Point[] reds;

            if (pattern.Relativ)
            {
                blues = pattern.Blues.Select(entry => new Point(entry.X + pattern.Center.X, entry.Y + pattern.Center.Y)).ToArray();
                reds = pattern.Reds.Select(entry => new Point(entry.X + pattern.Center.X, entry.Y + pattern.Center.Y)).ToArray();
            }
            else
            {
                blues = pattern.Blues;
                reds = pattern.Reds;
            }

            foreach (var mapCell in mapControl.Cells)
            {
                if (blues.Contains(mapCell.GetPlanLocation(mapControl)))
                    mapCell.State = CellState.BluePlacement;
                else if (reds.Contains(mapCell.GetPlanLocation(mapControl)))
                    mapCell.State = CellState.RedPlacement;

                else mapCell.State = CellState.None;
            }

            checkBoxRelativeIds.Checked = pattern.Relativ;

            mapControl.Invalidate();
        }

        private void UpdateComplexity()
        {
            var calc = new PlacementComplexityCalculator(mapControl.Cells.Where(entry => entry.State != CellState.None).Select(entry => entry.GetPlanLocation(mapControl)).ToArray());

            labelComplexity.Text = calc.Compute().ToString();
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

            UpdateComplexity();
            control.Invalidate(cell);
        }

        private void CheckBoxLowQualityCheckStateChanged(object sender, EventArgs e)
        {
            mapControl.LesserQuality = checkBoxLowQuality.Checked;
        }

        private void ButtonOpenClick(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Pattern|*.xml",
                Multiselect = false,
                Title = "Open a pattern file ..."
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SetPattern(XmlUtils.Deserialize<PlacementPattern>(dialog.FileName));
                    Text = Path.GetFileName(dialog.FileName);

                    UpdateComplexity();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("Cannot open file : {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ButtonSaveClick(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = "xml",
                Title = "Save the pattern ...",
                OverwritePrompt = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var file = dialog.FileName;

                XmlUtils.Serialize(file, GetPattern());
                Text = Path.GetFileName(file);
            }
        }

        private void CheckBoxRelativeIdsCheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
