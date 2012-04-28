using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Waf.Applications;
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
        private CompositionContainer m_container;
        private ApplicationController m_controller;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if (DEBUG != true)
            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
#endif
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            m_container = new CompositionContainer(catalog);
            CompositionBatch batch = new CompositionBatch();
            batch.AddExportedValue(m_container);
            m_container.Compose(batch);

            m_controller = m_container.GetExportedValue<ApplicationController>();
            m_controller.Initialize();
            m_controller.Run();
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
