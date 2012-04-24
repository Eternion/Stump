using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Tools.QuickItemEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DBConnector m_connector = new DBConnector();

        private DatabaseAccessor m_databaseAccessor;
        private ItemEditor m_itemEditor;

        public MainWindow()
        {
            InitializeComponent();
            contentControl.Content = m_connector;

            m_connector.Connected += OnConnectorConnected;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (contentControl.Content == m_connector)
            {
                m_connector.SaveConfigFile();
            }

            base.OnClosing(e);
        }

        private void OnConnectorConnected(DBConnector connector, DatabaseAccessor dbAccessor)
        {
            connector.SaveConfigFile();
            m_databaseAccessor = dbAccessor;
            var language = connector.DisplayLanguage;
            Task.Factory.StartNew(
                () =>
                    {
                        Dispatcher.BeginInvoke((Action) (()
                                                         =>
                                                         contentControl.Content =
                                                         new TextBlock
                                                             {
                                                                 Text = "Loading Texts ...",
                                                                 HorizontalAlignment = HorizontalAlignment.Center,
                                                                 VerticalAlignment = VerticalAlignment.Center,
                                                             }));

                        TextManager.Instance.SetDefaultLanguage(language);
                        TextManager.Instance.Initialize();

                        Dispatcher.BeginInvoke((Action) (()
                                                         =>
                                                             {
                                                                 (contentControl.Content as TextBlock).Text
                                                                     = "Loading Items ...";
                                                             }));

                        ItemManager.Instance.Initialize();


                        Dispatcher.BeginInvoke((Action)( () =>
                        {
                            SizeToContent = SizeToContent.Manual;
                            Width = 700;
                            Height = 550;
                            contentControl.Content = (m_itemEditor = new ItemEditor(m_databaseAccessor));
                        } ));
                    }
                );
        }
    }
}