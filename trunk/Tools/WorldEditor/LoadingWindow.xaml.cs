using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Stump.Core.I18N;
using WorldEditor.Config;
using WorldEditor.Database;
using WorldEditor.Loaders.Data;
using WorldEditor.Loaders.I18N;
using WorldEditor.Loaders.Icons;

namespace WorldEditor
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        private ManualResetEvent m_reset;

        public LoadingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(Initialize);
        }

        public static readonly DependencyProperty StatusTextProperty =
            DependencyProperty.Register("StatusText", typeof (string), typeof (LoadingWindow), new PropertyMetadata(default(string)));

        public string StatusText
        {
            get { return (string) GetValue(StatusTextProperty); }
            set { SetValue(StatusTextProperty, value); }
        }

        private void Initialize()
        {
            SetStatus(string.Format("Load {0}...", Settings.ConfigPath));
            Settings.LoadSettings();


            if (Settings.IsFirstLaunch)
                ShowConfigDialog();

            OpenDB();
            
            InitializeLoader(() => I18NDataManager.Instance.Initialize(),
                             "Loading d2i files ...");
            InitializeLoader(() => IconsManager.Instance.Initialize(Settings.LoaderSettings.ItemIconsFile),
                             "Loading item icons ...");
            // todo
            I18NDataManager.Instance.DefaultLanguage = Languages.French;

            InitializeLoader(() => ObjectDataManager.Instance.Initialize(),
                             "Loading tables informations ...");

            /*
            InitializeLoader(() => ObjectDataManager.Instance.AddReaders(Settings.LoaderSettings.D2ODirectory),
                             "Loading d2o files ...");
            InitializeLoader(() =>
                {
                    ItemManager.Instance.ChangeDataSource(DatabaseManager.Instance.Database);
                    ItemManager.Instance.Initialize();
                },
                             "Loading database items ...");
            */
            EndInitialization();
        }

        private void EndInitialization()
        {
            Dispatcher.Invoke(Close);
        }
    

        private void OpenDB()
        {
            SetStatus(string.Format("Open Database ..."));
            try
            {
                DatabaseManager.Instance.Connect();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(string.Format("Unable to reach database on {0} : {1}\r\nDo you want to modify the config ?",
                                                  Settings.DatabaseConfiguration.Host, ex.Message),
                                    "Error", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                {
                    ShowConfigDialog();
                    OpenDB();
                }
                else
                {
                    Dispatcher.Invoke(Application.Current.Shutdown);
                }
            }
        }

        private void InitializeLoader(Action initializer, string message)
        {
            SetStatus(message);
            try
            {
                initializer();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(string.Format("Cannot perform initialization : {0}\r\nDo you want to modify the config ?", ex.Message), "Error", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                {
                    ShowConfigDialog();
                    InitializeLoader(initializer, message);
                }
                else
                {
                    Dispatcher.Invoke(Application.Current.Shutdown);
                }
            }
        }

        private void ShowConfigDialog()
        {
            m_reset = new ManualResetEvent(false);
            Dispatcher.BeginInvoke((Action)(() =>
                {
                    Hide();

                    var dialog = new ConfigDialog();
                    if (!Equals(dialog.ShowDialog(), true))
                        Application.Current.Shutdown();
                    else
                    {
                        Show();
                        m_reset.Set();
                    }
                }));
            m_reset.WaitOne();
        }

        private void SetStatus(string text)
        {
            Dispatcher.BeginInvoke((Action)(() =>
                {
                    StatusText = text;
                }));
        }
    }
}
