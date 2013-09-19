using System.Windows;
using System.Windows.Threading;
using Uplauncher.Helpers;

namespace Uplauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (ApplicationRunningHelper.AlreadyRunning())
                Shutdown();

            DispatcherUnhandledException += OnUnhandledException;
        }


        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Clipboard.SetText(e.Exception.ToString());
            MessageBox.Show("Erreur (copié) : " + e.Exception);
        }
    }
}
