using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uplauncher
{
    /// <summary>
    /// Interaction logic for VoteView.xaml
    /// </summary>
    public partial class VoteView : Window
    {
        public VoteView(string url, int siteID)
        {
            ModelView = new VoteModelView(url, siteID);
            ModelView.View = this;
            InitializeComponent();
        }

        public VoteModelView ModelView
        {
            get;
            private set;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            ModelView.Reload();
        }

        
    }
}
