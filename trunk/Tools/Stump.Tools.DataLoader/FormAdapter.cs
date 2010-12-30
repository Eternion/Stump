using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stump.Tools.DataLoader
{
    public abstract partial class FormAdapter : Form
    {
        protected FormAdapter()
        {
            InitializeComponent();
        }

        public abstract IFileAdapter Adapter
        {
            get;
            set;
        }
    }
}
