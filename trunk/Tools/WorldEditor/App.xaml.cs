using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageService.ShowError(null, "Unhandled Exception : " + e.ExceptionObject);
            Clipboard.SetText(e.ExceptionObject.ToString());
        }
    }
}
