using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Stump.Core.Xml;
using Uplauncher.Patcher;
using Application = System.Windows.Application;

namespace Uplauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool m_isShuttingDown;

        public MainWindow()
        {
            ModelView = new UplauncherModelView();
            ModelView.View = this;

            InitializeComponent();

        }

        public UplauncherModelView ModelView
        {
            get;
            set;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ModelView.CheckUpdates();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if ((bool)typeof(Application).GetProperty("IsShuttingDown", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, new object[0]))
            {
                ModelView.NotifyIcon.Visible = false;
                return;
            }

            e.Cancel = true;
            ModelView.HideWindowInTrayIcon();
        }
    }
}
