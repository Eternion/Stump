using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stump.Core.IO.Watchers
{
    public delegate void WatchAction(string fileName);

    public enum WatcherType
    {
        Creation,
        Modification,
        Deletion,
    }

    public static class FileWatcherManager
    {
        private static readonly List<FileWatcher> Watchers = new List<FileWatcher>();

        public static void Watch(string path, WatcherType watcherType, Action action)
        {
            Watch(path, watcherType, entry => action());
        }

        public static void Watch(string path, WatcherType watcherType, WatchAction action)
        {
            var watcher = new FileWatcher(Path.GetFullPath(path), watcherType, action)
                { Watching = true };

            Watchers.Add(watcher);
        }

        public static void StopWatching(string path)
        {
            var watchers = Watchers.Where(entry => 
                entry.FullPath == Path.GetFullPath(path)).ToArray();

            foreach (var watcher in watchers)
            {
                watcher.Watching = false;
                watcher.Dispose();
                Watchers.Remove(watcher);
            }
        }
    }
}