using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Stump.Core.IO.Watchers
{
    public class FileWatcher : FileSystemWatcher
    {
        private DateTime m_lastModification;

        public FileWatcher(string path, WatcherType type, WatchAction action)
            : base(System.IO.Path.GetDirectoryName(path))
        {
            FullPath = System.IO.Path.GetFullPath(path);
            Type = type;
            Action = action;

            // it's a directory so we add the sub dirs
            if (path.EndsWith(System.IO.Path.AltDirectorySeparatorChar.ToString()) ||
                path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
            {
                IncludeSubdirectories = true;
            }

            Deleted += OnDeleted;
            Created += OnCreated;
            Changed += OnChanged;

            EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (System.IO.Path.GetFullPath(e.FullPath) != FullPath || !Watching || Type != WatcherType.Modification ||
                !((DateTime.Now - m_lastModification).TotalMilliseconds > 100d)) return;
            m_lastModification = DateTime.Now;
            EnableRaisingEvents = false;
            Watching = false;
            Action(FullPath);
            Watching = true;
            EnableRaisingEvents = true;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == FullPath && Watching && Watching && Type == WatcherType.Creation)
            {
                Watching = false;
                Action(FullPath);
                Watching = true;
            }
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath != FullPath || !Watching || !Watching || Type != WatcherType.Deletion)
                return;

            Watching = false;
            Action(FullPath);
            Watching = true;
        }

        public WatchAction Action
        {
            get;
            set;
        }

        public string FullPath
        {
            get;
            set;
        }

        public WatcherType Type
        {
            get;
            set;
        }

        public bool Watching
        {
            get;
            set;
        }
    }
}