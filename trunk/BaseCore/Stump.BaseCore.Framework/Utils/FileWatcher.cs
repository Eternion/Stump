// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Stump.BaseCore.Framework.Utils
{
    public delegate void WatchAction(string fileName);

    public static class FileWatcher
    {
        private static Dictionary<string, FileSystemWatcher> m_watchers = new Dictionary<string, FileSystemWatcher>();
        private static Dictionary<string, Action> m_fileList = new Dictionary<string, Action>();
        private static Dictionary<string, WatchAction> m_directoryList = new Dictionary<string, WatchAction>();


        private static void CreateFileWatcher(string uri)
        {
            var watcher = new FileSystemWatcher(uri);
            watcher.EnableRaisingEvents = true;
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            m_watchers.Add(uri, watcher);
        }

        private static void CreateFolderWatcher(string uri)
        {
            var watcher = new FileSystemWatcher(uri);
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;
            watcher.Created += new FileSystemEventHandler(watcher_Created);
            m_watchers.Add(uri, watcher);
        }

        public static void RegisterFileModification(string fileName, Action action)
        {
            var fileInfo = new FileInfo(fileName);

            if (!fileInfo.Exists)
                throw new FileNotFoundException("Le fichier n'existe pas");

            if (!m_watchers.ContainsKey(fileInfo.DirectoryName))
                CreateFileWatcher(fileInfo.DirectoryName);

            m_fileList.Add(fileInfo.FullName, action);
        }

        public static void RegisterFileCreation(string directoryName, WatchAction action)
        {
            var directory = new DirectoryInfo(directoryName);

            if (!directory.Exists)
                throw new FileNotFoundException("Le répertoire n'existe pas");

            if (!m_watchers.ContainsKey(directory.FullName))
                CreateFolderWatcher(directory.FullName);

            m_directoryList.Add(directory.FullName, action);
        }

        static void watcher_Created(object sender, FileSystemEventArgs e)
        {
            var directory = e.FullPath.Remove(e.FullPath.LastIndexOf('\\'));
            if (m_directoryList.ContainsKey(directory))
            {
                m_directoryList[directory](e.FullPath);
            }
        }

        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (m_fileList.ContainsKey(e.FullPath))
            {
                var watcher = sender as FileSystemWatcher;
                watcher.EnableRaisingEvents = false;
                m_fileList[e.FullPath]();
                watcher.EnableRaisingEvents = true;
            }
        }
    }
}