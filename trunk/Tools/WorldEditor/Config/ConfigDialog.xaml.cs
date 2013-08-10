using System.Windows;
using Stump.ORM;

namespace WorldEditor.Config
{
    /// <summary>
    /// Interaction logic for ConfigDialog.xaml
    /// </summary>
    public partial class ConfigDialog : Window
    {
        public ConfigDialog()
        {
            InitializeComponent();
            DataContext = ModelView = new ConfigDialogModelView();
            ModelView.DBConfig = new DatabaseConfiguration(
                Settings.DatabaseConfiguration.Host,
                Settings.DatabaseConfiguration.User,
                Settings.DatabaseConfiguration.Password,
                Settings.DatabaseConfiguration.DbName,
                Settings.DatabaseConfiguration.ProviderName);
            ModelView.LoaderSettings = Settings.LoaderSettings.Clone();
        }

        public ConfigDialogModelView ModelView
        {
            get;
            set;
        }

        private void OnOKClicked(object sender, RoutedEventArgs e)
        {
            if (ModelView.IsFirstLaunch)
                ModelView.IsFirstLaunch = false;

            Settings.DatabaseConfiguration = ModelView.DBConfig;
            Settings.LoaderSettings = ModelView.LoaderSettings;

            Settings.SaveSettings();

            DialogResult = true;
            Close();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
