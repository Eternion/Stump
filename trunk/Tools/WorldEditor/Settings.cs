#region License GNU GPL
// Settings.cs
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
using System.IO;
using Stump.Core.Attributes;
using Stump.ORM;

namespace WorldEditor
{
    public class Settings
    {
        [Variable(true)]
        public static string CustomDofusAppPath;

        [Variable(true)]
        public static string WorldGfxFile = @"content\gfx\world\gfx0.d2p";

        [Variable(true)]
        public static string WorldEleFile = @"content\maps\elements.ele";

        [Variable(true)]
        public static string GenericMapDecryptionKey = "649ae451ca33ec53bbcbcc33becf15f4";

        [Variable(Priority = 10)]
        public static DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
        {
            Host = "localhost",
            DbName = "stump_world",
            User = "root",
            Password = "",
            ProviderName = "MySql.Data.MySqlClient",
            //UpdateFileDir = "./sql_update/",
        };

        public static string DofusAppPath
        {
            get
            {
                return CustomDofusAppPath ?? FindDofusPath();
            }
        }

        private static string FindDofusPath()
        {
            string programFiles = Environment.GetEnvironmentVariable("programfiles(x86)");

            if (string.IsNullOrEmpty(programFiles))
                programFiles = Environment.GetEnvironmentVariable("programfiles");

            if (string.IsNullOrEmpty(programFiles))
                return null;

            if (Directory.Exists(Path.Combine(programFiles, "Dofus2", "app")))
                return Path.Combine(programFiles, "Dofus2", "app");
            if (Directory.Exists(Path.Combine(programFiles, "Dofus 2", "app")))
                return Path.Combine(programFiles, "Dofus 2", "app");

            return null;
        }
    }
}