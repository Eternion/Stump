using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace Uplauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            ModelView = new UplauncherModelView(DateTime.Now) { View = this };

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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            DragMove();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = (Hyperlink)sender;
            Process.Start(hyperlink.NavigateUri.ToString());
        }
    }
}
