#region License GNU GPL
// VoteModelView.cs
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
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media.Imaging;
using Uplauncher.Properties;
using WebBrowser = System.Windows.Forms.WebBrowser;

namespace Uplauncher
{
    public class VoteModelView : INotifyPropertyChanged
    {
        private int m_reloadTries;
        private WebBrowser m_webBrowser;

        private DelegateCommand m_reloadCommand;
        private WebBrowserDocumentCompletedEventHandler m_onLoaded;
        private DelegateCommand m_voteCommand;

        public VoteModelView(string url, int siteID)
        {
            RawURL = url;
            SiteID = siteID;
            AskingCredentials = true;
        }

        public string RawURL
        {
            get;
            set;
        }

        public string CompleteURL
        {
            get { return string.Format(RawURL, SiteID); }
        }

        public int SiteID
        {
            get;
            set;
        }

        public WindowsFormsHost WebBrowserHost
        {
            get;
            private set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public bool AskingCredentials
        {
            get;
            set;
        }

        public bool NotAskingCredentials
        {
            get { return !AskingCredentials; }
        }

        public bool AreCredentialsValid
        {
            get;
            private set;
        }

        public bool WrongCredentials
        {
            get;
            private set;
        }

        public bool IsLoading
        {
            get;
            set;
        }

        public bool? DialogResult
        {
            get;
            set;
        }

        public DelegateCommand ReloadCommand
        {
            get
            {
                return m_reloadCommand ?? ( m_reloadCommand = new DelegateCommand(Reload, () => !IsLoading) );
            }
        }


        #region LoginCommand

        private DelegateCommand m_loginCommand;

        public DelegateCommand LoginCommand
        {
            get { return m_loginCommand ?? (m_loginCommand = new DelegateCommand(OnLogin, CanLogin)); }
        }

        public VoteView View
        {
            get;
            set;
        }

        private bool CanLogin(object parameter)
        {
            return AskingCredentials && parameter is PasswordBox && 
                !string.IsNullOrEmpty((parameter as PasswordBox).Password) && !string.IsNullOrEmpty(Username);
        }

        private void OnLogin(object parameter)
        {
            if (parameter == null || !CanLogin(parameter))
                return;

            Password = ( parameter as PasswordBox ).Password;

            try
            {
                var client = new WebClient();
                var output = client.DownloadString(string.Format(Constants.APILoginURL, Username, Password, 0));
                var result = bool.Parse(output);

                if (!result)
                {
                    WrongCredentials = true;
                }
                else
                {
                    AskingCredentials = false;
                    AreCredentialsValid = true;
                }
            }
            catch (WebException)
            {
                MessageBox.Show(Resources.Unreached_Server, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = false;
            }
        }

        #endregion


        private void OnNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.AbsoluteUri == Constants.RpgParadizeHomeUrl)
                IsLoading = true;
        }

        private void OnVoteCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsoluteUri != Constants.RpgParadizeHomeUrl)
                return;

            m_webBrowser.DocumentCompleted -= OnVoteCompleted;

            if (m_webBrowser.Document == null)
            {
                MessageBox.Show(Resources.Unknown_Vote_Error, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                IsLoading = false;
                return;
            }
            else
            {
                if (!m_webBrowser.Document.Cookie.Contains("parasubmitvote2"))
                {
                    MessageBox.Show(Resources.Vote_Refused_Error, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = false;
                }
                else
                {
                    var client = new WebClient();
                    var output = client.DownloadString(string.Format(Constants.APILoginURL, Username, Password, 1));

                    if (string.IsNullOrEmpty(output))
                    {
                        MessageBox.Show(Resources.Unreached_Server, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DialogResult = false;
                    }

                    int minutesUntilNextVote;
                    if (int.TryParse(output, out minutesUntilNextVote))
                    {
                        MessageBox.Show(string.Format(Resources.Already_Voted, minutesUntilNextVote), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DialogResult = false;
                    }
                    else
                    {
                        var result = bool.Parse(output); 
                        DialogResult = result;
                    }
                }
            }

            IsLoading = false;
        }

        public void Reload()
        {
            m_webBrowser = new WebBrowser();
            WebBrowserHost = new WindowsFormsHost() { Child=m_webBrowser };
            WebBrowserHelper.ClearCache(Constants.RpgParadizeURL);

            m_webBrowser.ScrollBarsEnabled = false;
            m_webBrowser.ScriptErrorsSuppressed = true;
            m_webBrowser.AllowWebBrowserDrop = false;
            IsLoading = true;
            m_webBrowser.DocumentCompleted += m_onLoaded = (e, s) => OnReloaded();

            m_webBrowser.Navigate(CompleteURL);
        }

        private void OnReloaded()
        {
            try
            {
                m_webBrowser.DocumentCompleted -= m_onLoaded;

                if (m_webBrowser.Document == null)
                    throw new Exception(string.Format("Cannot reach {0}", CompleteURL));

                if (m_webBrowser.Document.Cookie.Contains("parasubmitvote2"))
                    MessageBox.Show("Cache not erased correctly");

                var element = m_webBrowser.Document.GetElementById("recaptcha_table");

                if (element == null)
                    throw new Exception("Element 'recaptcha_table' not found");
                
                element.ScrollIntoView(true);

                m_webBrowser.Navigating += OnNavigating;
                m_webBrowser.DocumentCompleted += OnVoteCompleted;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                m_reloadTries++;

                if (m_reloadTries > 5)
                {
                    MessageBox.Show(string.Format(Resources.Error_OnLoading, ex), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = false;
                }
                else
                {
                    Reload();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}