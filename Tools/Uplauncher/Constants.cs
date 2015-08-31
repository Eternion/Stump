#region License GNU GPL
// Constants.cs
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

namespace Uplauncher
{
    public class Constants
    {
        public const string SiteURL = "http://www.arkalys.com/";
        public const string UpdateSiteURL = "http://patch.arkalys.com/";
        public static readonly Uri RSSNewsURL = new Uri("http://blog.arkalys.com/rss/");
        public const string VoteURL = "http://www.arkalys.com/vote";

        public const string DofusExePath = "app\\Dofus.exe";
        public const string DofusRegExePath = "app\\reg\\Reg.exe";
        public const string LocalChecksumFile = "checksum.arkalys";
        public const string RemotePatchFile = "patch.xml";

        public const string ExeReplaceTempPath = "temp_upl.exe";

        public static string CurrentExePath
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().Location; }
        }

        public static string ApplicationVersion
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        public static string ApplicationName
        {
            get
            {
                return "Uplauncher Arkalys";
            }
        }

    }
}