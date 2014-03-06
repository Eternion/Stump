using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WorldEditor.Helpers;

namespace WorldEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); 
            if (!Debugger.IsAttached)
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            var mainWindow = new MainWindow();
            MainWindow = mainWindow;
            mainWindow.Hide();

            var loadingWindow = new LoadingWindow();
            loadingWindow.ShowDialog();

            mainWindow.Show();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageService.ShowError(null, "Unhandled Exception : " + e.ExceptionObject);
            Clipboard.SetText(e.ExceptionObject.ToString());
        }

        
    }
}
