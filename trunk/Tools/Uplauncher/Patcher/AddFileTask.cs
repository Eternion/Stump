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
using System.Windows.Forms;
using System.Xml.Serialization;
using Stump.Core.Cryptography;
using Uplauncher.Properties;

namespace Uplauncher.Patcher
{
    [XmlType("Add")]
    public class AddFileTask : PatchTask
    {
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

        public override bool CanApply()
        {
            return true;
        }

        public override void Apply(UplauncherModelView uplauncher)
        {
            var fullPath = Path.GetFullPath("./" + LocalURL);
            if (!fullPath.Equals(Path.GetFullPath(Constants.CurrentExePath), StringComparison.InvariantCultureIgnoreCase))
            {
                var directory = Path.GetDirectoryName("./app/" + LocalURL);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                if (File.Exists("./app/" + LocalURL))
                {
                    uplauncher.SetState(string.Format("Check if {0} already exists ...", RelativeURL));

                    var md5 = Cryptography.GetFileMD5HashBase64("./app/" + LocalURL);
                    //var remoteMd5 = NetExtensions.RequestMD5(Constants.UpdateSiteURL + RelativeURL);

                    if (md5 != FileMD5)
                        return;

                    uplauncher.SetState(string.Format("File {0} already exists... Next !", RelativeURL));

                    OnApplied();
                    return;
                }

                uplauncher.SetState(string.Format("Download {0} ...", RelativeURL));
                uplauncher.WebClient.DownloadFileCompleted += OnFileDownloaded;
                uplauncher.WebClient.DownloadFileAsync(new Uri(Constants.UpdateSiteURL + RelativeURL), "./app/" + LocalURL, LocalURL);
            }
            else
            {
                if (!File.Exists(fullPath))
                    return;

                uplauncher.SetState(string.Format("Check if {0} already exists ...", RelativeURL));
                var md5 = Cryptography.GetFileMD5HashBase64(LocalURL);

                if (md5 == FileMD5)
                {
                    uplauncher.SetState(string.Format("File {0} already exists... Next !", RelativeURL));

                    OnApplied();
                    return;
                }

                uplauncher.SetState(string.Format("Download {0} ...", RelativeURL));
                uplauncher.WebClient.DownloadFileCompleted += OnUplauncherDownloaded;
                uplauncher.WebClient.DownloadFileAsync(new Uri(Constants.UpdateSiteURL + RelativeURL), "./" + Constants.ExeReplaceTempPath, Constants.ExeReplaceTempPath);
            }
        }

        private void OnFileDownloaded(object sender, AsyncCompletedEventArgs e)
        {
            ((WebClient)sender).DownloadFileCompleted -= OnFileDownloaded;
            OnApplied();
        }

        private void OnUplauncherDownloaded(object sender, AsyncCompletedEventArgs e)
        {
            var file = Path.GetTempFileName() + ".exe";
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