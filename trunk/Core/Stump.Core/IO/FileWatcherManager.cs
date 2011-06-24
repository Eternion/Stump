using System;
using System.Collections.Generic;
using System.IO;

namespace Stump.Core.IO
{
    public delegate void WatchAction(string fileName);

    public static class FileWatcherManager
    {
        private static readonly Dictionary<string, FileSystemWatcher> m_watchers = new Dictionary<string, FileSystemWatcher>();
        private static readonly Dictionary<string, Action> m_fileList = new Dictionary<string, Action>();
        private static readonly Dictionary<string, WatchAction> m_directoryList = new Dictionary<string, WatchAction>();


        private static void CreateFileWatcher(string uri)
        {
            var watcher = new FileSystemWatcher(uri) {EnableRaisingEvents = true};
            watcher.Changed += OnWatcherChanged;
            m_watchers.Add(uri, watcher);
        }

        private static void CreateFolderWatcher(string uri)
        {
            var watcher = new FileSystemWatcher(uri) {EnableRaisingEvents = true, IncludeSubdirectories = true};
            watcher.Created += OnWatcherCreated;
            m_watchers.Add(uri, watcher);
        }

        public static void RegisterFileModification(string fileName, Action action)
        {
            var fileInfo = new FileInfo(fileName);

            if (!fileInfo.Exists)
                throw new FileNotFoundException("File not found");

            if (!m_watchers.ContainsKey(fileInfo.DirectoryName))
                CreateFileWatcher(fileInfo.DirectoryName);

            m_fileList.Add(fileInfo.FullName, action);
        }

        public static void RegisterFileCreation(string directoryName, WatchAction action)
        {
            var directory = new DirectoryInfo(directoryName);

            if (!directory.Exists)
                throw new FileNotFoundException("Directory not found");

            if (!m_watchers.ContainsKey(directory.FullName))
                CreateFolderWatcher(directory.FullName);

            m_directoryList.Add(directory.FullName, action);
        }

        private static void OnWatcherCreated(object sender, FileSystemEventArgs e)
        {
            string directory = e.FullPath.Remove(e.FullPath.LastIndexOf('\\'));
            if (m_directoryList.ContainsKey(directory))
            {
                m_directoryList[directory](e.FullPath);
            }
        }

        private static void OnWatcherChanged(object sender, FileSystemEventArgs e)
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