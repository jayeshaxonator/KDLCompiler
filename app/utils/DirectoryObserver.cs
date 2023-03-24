using System;
using System.Collections.Generic;
using System.IO;

namespace KDLCompiler
{
    public class DirectoryObserver
    {
        private readonly FileSystemWatcher watcher;
        private readonly string extension;
        private readonly FileIntegrityChecker integrityChecker;

        public event EventHandler<List<string>> FilesChanged;

        public DirectoryObserver(string path, string extension)
        {
            this.watcher = new FileSystemWatcher();
            this.watcher.Path = path;
            this.watcher.IncludeSubdirectories = true;
            this.watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            this.watcher.Filter = $"*.{extension}";
            this.watcher.Changed += new FileSystemEventHandler(this.OnFileChanged);
            this.watcher.Created += new FileSystemEventHandler(this.OnFileChanged);
            this.watcher.Deleted += new FileSystemEventHandler(this.OnFileChanged);
            this.extension = extension;
            this.integrityChecker = new FileIntegrityChecker(path);
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            //Console.WriteLine($"File {e.FullPath} {e.ChangeType}");

            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                //this.integrityChecker.RemoveHash(e.FullPath);
            }
            else
            {
                //this.integrityChecker.UpdateHash(e.FullPath);
            }

            List<string> addedFiles = new List<string>();
            List<string> deletedFiles = new List<string>();
            List<string> modifiedFiles = new List<string>();

            this.integrityChecker.CheckIntegrity();

            List<string> changedFiles = new List<string>();
            changedFiles.AddRange(integrityChecker.GetAddedFiles());
            changedFiles.AddRange(integrityChecker.GetDeletedFiles());
            changedFiles.AddRange(integrityChecker.GetModifiedFiles());
            integrityChecker.Reset();
            this.FilesChanged?.Invoke(this, changedFiles);
        }

        public void StartObserving()
        {
            this.watcher.EnableRaisingEvents = true;
        }

        public void StopObserving()
        {
            this.watcher.EnableRaisingEvents = false;
        }
    }
}
