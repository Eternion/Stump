using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stump.Tools.DataLoader.D2O
{
    public partial class FormObjectViewer : Form
    {
        public FormObjectViewer()
        {
            InitializeComponent();
        }

        public object ViewedObject
        {
            get { return stateBrowser.ObjectToBrowse; }
            set { stateBrowser.ObjectToBrowse = value; }
        }
    }
}
