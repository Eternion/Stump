using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Threading;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Tools.Toolkit.Views
{
    /// <summary>
    /// Interaction logic for LoadScreen.xaml
    /// </summary>
    public partial class LoadScreen : Window, IView
    {
        public LoadScreen()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            InitializationManager.Instance.AddAssembly(Assembly.GetExecutingAssembly());
            var passes = Enum.GetValues(typeof(InitializationPass)) as InitializationPass[];
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => progressBar.Maximum = (double)passes.Length));
            InitializationManager.Instance.ProcessInitialization +=
                OnProcessInitialization;

            for (int i = 0; i < passes.Length; i++)
            {
                InitializationManager.Instance.Initialize(passes[i]);
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => progressBar.Value = i + 1));
            }
        }

        private void OnProcessInitialization(string str)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => loadingText.Text = str));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var task = new Task(Initialize);
            task.ContinueWith(continueTask =>
                                  {
                                      if (continueTask.IsFaulted)
                                          MessageBox.Show(continueTask.Exception.InnerException.ToString());
                                  });

            //Task.Factory.StartNew(() => Initialize());
        }
    }
}
