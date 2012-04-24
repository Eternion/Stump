using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Stump.Tools.Toolkit.Controllers;

namespace Stump.Tools.Toolkit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if (DEBUG != true)
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif

            ApplicationController.Instance.Initialize();
            ApplicationController.Instance.Run();
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, false);
            e.Handled = true;
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception, e.IsTerminating);
        }

        private static void HandleException(Exception e, bool isTerminating)
        {
            if (e == null)
            {
                return;
            }

            Trace.TraceError(e.ToString());

            if (!isTerminating)
            {
                MessageBox.Show(string.Format("Unhandled Error : {0}", e), ApplicationInfo.ProductName,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
