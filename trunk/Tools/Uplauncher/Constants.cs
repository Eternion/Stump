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
        public const string ApplicationName = "Uplauncher Arkalys";

        public const string SiteURL = "http://arkalys.com/";
        public const string UpdateSiteURL = "http://arkalys.alwaysdata.net/patchs/";
        public static readonly Uri RSSNewsURL = new Uri("http://arkalys.com/misc/rss");
        public const string VoteURL = "http://www.rpg-paradize.com/?page=vote&vote=35907";

        public const string DofusExePath = "Dofus.exe";
        public const string DofusRegExePath = "reg\\Reg.exe";
        public const string RemoteMetaFile = "updates.xml";
        public const string LocalVersionFile = "version";

        public const string ExeReplaceTempPath = "temp_upl.exe";

        public static readonly string CurrentExePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
    }
}