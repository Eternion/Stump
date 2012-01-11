using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using CustomUplauncher.Patchs;
using CustomUplauncher.Updater;

namespace CustomUplauncher
{
    public partial class FormMain : Form
    {
        public const string XmlStorageFile = "servers.xml";
        public const string CurrentPatchFile = "patch.xml";

        public const string DofusPath = @"C:\Program Files (x86)\Dofus 2\app\";
        public const string PatchsStorageDirectory = "./patchs/";

        private PatchInformations m_currentPatch;
        private int m_selectedServerIndex;

        private List<ServerInformations> m_servers;

        public FormMain()
        {
            InitializeComponent();
            Servers = new List<ServerInformations>();

            LoadServersFromXml(XmlStorageFile);
            DefineCurrentPatch(CurrentPatchFile);
            Application.ApplicationExit += ApplicationApplicationExit;

            SelectServer(-1);
        }

        public List<ServerInformations> Servers
        {
            get { return m_servers; }
            set
            {
                m_servers = value;
                RefreshServersList();
            }
        }

        public ServerInformations SelectedServer
        {
            get;
            private set;
        }

        public int SelectedServerIndex
        {
            get { return m_selectedServerIndex; }
            set
            {
                m_selectedServerIndex = value;
                SelectServer(value);
            }
        }

        public void LoadServersFromXml(string xmlFile)
        {
            if (!File.Exists(xmlFile))
                return;

            try
            {
                using (XmlReader reader = XmlReader.Create(xmlFile))
                {
                    var serializer = new XmlSerializer(typeof (ServerInformations[]));
                    Servers = (serializer.Deserialize(reader) as ServerInformations[]).ToList();
                }
            }
            catch
            {
                MessageBox.Show("Storage file corrupted and will be reset");
            }
        }

        private void DefineCurrentPatch(string patchXml)
        {
            if (!File.Exists(patchXml))
                return;

            try
            {
                using (XmlReader reader = XmlReader.Create(patchXml))
                {
                    var serializer = new XmlSerializer(typeof (PatchInformations));
                    m_currentPatch = serializer.Deserialize(reader) as PatchInformations;
                }
            }
            catch
            {
                MessageBox.Show("Patch file corrupted and will be reset");
            }
        }

        public void SaveServersToXml(string xmlFile)
        {
            var settings = new XmlWriterSettings
                               {
                                   Indent = true,
                               };

            using (XmlWriter writer = XmlWriter.Create(xmlFile, settings))
            {
                var serializer = new XmlSerializer(typeof (ServerInformations[]));
                serializer.Serialize(writer, Servers.ToArray());
            }
        }

        public void SelectServer(int index)
        {
            listBoxServers.SelectedIndex = index;
            buttonDelete.Enabled = listBoxServers.SelectedItem != null;

            SelectedServer = index < 0 || index >= Servers.Count ? null : Servers[index];
            DisplayServerInformations(SelectedServer);
        }

        public void RefreshServersList()
        {
            listBoxServers.BeginUpdate();
            listBoxServers.Items.Clear();

            foreach (ServerInformations server in m_servers)
            {
                listBoxServers.Items.Add(server);
            }

            listBoxServers.EndUpdate();

            SelectServer(-1);
        }

        public void AddServer(ServerInformations server)
        {
            m_servers.Add(server);
            RefreshServersList();
        }

        public void RemoveServer(ServerInformations server)
        {
            m_servers.Remove(server);
            RefreshServersList();
        }

        public void RemoveServer(int index)
        {
            m_servers.RemoveAt(index);

            RefreshServersList();
        }

        public void EditSelectedServer()
        {
            if (SelectedServer == null)
                return;

            var dialog = new FormServer
                             {ServerInformations = new ServerInformations(SelectedServer)};

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectedServer = dialog.ServerInformations;
                Servers[SelectedServerIndex] = SelectedServer;

                RefreshServersList();
                SelectServer(SelectedServerIndex);
            }
        }

        private void DisplayServerInformations(ServerInformations server)
        {
            if (server == null)
            {
                labelNameValue.Text = string.Empty;
                labelIpValue.Text = string.Empty;
                labelPortsValue.Text = string.Empty;
                labelAddressValue.Text = string.Empty;
                labelPatchUrlValue.Text = string.Empty;

                ToggleServerUsable(false);
            }
            else
            {
                labelNameValue.Text = server.Name;
                labelIpValue.Text = server.Ip;
                labelPortsValue.Text = server.Ports;
                labelAddressValue.Text = server.Address;
                labelPatchUrlValue.Text = server.PatchUrl;

                ToggleServerUsable(true);
            }
        }

