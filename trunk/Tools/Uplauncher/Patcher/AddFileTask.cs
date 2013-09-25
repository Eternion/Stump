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
using Stump.Core.Cryptography;
using Stump.Core.Extensions;

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

        public override bool CanApply()
        {
            return true;
        }

        public override void Apply(UplauncherModelView uplauncher)
        {
            var directory = Path.GetDirectoryName("./" + LocalURL);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (File.Exists("./" + LocalURL))
            {
                uplauncher.SetState(string.Format("Check if {0} already exists ...", RelativeURL));

                var md5 = Cryptography.GetFileMD5HashBase64("./" + LocalURL);
                var remoteMd5 = NetExtensions.RequestMD5(Constants.UpdateSiteURL + RelativeURL);

                Debug.WriteLine("File {0} already exists  MD5:{1} RemoteMD5:{2} ...", LocalURL, md5, remoteMd5);
                if (md5 == remoteMd5)
                {
                    OnApplied();
                    return;
                }
            }

            uplauncher.SetState(string.Format("Download {0} ...", RelativeURL));
            Debug.WriteLine("Download {0} ...", RelativeURL);
            uplauncher.WebClient.DownloadFileCompleted += OnFileDownloaded;
            uplauncher.WebClient.DownloadFileAsync(new Uri(Constants.UpdateSiteURL + RelativeURL), "./" + LocalURL, LocalURL);
        }

        private void OnFileDownloaded(object sender, AsyncCompletedEventArgs e)
        {
            ((WebClient)sender).DownloadFileCompleted -= OnFileDownloaded;
            OnApplied();
        }
    }
}