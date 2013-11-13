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
using Stump.Core.I18N;
using Stump.Core.Xml.Config;
using Stump.ORM;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Config
{
    public class Settings
    {
        public const string ConfigPath = "config.xml";
        private static XmlConfig m_config;

        [Variable(Priority = 10)] public static DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
            {
                Host = "localhost",
                DbName = "stump_data",
                User = "root",
                Password = "",
                ProviderName = "MySql.Data.MySqlClient",
                //UpdateFileDir = "./sql_update/",
            };

        private static bool m_isFirstLaunch = true;
        [Variable]
        public static bool IsFirstLaunch
        {
            get { return m_isFirstLaunch; }
            set { m_isFirstLaunch = value; }
        }

        [Variable] public static LoaderSettings LoaderSettings = new LoaderSettings
            {
                BasePath = FindDofusPath(),
                D2IRelativeDirectory = @"data\i18n",
                D2ORelativeDirectory = @"data\common",
                ItemIconsRelativeFile= @"content\gfx\items\bitmap0.d2p",
                MapsRelativeFile = @"content\maps\maps0.d2p",
                WorldGfxRelativeFile = @"content\gfx\world\gfx0.d2p",
                WorldEleRelativeFile = @"content\maps\elements.ele",
                GenericMapDecryptionKey = "649ae451ca33ec53bbcbcc33becf15f4",
            };

        [Variable] public static Languages DefaultLanguage = Languages.French;

        [Variable]
        public static uint MinI18NId = 200000;

        [Variable]
        public static int MinDataId = 20000;


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

        public static void LoadSettings()
        {
            m_config = new XmlConfig(ConfigPath);
            m_config.AddAssembly(typeof (Settings).Assembly);

            if (!File.Exists(ConfigPath))
                m_config.Create();
            else
                m_config.Load();
        }

        public static void SaveSettings()
        {
            if (m_config == null)
                throw new Exception("Cannot save settings, config file not loaded");

            m_config.Save();
        }
    }
}