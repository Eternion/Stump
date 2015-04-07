#region License GNU GPL
// UplauncherModelView.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Uplauncher.Helpers;
using Uplauncher.Patcher;
using Uplauncher.Properties;
using Uplauncher.Utils;
using Clipboard = System.Windows.Forms.Clipboard;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Uplauncher
{
    public class UplauncherModelView : INotifyPropertyChanged
    {
        private WebClient m_client = new WebClient();
        private readonly SoundProxy m_soundProxy = new SoundProxy();
        private Stack<MetaFileEntry> m_currentTasks;
        private readonly DateTime? m_lastUpdateCheck;
        private static readonly Color DefaultMessageColor = Colors.Black;

        private readonly FileSizeFormatProvider m_bytesFormatProvider = new FileSizeFormatProvider();
        private MetaFile m_metaFile;


        public event PropertyChangedEventHandler PropertyChanged;

        private readonly BackgroundWorker m_MD5Worker = new BackgroundWorker();

        public UplauncherModelView(DateTime? lastUpdateCheck)
        {
            m_lastUpdateCheck = lastUpdateCheck;

            NotifyIcon = new NotifyIcon
                {
                    Visible = true,
                    Icon = Resources.dofus_icon_48,
                    ContextMenu = new ContextMenu(new[]
                        { 
                            new MenuItem("Ouvrir", OnTrayClickShow),
                            new MenuItem("Lancer le Jeu", OnTrayClickGame),
                            new MenuItem("Voter", OnTrayClickVote),
                            new MenuItem("Quitter", OnTrayClickExit)
                        })
                };

            NotifyIcon.DoubleClick += OnTrayDoubleClick;

            //Task.Factory.StartNew(CheckVoteTiming);
        }

        public WebClient WebClient
        {
            get { return m_client; }
        }


        #region PlayCommand

        private DelegateCommand m_playCommand;

        public DelegateCommand PlayCommand
        {
            get { return m_playCommand ?? (m_playCommand = new DelegateCommand(OnPlay, CanPlay)); }
        }

        private bool CanPlay(object parameter)
        {
            return !IsUpdating && IsUpToDate;
        }

        private void OnPlay(object parameter)
        {
            if (!CanPlay(parameter))
                return;

            if (m_lastUpdateCheck == null || DateTime.Now - m_lastUpdateCheck > TimeSpan.FromMinutes(5))
                CheckUpdates();

            if (!File.Exists(Constants.DofusExePath))
            {
                MessageBox.Show(string.Format(Resources.Dofus_Not_Found, Constants.DofusExePath), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!m_soundProxy.Started)
                m_soundProxy.StartProxy();

            if (m_soundProxy.Started && (m_regProcess == null || m_regProcess.HasExited))
                StartRegApp();

            var process = new Process
                {
                StartInfo = new ProcessStartInfo(Constants.DofusExePath, m_soundProxy.Started ? "--reg-client-port=" + m_soundProxy.ClientPort : string.Empty),
            };

            if (!process.Start())
            {
                MessageBox.Show(Resources.Cannot_Start_Dofus, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SetState("Jeu lancé");
                HideWindowInTrayIcon();
            }
        }

        private void StartRegApp()
        {
            if (!File.Exists(Constants.DofusExePath))
            {
                NotifyIcon.ShowBalloonTip(4000, Constants.ApplicationName, "\"reg/Reg.exe\" est introuvable. Les sons ne seront pas activés", ToolTipIcon.Warning);
                return;
            }

            m_regProcess = new Process
                {
                StartInfo = new ProcessStartInfo(Constants.DofusRegExePath, "--reg-engine-port=" + m_soundProxy.RegPort),
            };

            if (!m_regProcess.Start())
            {
                NotifyIcon.ShowBalloonTip(4000, Constants.ApplicationName, "Impossible de lancer \"reg/Reg.exe\". Raison inconnue", ToolTipIcon.Warning);
            }
        }

        #endregion

        #region VoteCommand

        private DelegateCommand m_voteCommand;
        private Process m_regProcess;

        public DelegateCommand VoteCommand
        {
            get { return m_voteCommand ?? (m_voteCommand = new DelegateCommand(OnVote, CanVote)); }
        }

        private bool CanVote(object parameter)
        {
            return true;
        }

        private void OnVote(object parameter)
        {
            Process.Start(Constants.VoteURL);
            LastVote = DateTime.Now;
        }

        #endregion

        #region SiteCommand

        private DelegateCommand m_siteCommand;

        public DelegateCommand SiteCommand
        {
            get { return m_siteCommand ?? (m_siteCommand = new DelegateCommand(OnSite, CanSite)); }
        }

        private bool CanSite(object parameter)
        {
            return true;
        }

        private void OnSite(object parameter)
        {
            if (!CanSite(parameter))
                return;


            Process.Start(Constants.SiteURL);
        }

        #endregion

        #region CloseCommand

        private DelegateCommand m_closeCommand;

        public DelegateCommand CloseCommand
        {
            get { return m_closeCommand ?? (m_closeCommand = new DelegateCommand(OnClose, CanClose)); }
        }

        private static bool CanClose(object parameter)
        {
            return true;
        }

        private void OnClose(object parameter)
        {
            if (!CanClose(parameter))
                return;

            HideWindowInTrayIcon();
        }

        #endregion

        #region RepairGameCommand

        private DelegateCommand m_repairGameCommand;

        public DelegateCommand RepairGameCommand
        {
            get { return m_repairGameCommand ?? (m_repairGameCommand = new DelegateCommand(OnRepairGame, CanRepairGame)); }
        }

        private bool CanRepairGame(object parameter)
        {
            return !IsUpdating;
        }

        private void OnRepairGame(object parameter)
        {
            if (!CanRepairGame(parameter))
                return;

            if (IsUpdating)
                return;

            var dialogResult = MessageBox.Show(@"Êtes-vous sur de vouloir réparer le jeu? Tous les fichiers seront vérifiés puis re-téléchargés si besoin !", "Réparer le jeu", MessageBoxButtons.YesNo);

            if (dialogResult != DialogResult.Yes)
                return;

            foreach (var process in Process.GetProcesses().Where(process => process.ProcessName == "Dofus"))
            {
                process.Kill();
            }

            File.Delete(Constants.LocalChecksumFile);

            CheckUpdates();
        }

        #endregion

        #region ChangeLanguageCommand

        private DelegateCommand m_changeLanguageCommand;

        public DelegateCommand ChangeLanguageCommand
        {
            get { return m_changeLanguageCommand ?? (m_changeLanguageCommand = new DelegateCommand(OnChangeLanguage, CanChangeLanguage)); }
        }

        private static bool CanChangeLanguage(object parameter)
        {
            return false;
        }

        private static void OnChangeLanguage(object parameter)
        {
            if (parameter == null || !CanChangeLanguage(parameter))
                return;
        }

        #endregion

        #region TrayIcon

        public void HideWindowInTrayIcon()
        {
            View.Hide();
            if (NotifyIcon != null)
            {
                NotifyIcon.ShowBalloonTip(4000, Constants.ApplicationName, "La fenêtre est rangé dans la barre de notifications", ToolTipIcon.Info);
            }
        }

        private void OnTrayClickVote(object sender, EventArgs eventArgs)
        {
            View.Show();
            OnVote(null);
        }

        private void OnTrayClickGame(object sender, EventArgs eventArgs)
        {
            View.Show();
            if (CanPlay(eventArgs))
                OnPlay(eventArgs);
        }

        private void OnTrayClickShow(object sender, EventArgs eventArgs)
        {
            View.Show();
        }

        private void OnTrayDoubleClick(object sender, EventArgs e)
        {
            View.Show();
        }

        private static void OnTrayClickExit(object sender, EventArgs eventArgs)
        {
            System.Windows.Application.Current.Shutdown();
        }

        #endregion

        #region Vote Timer

        private void CheckVoteTiming()
        {
            var processStart = Process.GetCurrentProcess().StartTime;
            if (DateTime.Now - processStart > TimeSpan.FromMinutes(3))
            {
                if (LastVote == null || DateTime.Now - LastVote >= TimeSpan.FromHours(2))
                    NotifyIcon.ShowBalloonTip(5000, Constants.ApplicationName, "Vous pouvez à nouveau voter pour le serveur", ToolTipIcon.Warning);
            }
            Thread.Sleep(2 * 60 * 1000);
            Task.Factory.StartNew(CheckVoteTiming);
        }

        #endregion

        public void CheckUpdates()
        {
            if (IsUpdating)
                return;

            IsUpdating = true;
            m_playCommand.RaiseCanExecuteChanged();

            SetState("Téléchargement des informations ...");

            m_client = new WebClient();
            m_client.DownloadProgressChanged += OnDownloadProgressChanged;
            m_client.DownloadStringCompleted += OnPatchDownloaded;
            try
            {
                 m_client.DownloadStringAsync(new Uri(Constants.UpdateSiteURL + Constants.RemotePatchFile), Constants.RemotePatchFile);
            }
            catch (SocketException)
            {
                SetState("Le serveur est indisponible");
            }
        }

        private void OnPatchDownloaded(object sender, DownloadStringCompletedEventArgs e)
        {
            ProgressDownloadSpeedInfo = string.Empty;
            m_client.DownloadStringCompleted -= OnPatchDownloaded;
            try
            {
                m_metaFile = XmlUtils.Deserialize<MetaFile>(new StringReader(e.Result));

                m_MD5Worker.WorkerReportsProgress = true;
                m_MD5Worker.DoWork += MD5Worker_DoWork;
                m_MD5Worker.ProgressChanged += MD5Worker_ProgressChanged;
                m_MD5Worker.RunWorkerCompleted += MD5Worker_RunWorkerCompleted;

                // if a checksum of the client already exist with compare it to the remote one
                if (!File.Exists(Constants.LocalChecksumFile))
                {
                    m_MD5Worker.RunWorkerAsync();
                }
                else
                {
                    LocalChecksum = File.ReadAllText(Constants.LocalChecksumFile);
                    if (string.IsNullOrEmpty(LocalChecksum))
                        m_MD5Worker.RunWorkerAsync();
                    else
                        CompareChecksums();
                }
            }
            catch (Exception ex)
            {
                HandleDownloadError(false, ex, (string)e.UserState);
            }
        }

        // create the md5 file from the whole directory
        private void MD5Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            m_MD5Worker.DoWork -= MD5Worker_DoWork;
            var path = Directory.GetCurrentDirectory();

            var filesNames = new HashSet<string>(m_metaFile.Tasks.Select(x => x.LocalURL));
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).
                Where(x => filesNames.Contains(GetRelativePath(Path.GetFullPath(x), Path.GetFullPath("./")))). 
                OrderBy(p => p).ToList();

            var md5 = MD5.Create();
            
            var startDate = DateTime.Now;
            long bytesComputed = 0;
            var filesChecked = 0;
            // process in parallel each file but the last
            foreach(var file in files.Take(files.Count - 1))
            {
                var relativePath = file.Substring(path.Length + 1);
                var pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
                md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);
                
                Interlocked.Add(ref bytesComputed, pathBytes.Length);

                var contentBytes = File.ReadAllBytes(file);
                md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                Interlocked.Add(ref bytesComputed, contentBytes.Length);
                Interlocked.Increment(ref filesChecked);

                var percentProgress = (filesChecked*100)/files.Count;
                m_MD5Worker.ReportProgress(percentProgress, bytesComputed/(DateTime.Now - startDate).TotalSeconds);
            }

            if (files.Count > 0)
            {
                var lastFileBytes = File.ReadAllBytes(files.Last());
                md5.TransformFinalBlock(lastFileBytes, 0, lastFileBytes.Length);
            }


            LocalChecksum = files.Count > 0 ? BitConverter.ToString(md5.Hash).Replace("-", "").ToLower() : string.Empty;
            File.WriteAllText(Constants.LocalChecksumFile, LocalChecksum);
        }

        private void MD5Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SetState(string.Format(m_bytesFormatProvider, "Vérification de l'intégrité des fichiers en cours... ({0} % accompli) ({1:fs}/s)", e.ProgressPercentage, (double)e.UserState), Colors.Green);
        }

        private void MD5Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            m_MD5Worker.RunWorkerCompleted -= MD5Worker_RunWorkerCompleted;
            DownloadProgress = 100;
            SetState("Vérification de l'intégrité des fichiers terminé.", Colors.Green);

            CompareChecksums();
        }

        private void CompareChecksums()
        {
            try
            {
                if (m_metaFile != null && m_metaFile.FolderChecksum != LocalChecksum)
                {
                    IsUpToDate = false;
                    m_playCommand.RaiseCanExecuteChanged();

                    m_currentTasks = new Stack<MetaFileEntry>(m_metaFile.Tasks);
                    GlobalDownloadProgress = true;
                    TotalBytesToDownload = m_metaFile.Tasks.Sum(x => x.FileSize);
                    DownloadProgress = 0;
                    ProgressDownloadSpeedInfo = string.Empty;
                    ProcessTask();
                }
                else
                {
                    File.WriteAllText(Constants.LocalChecksumFile, LocalChecksum);
                    SetState(string.Format("Le jeu est à jour"), Colors.Green);
                    IsUpdating = false;
                    IsUpToDate = true;

                    View.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        m_playCommand.RaiseCanExecuteChanged();
                        m_repairGameCommand.RaiseCanExecuteChanged();
                    }));
                }
            }
            catch (Exception ex)
            {
                HandleDownloadError(false, ex, Constants.UpdateSiteURL + Constants.RemotePatchFile);
            }
        }


        private void ProcessTask()
        {
            ThreadPool.QueueUserWorkItem(_ =>
                {
                    if (m_currentTasks.Count == 0)
                    {
                        OnUpdateEnded(true);
                        return;
                    }

                    var task = m_currentTasks.Pop();

                    task.Downloaded += OnTaskApplied;
                    task.Download(this);
                });
        }

        private void OnTaskApplied(MetaFileEntry x)
        {
            TotalDownloadedBytes += x.FileSize;
            DownloadProgress = ((double)TotalDownloadedBytes / TotalBytesToDownload) * 100;

            ProcessTask();
        }

        private void OnUpdateEnded(bool success)
        {
            if (success)
            {
                SetState(string.Format("Le jeu est à jour"), Colors.Green);
                LocalChecksum = m_metaFile.FolderChecksum;
                File.WriteAllText(Constants.LocalChecksumFile, LocalChecksum);
            }

            IsUpToDate = success;
            IsUpdating = false;
            GlobalDownloadProgress = false;
            ProgressDownloadSpeedInfo = string.Empty;

            View.Dispatcher.BeginInvoke(new Action(() =>
            {
                m_playCommand.RaiseCanExecuteChanged();
                m_repairGameCommand.RaiseCanExecuteChanged();
            }));
        }

        private void HandleDownloadError(bool cancelled, Exception ex, string url)
        {
            if (cancelled)
                SetState("Mise à jour interrompue", Colors.Red);
            else
            {
                var remoteURL = url;

                MessageBox.Show(string.Format(Resources.Download_File_Error, remoteURL, ex), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Clipboard.SetText(ex.ToString());
                SetState(string.Format("Erreur lors de la mise à jour : {0}", ex.InnerException.Message), Colors.Red);
            }

            OnUpdateEnded(false);
        }


        private DateTime? m_lastProgressChange;
        private long m_lastGlobalDownloadedBytes;
        private long m_lastFileDownloadedBytes;
        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (!GlobalDownloadProgress)
            {
                DownloadProgress = ((double) e.BytesReceived/e.TotalBytesToReceive)*100;

                if (m_lastProgressChange != null &&
                    (DateTime.Now - m_lastProgressChange.Value) > TimeSpan.FromSeconds(1))
                {
                    ProgressDownloadSpeedInfo = string.Format(m_bytesFormatProvider, "{0:fs} / {1:fs} ({2:fs}/s)",
                        (e.BytesReceived), e.TotalBytesToReceive,
                        (e.BytesReceived - m_lastFileDownloadedBytes)/
                        (DateTime.Now - m_lastProgressChange.Value).TotalSeconds);

                    
                    m_lastProgressChange = DateTime.Now;
                    m_lastFileDownloadedBytes = e.BytesReceived;
                }
            }
            else
            {
                if (m_lastProgressChange != null && (DateTime.Now - m_lastProgressChange.Value) > TimeSpan.FromSeconds(1))
                {
                    ProgressDownloadSpeedInfo = string.Format(m_bytesFormatProvider, "{0:fs} / {1:fs} ({2:fs}/s)",
                        (TotalDownloadedBytes + e.BytesReceived), TotalBytesToDownload,
                        ((TotalDownloadedBytes + e.BytesReceived) - m_lastGlobalDownloadedBytes)/
                        (DateTime.Now - m_lastProgressChange.Value).TotalSeconds);

                    
                    m_lastProgressChange = DateTime.Now;
                    m_lastGlobalDownloadedBytes = TotalDownloadedBytes + e.BytesReceived;
                }
            }

            if (m_lastProgressChange == null)
                m_lastProgressChange = DateTime.Now;
        }

        public void SetState(string message)
        {
            StateMessageColor = DefaultMessageColor;
            StateMessage = message;
        }

        public void SetState(string message, Color color)
        {
            StateMessageColor = color;
            StateMessage = message;
        }

        public MainWindow View
        {
            get;
            set;
        }

        public NotifyIcon NotifyIcon
        {
            get;
            private set;
        }

        public bool IsUpdating
        {
            get;
            set;
        }

        public bool IsUpToDate
        {
            get;
            set;
        }

        public string LocalChecksum
        {
            get;
            private set;
        }

        public DateTime? LastVote
        {
            get;
            private set;
        }

        public double DownloadProgress
        {
            get;
            set;
        }

        public Color StateMessageColor
        {
            get;
            set;
        }

        public string StateMessage
        {
            get;
            set;
        }

        public long TotalBytesToDownload
        {
            get;
            set;
        }

        public long TotalDownloadedBytes
        {
            get;
            set;
        }

        public bool GlobalDownloadProgress
        {
            get;
            set;
        }

        public string ProgressDownloadSpeedInfo
        {
            get;
            set;
        }

        static string GetRelativePath(string fullPath, string relativeTo)
        {
            var foldersSplitted = fullPath.Split(new[] { relativeTo.Replace("/", "\\").Replace("\\\\", "\\") }, StringSplitOptions.RemoveEmptyEntries); // cut the source path and the "rest" of the path

            return foldersSplitted.Length > 0 ? foldersSplitted.Last() : ""; // return the "rest"
        }
    }
}