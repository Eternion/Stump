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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Stump.Core.Xml;
using Uplauncher.Helpers;
using Uplauncher.Patcher;
using Uplauncher.Properties;
using Clipboard = System.Windows.Forms.Clipboard;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Uplauncher
{
    public class UplauncherModelView : INotifyPropertyChanged
    {
        private WebClient m_client = new WebClient();
        private readonly SoundProxy m_soundProxy = new SoundProxy();
        private UpdateMeta m_meta;
        private UpdateEntry m_currentUpdate;
        private Stack<PatchTask> m_currentTasks;
        private Stack<UpdateEntry> m_sequence;
        private bool m_replaceExe;
        private readonly DateTime? m_lastUpdateCheck;
        private static readonly Color DefaultMessageColor = Colors.Black;

        public event PropertyChangedEventHandler PropertyChanged;

        public UplauncherModelView(DateTime? lastUpdateCheck)
        {
            m_lastUpdateCheck = lastUpdateCheck;

            NotifyIcon = new NotifyIcon
                {
                    Visible = true,
                    Icon = Resources.dofus_icon_48,
                    ContextMenu = new ContextMenu(new[]
                        {
                            new MenuItem("Voter", OnTrayClickVote),
                            new MenuItem("Ouvrir", OnTrayClickShow),
                            new MenuItem("Quitter", OnTrayClickExit)
                        })
                };

            NotifyIcon.DoubleClick += OnTrayDoubleClick;

            Task.Factory.StartNew(CheckVoteTiming);
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

        private static bool CanRepairGame(object parameter)
        {
            return true;
        }

        private void OnRepairGame(object parameter)
        {
            if (!CanRepairGame(parameter))
                return;

            if (IsUpdating)
                return;

            var dialogResult = MessageBox.Show("Êtes-vous sur de vouloir réparer le jeu? Tous les fichiers seront supprimés puis téléchargés à nouveau !", "Réparer le jeu", MessageBoxButtons.YesNo);

            if (dialogResult != DialogResult.Yes)
                return;

            foreach (var process in Process.GetProcesses().Where(process => process.ProcessName == "Dofus"))
            {
                process.Kill();
            }

            var appFolder = Environment.CurrentDirectory + @"\app";
            if (Directory.Exists(appFolder))
            {
                Directory.Delete(appFolder, true);
            }

            Directory.CreateDirectory("app");
            File.Delete("version");

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

            SetState("Vérification de la mise à jour ...");

            if (File.Exists(Constants.LocalVersionFile))
            {
                int version;
                CurrentVersion = int.TryParse(File.ReadAllText(Constants.LocalVersionFile), out version) ? version : 0;
            }
            else
                CurrentVersion = 0;

            Debug.WriteLine("Current Version : {0}", CurrentVersion);

            m_client = new WebClient();
            m_client.DownloadProgressChanged += OnDownloadProgressChanged;
            m_client.DownloadStringCompleted += OnMetaFileDownloaded;
            m_client.DownloadStringAsync(new Uri(Constants.UpdateSiteURL + Constants.RemoteMetaFile), Constants.UpdateSiteURL + Constants.RemoteMetaFile);
        }

       
        private void OnMetaFileDownloaded(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                HandleDownloadError(e.Cancelled, e.Error, (string)e.UserState);
                return;
            }

            Debug.WriteLine("Remote Version file : {0}", e.Result);
            m_client.DownloadStringCompleted -= OnMetaFileDownloaded;

            try
            {
                m_meta = XmlUtils.Deserialize<UpdateMeta>(new StringReader(e.Result));
            }
            catch (Exception ex)
            {
                HandleDownloadError(false, ex, (string)e.UserState);
                return;
            }

            if (m_meta.LastVersion > CurrentVersion)
            {
                IsUpToDate = false;
                m_playCommand.RaiseCanExecuteChanged();
                RemoteVersion = m_meta.LastVersion;
                Debug.WriteLine("RemoteVersion = {0} CurrentVersion = {1} -> UPDATE", RemoteVersion, CurrentVersion);
                var sequence = new Stack<UpdateEntry>(GeneratePatchSequenceExecution(m_meta.Updates, CurrentVersion, RemoteVersion));
                
                // check validity
                if (sequence.Count == 0 || sequence.All(x => x.FromVersion != CurrentVersion) || sequence.All(x => x.ToVersion != RemoteVersion))
                {
                    HandleDownloadError(false, new Exception("Cannot generate the update sequence. Retry later"), (string)e.UserState);
                    return;
                }

                m_sequence = sequence;

                ProcessSequence();
            }
            else
            {
                SetState(string.Format("Le jeu est à jour (version {0})", CurrentVersion), Colors.Green);
                IsUpdating = false;
                IsUpToDate = true;
            }
        }

        private static IEnumerable<UpdateEntry> GeneratePatchSequenceExecution(ICollection<UpdateEntry> entries, int forRevision, int toRevision, bool reverse = true)
        {
            if (!entries.Any())
                return Enumerable.Empty<UpdateEntry>();

            var possibleSequences = new List<IEnumerable<UpdateEntry>>();

            foreach (var file in entries.Where(entry => entry.FromVersion == forRevision))
            {
                // direct update
                if (file.FromVersion == forRevision &&
                    file.ToVersion == toRevision)
                    return new[] { file };

                var patchs = new List<UpdateEntry>
                    {
                    file
                };

                patchs.AddRange(GeneratePatchSequenceExecution(entries.Where(entry => entry.FromVersion >= file.ToVersion).ToArray(), file.ToVersion, toRevision, false));

                if (patchs.Count > 1)
                    possibleSequences.Add(patchs);
            }

            if (possibleSequences.Count <= 0)
                return Enumerable.Empty<UpdateEntry>();

            // return the sequence that have lesser patchs
            var sequence = possibleSequences.OrderBy(entry => entry.Count()).First();

            return reverse ? sequence.Reverse() : sequence;
        }

        private void ProcessSequence()
        {
            // executed, update the version
            if (m_currentUpdate != null)
            {
                CurrentVersion = m_currentUpdate.ToVersion;
                File.WriteAllText(Constants.LocalVersionFile, CurrentVersion.ToString());

                // uplauncher.exe must be replace. we start another process that will replace it
                if (m_replaceExe)
                {
                    var file = Path.GetTempFileName() + ".exe";
                    File.WriteAllBytes(file, Resources.UplauncherReplacer);

                    Process.Start(file, string.Format("{0} \"{1}\" \"{2}\"", Process.GetCurrentProcess().Id,
                                                      Path.GetFullPath("./app/" + Constants.ExeReplaceTempPath),
                                                      Path.GetFullPath(Constants.CurrentExePath)));
                    NotifyIcon.Visible = false;
                    Environment.Exit(1);
                }
            }

            if (m_sequence.Count == 0)
            {
                OnUpdateEnded(true);
                return;
            }

            m_currentUpdate = m_sequence.Pop();

            // download patch xml
            m_client.DownloadStringCompleted += OnPatchDownloaded;
            m_client.DownloadStringAsync(new Uri(Constants.UpdateSiteURL + m_currentUpdate.PatchRelativURL), Constants.UpdateSiteURL + m_currentUpdate.PatchRelativURL);
        }

        private void OnPatchDownloaded(object sender, DownloadStringCompletedEventArgs e)
        {
            m_client.DownloadStringCompleted -= OnPatchDownloaded;
            Patch patch;
            try
            {
                patch = XmlUtils.Deserialize<Patch>(new StringReader(e.Result));
            }
            catch (Exception ex)
            {
                HandleDownloadError(false, ex, (string)e.UserState);
                return;
            }

            m_currentTasks = new Stack<PatchTask>(patch.Tasks);
            ProcessTask();
        }

        private void ProcessTask()
        {
            if (m_currentTasks.Count == 0)
            {
                ProcessSequence();
                return;
            }

            var task = m_currentTasks.Pop();

            CheckIfReplaceExe(task);
            task.Applied += x => ProcessTask();
            task.Apply(this);
        }

        private void CheckIfReplaceExe(PatchTask task)
        {
            if (!(task is AddFileTask))
                return;

            var addFile = task as AddFileTask;
            var fullPath = Path.GetFullPath("./" + addFile.LocalURL);

            if (!fullPath.Equals(Path.GetFullPath(Constants.CurrentExePath), StringComparison.InvariantCultureIgnoreCase))
                return;

            addFile.LocalURL = Constants.ExeReplaceTempPath;
            m_replaceExe = true;
        }

        private void OnUpdateEnded(bool success)
        {
            if (success)
            {
                SetState(string.Format("Le jeu est à jour (version {0})", CurrentVersion), Colors.Green);

                if (RemoteVersion > 0)
                {
                    CurrentVersion = RemoteVersion;
                    File.WriteAllText(Constants.LocalVersionFile, CurrentVersion.ToString());
                }
                else
                {
                    File.Delete(Constants.LocalVersionFile);
                }
            }

            IsUpToDate = success;
            IsUpdating = false;

            m_playCommand.RaiseCanExecuteChanged();
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
                SetState(string.Format("Erreur lors de la mise à jour : {0}", ex.Message), Colors.Red);
            }

            OnUpdateEnded(false);
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgress = ((double)e.BytesReceived / e.TotalBytesToReceive) * 100;
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

        public int CurrentVersion
        {
            get;
            private set;
        }

        public int RemoteVersion
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
    }
}