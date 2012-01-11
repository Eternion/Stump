using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using CustomUplauncher.Patchs;

namespace CustomUplauncher
{
    public class DownloadedFile
    {
        public DownloadedFile()
        {
        }

        public DownloadedFile(string fullPath, string relativePath)
        {
            FullPath = fullPath;
            RelativePath = relativePath;
        }

        public string FullPath
        {
            get;
            set;
        }

        public string RelativePath
        {
            get;
            set;
        }
    }

    public partial class FormPatch : Form
    {
        private readonly WebClient m_client;
        private readonly List<DownloadedFile> m_downloadedFiles = new List<DownloadedFile>();
        private readonly string m_patchUrl;
        private readonly string m_saveRootDirectory;

        private IEnumerator m_downloadsEnumerator;
        private PatchInformations m_patch;

        public FormPatch(string patchUrl, string saveRootDirectory)
        {
            InitializeComponent();

            m_patchUrl = patchUrl;
            m_saveRootDirectory = saveRootDirectory;

            m_client = new WebClient();
            m_client.DownloadFileCompleted += ClientDownloadFileCompleted;
            m_client.DownloadProgressChanged += ClientDownloadProgressChanged;
        }

        public string DestinationDirectory
        {
            get;
            private set;
        }

        public bool DownloadSuccessed
        {
            get;
            private set;
        }

        public Exception Exception
        {
            get;
            private set;
        }

        private void FormPatchShown(object sender, EventArgs e)
        {
            StartDownloadProcedure();
        }

        public void StartDownloadProcedure()
        {
            var task = new Task(
                () =>
                    {
                        if (m_patch == null)
                            RequestRemotePatch();

                        DestinationDirectory = Path.Combine(m_saveRootDirectory, m_patch.Guid.ToString());

                        if (!Directory.Exists(DestinationDirectory))
                            Directory.CreateDirectory(DestinationDirectory);

                        if (RecoverPatch())
                        {
                            CloseDialog(DialogResult.OK);
                            return;
                        }

                        SavePatch(Path.Combine(DestinationDirectory, "patch.xml"));
                        m_downloadsEnumerator = m_patch.Downloads.GetEnumerator();

                        if (!m_downloadsEnumerator.MoveNext())
                        {
                            CloseDialog(DialogResult.OK);
                        }
                        else
                        {
                            var obj = m_downloadsEnumerator.Current as PatchUrl;
                            DownloadFileAsync(obj.Url, Path.Combine(DestinationDirectory, obj.Destination));
                        }
                    });
            task.Start();
        }

        public void CancelDownload()
        {
            m_client.CancelAsync();

            CloseDialog(DialogResult.Cancel);
        }

        public IEnumerable<DownloadedFile> GetDownloadedFiles()
        {
            return m_downloadedFiles;
        }

        private void SavePatch(string path)
        {
            try
            {
                var settings = new XmlWriterSettings
                                   {
                                       Indent = true,
                                   };

                using (XmlWriter writer = XmlWriter.Create(path, settings))
                {
                    var serializer = new XmlSerializer(typeof (PatchInformations));
                    serializer.Serialize(writer, m_patch);
                }
            }
            catch (Exception ex)
            {
                ErrorOccurs("Impossible de sauvegarder patch.xml : " + ex.Message);
            }
        }

        private void RequestRemotePatch()
        {
            try
            {
                SetStatus("Download patch.xml");

                var client = new WebClient();
                var uri = new Uri(m_patchUrl);
                byte[] patchData = client.DownloadData(uri);

                using (XmlReader reader = XmlReader.Create(new MemoryStream(patchData)))
                {
                    var serializer = new XmlSerializer(typeof (PatchInformations));
                    m_patch = serializer.Deserialize(reader) as PatchInformations;
                }

                SetStatus("patch.xml downloaded");
            }
            catch (WebException ex)
            {
                ErrorOccurs("Impossible de télécharger patch.xml");
            }
        }

        private bool RecoverPatch()
        {
            try
            {
                if (!Directory.Exists(DestinationDirectory))
                    return false;

                if (!File.Exists(Path.Combine(DestinationDirectory, "patch.xml")))
                    return false;

                PatchInformations patch;
                using (XmlReader reader = XmlReader.Create(Path.Combine(DestinationDirectory, "patch.xml")))
                {
                    var serializer = new XmlSerializer(typeof (PatchInformations));
                    patch = serializer.Deserialize(reader) as PatchInformations;
                }

                if (patch.Guid != m_patch.Guid)
                    return false;

                foreach (PatchUrl download in patch.Downloads)
                {
                    string path = Path.Combine(DestinationDirectory, download.Destination);

                    if (!File.Exists(path))
                    {
                        m_downloadedFiles.Clear();
                        return false;
                    }

                    m_downloadedFiles.Add(new DownloadedFile(path, download.Destination));
                }
            }
            catch (Exception ex)
            {
                ErrorOccurs("Impossible de restaurer le patch : " + ex.Message);
            }

            return true;
        }

        private void DownloadFileAsync(string url, string destination)
        {
            var uri = new Uri(url);
            try
            {
                m_client.DownloadFileAsync(uri, destination);
                SetStatus("Downloading " + url);
                SetDownloadProgress(0);
            }
            catch (WebException ex)
            {
                ErrorOccurs("Impossible de télécharger " + url);
            }
        }

        private void MoveAllFiles()
        {
            /*buttonCancel.Enabled = false;
            SetStatus("Téléchargement achevé");

            foreach (var file in Directory.EnumerateFiles(m_tempDir))
            {
                if (Path.GetFileName(file) == "patch.xml")
                    continue;

                var relativePath = GetRelativePath(m_tempDir, file);
                SetStatus("Deplace " + relativePath);

                var destination = Path.Combine(DestinationDirectory, relativePath);

                if (File.Exists(destination))
                    File.Delete(destination);

                File.Move(file, destination);
            }

            Directory.Delete(m_tempDir, true);*/
        }


        private void SetStatus(string status)
        {
            labelStatus.Text = status;
        }

        private void SetDownloadProgress(int percent)
        {
            progressBarDownload.Value = percent;
        }

        private void ClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            SetDownloadProgress((int) Math.Truncate((double) e.BytesReceived/e.TotalBytesToReceive*100d));
        }

        private void ClientDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var download = m_downloadsEnumerator.Current as PatchUrl;
            m_downloadedFiles.Add(new DownloadedFile(Path.Combine(DestinationDirectory, download.Destination), download.Destination));

            if (m_downloadsEnumerator.MoveNext())
            {
                var obj = m_downloadsEnumerator.Current as PatchUrl;
                DownloadFileAsync(obj.Url, Path.Combine(DestinationDirectory, obj.Destination));
            }
            else
            {
                CloseDialog(DialogResult.OK);

                /*try
                {
                    MoveAllFiles();
                }
                catch
                {
                    ErrorOccurs("Impossible de déplacer les fichiers téléchargés. Verifiez les droits administrateurs");
                    CancelDownload();
                    return;
                }

                NotifyDownloadEnded(true);*/
            }
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            CancelDownload();
        }

        private void CloseDialog(DialogResult result)
        {
            DialogResult = result;
            Close();
        }

        private void ErrorOccurs(string error)
        {
            Exception = new Exception(error);
            CancelDownload();
        }

        private void ErrorOccurs(Exception error)
        {
            Exception = error;
            CancelDownload();
        }
    }
}