        private void ToggleServerUsable(bool enabled)
        {
            groupBoxServer.Visible = enabled;
            Size = PreferredSize;
        }

        private bool RecoverPatch(Guid patchGuid)
        {
            string path = Path.Combine(PatchsStorageDirectory, patchGuid.ToString());

            if (!Directory.Exists(path))
                return false;

            string xmlPatchPath = Path.Combine(path, "patch.xml");
            PatchInformations patch;
            using (XmlReader reader = XmlReader.Create(xmlPatchPath))
            {
                var serializer = new XmlSerializer(typeof (PatchInformations));
                patch = serializer.Deserialize(reader) as PatchInformations;
            }

            if (patch.Guid != patchGuid)
                return false;

            if (patch.Downloads.Select(download => Path.Combine(path, download.Destination)).
                Any(file => !File.Exists(file)))
                return false;

            foreach (PatchUrl download in patch.Downloads)
            {
                string destination = Path.Combine(DofusPath, download.Destination);

                if (File.Exists(destination))
                    File.Delete(destination);

                File.Move(Path.Combine(path, download.Destination),
                          Path.Combine(DofusPath, download.Destination));
            }

            return true;
        }

        private void PlayToCurrentServer()
        {
            try
            {
                ApplyPatch();
                UpdateMetaFiles();
                ModifyConfigFile();
                ClearCache();

                MessageBox.Show("Vous pouvez maintenant lancer le jeu");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex);
            }
        }

        private void ApplyPatch()
        {
            if (SelectedServer == null)
                throw new Exception("Aucun serveur séléctionné");

            if (string.IsNullOrEmpty(SelectedServer.PatchUrl))
                return;

            var patchForm = new FormPatch(SelectedServer.PatchUrl, DofusPath);

            if (patchForm.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var file in patchForm.GetDownloadedFiles())
                {
                    string destination = Path.Combine(DofusPath, file.RelativePath);

                    if (File.Exists(destination))
                        File.Delete(destination);

                    File.Move(file.FullPath, destination);
                }
            }
            else
            {
                if (patchForm.Exception != null)
                    MessageBox.Show("Erreur lors de l'application du patch : " + patchForm.Exception);
                else
                    MessageBox.Show("Erreur inconnue lors de l'application du patch");
            }
        }

        private void UpdateMetaFiles()
        {
            var d2ometa = new MetaFile(Path.Combine(DofusPath, "data", "common", "data.meta"));
            d2ometa.Open();
            d2ometa.Save();

            var d2imeta = new MetaFile(Path.Combine(DofusPath, "data", "i18n", "data.meta"));
            d2imeta.Open();
            d2imeta.Save();
        }

        private void ModifyConfigFile()
        {
            string path = Path.Combine(DofusPath, "config.xml");
            var config = new XmlDocument();

            config.LoadXml(path);

            XPathNavigator navigator = config.CreateNavigator();
            XPathNavigator host = navigator.SelectSingleNode("//entry[@key='connection.host']");
            XPathNavigator port = navigator.SelectSingleNode("//entry[@key='connection.port']");

            host.SetValue(SelectedServer.Ip);
            port.SetValue(SelectedServer.Ports);

            var writer = new XmlTextWriter(path, Encoding.UTF8)
                             {
                                 Indentation = 1,
                                 IndentChar = '\t',
                                 Formatting = Formatting.Indented,
                             };

            config.Save(writer);
        }

        private void ClearCache()
        {
            string appData = Environment.GetEnvironmentVariable("appdata");

            if (appData == string.Empty)
            {
                MessageBox.Show("Impossible de vider le cache. Faites le manuellement", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dofuscache = Path.Combine(appData, "Dofus 2");

            foreach (string file in Directory.EnumerateFiles(dofuscache, "*", SearchOption.TopDirectoryOnly))
            {
                File.Delete(file);
            }
        }

        #region Controls Events

        private void ListBoxServersSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectServer(listBoxServers.SelectedIndex);
        }

        private void ButtonAddClick(object sender, EventArgs e)
        {
            var dialog = new FormServer();

            if (dialog.ShowDialog() == DialogResult.OK)
                AddServer(dialog.ServerInformations);
        }

        private void ButtonDeleteClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, string.Format("Êtes-vous sûr de supprimer {0} ?", SelectedServer.Name), "Suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                RemoveServer(SelectedServerIndex);
        }

        private void ButtonEditClick(object sender, EventArgs e)
        {
            EditSelectedServer();
        }

        private void ApplicationApplicationExit(object sender, EventArgs e)
        {
            SaveServersToXml(XmlStorageFile);
        }

        private void ButtonApplyPatchClick(object sender, EventArgs e)
        {
            PlayToCurrentServer();
        }

        #endregion
    }
}