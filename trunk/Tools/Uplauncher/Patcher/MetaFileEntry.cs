#region License GNU GPL

// AddFileTask.cs
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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using Uplauncher.Properties;
using Uplauncher.Utils;

namespace Uplauncher.Patcher
{
    [XmlType("Entry")]
    public class MetaFileEntry
    {        
        public event Action<MetaFileEntry> Downloaded;

        protected void OnApplied()
        {
            var handler = Downloaded;
            if (handler != null) handler(this);
        }

        [XmlAttribute("url")]
        public string RelativeURL
        {
            get;
            set;
        }

        [XmlAttribute("local")]
        public string LocalURL
        {
            get;
            set;
        }

        [XmlAttribute("MD5")]
        public string FileMD5
        {
            get;
            set;
        }
        
        
        [XmlAttribute("size")]
        public long FileSize
        {
            get;
            set;
        }
        public void Download(UplauncherModelView uplauncher)
        {
            string fullPath = Path.GetFullPath("./" + LocalURL);
            bool isUplauncherExeFile = fullPath.Equals(Path.GetFullPath(Constants.CurrentExePath),
                StringComparison.InvariantCultureIgnoreCase);


            uplauncher.SetState(string.Format("Check if {0} already exists ...", RelativeURL));

            if (File.Exists(fullPath))
            {
                string md5 = Cryptography.GetFileMD5HashBase64(fullPath);

                if (md5 == FileMD5)
                {
                    uplauncher.SetState(string.Format("File {0} already exists... Next !", RelativeURL));

                    OnApplied();
                    return;
                }
            }

            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            uplauncher.SetState(string.Format("Download {0} ...", RelativeURL));
            if (isUplauncherExeFile)
            {
                uplauncher.WebClient.DownloadFileCompleted += OnUplauncherDownloaded;

                uplauncher.WebClient.DownloadFileAsync(new Uri(Constants.UpdateSiteURL + RelativeURL),
                    "./" + Constants.ExeReplaceTempPath, Constants.ExeReplaceTempPath);
            }
            else
            {
                uplauncher.WebClient.DownloadFileCompleted += OnFileDownloaded;
                uplauncher.WebClient.DownloadFileAsync(new Uri(Constants.UpdateSiteURL + RelativeURL), "./" + LocalURL, LocalURL);

            }
        }

        private void OnFileDownloaded(object sender, AsyncCompletedEventArgs e)
        {
            ((WebClient) sender).DownloadFileCompleted -= OnFileDownloaded;
            OnApplied();
        }

        private void OnUplauncherDownloaded(object sender, AsyncCompletedEventArgs e)
        {
            ((WebClient) sender).DownloadFileCompleted -= OnUplauncherDownloaded;

            string file = Path.GetTempFileName() + ".exe";
            File.WriteAllBytes(file, Resources.UplauncherReplacer);

            var procInfo = new ProcessStartInfo
            {
                FileName = file,
                Arguments =
                    string.Format("{0} \"{1}\" \"{2}\"", Process.GetCurrentProcess().Id,
                        Path.GetFullPath(Constants.ExeReplaceTempPath),
                        Path.GetFullPath(Constants.CurrentExePath)),
                Verb = "runas"
            };

            try
            {
                Process.Start(procInfo);

                //NotifyIcon.Visible = false;
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                //The user refused the elevation
            }
        }
    }
}