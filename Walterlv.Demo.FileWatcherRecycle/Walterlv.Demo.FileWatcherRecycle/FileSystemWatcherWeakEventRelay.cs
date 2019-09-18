using System.IO;

namespace Walterlv.Demo.FileWatcherRecycle
{
    internal sealed class FileSystemWatcherWeakEventRelay : WeakEventRelay<FileSystemWatcher>
    {
        public FileSystemWatcherWeakEventRelay(FileSystemWatcher eventSource) : base(eventSource) { }

        private readonly WeakEvent<FileSystemEventHandler> _created = new WeakEvent<FileSystemEventHandler>();
        private readonly WeakEvent<FileSystemEventHandler> _changed = new WeakEvent<FileSystemEventHandler>();
        private readonly WeakEvent<RenamedEventHandler> _renamed = new WeakEvent<RenamedEventHandler>();
        private readonly WeakEvent<FileSystemEventHandler> _deleted = new WeakEvent<FileSystemEventHandler>();

        public event FileSystemEventHandler Created
        {
            add => Subscribe(o => o.Created += OnCreated, () => _created.Add(value));
            remove => Unsubscribe(o => o.Created -= OnCreated, () => _created.Remove(value));
        }

        public event FileSystemEventHandler Changed
        {
            add => Subscribe(o => o.Changed += OnChanged, () => _changed.Add(value));
            remove => Unsubscribe(o => o.Changed -= OnChanged, () => _changed.Remove(value));
        }

        public event RenamedEventHandler Renamed
        {
            add => Subscribe(o => o.Renamed += OnRenamed, () => _renamed.Add(value));
            remove => Unsubscribe(o => o.Renamed -= OnRenamed, () => _renamed.Remove(value));
        }

        public event FileSystemEventHandler Deleted
        {
            add => Subscribe(o => o.Deleted += OnDeleted, () => _deleted.Add(value));
            remove => Unsubscribe(o => o.Deleted -= OnDeleted, () => _deleted.Remove(value));
        }

        private void OnCreated(object sender, FileSystemEventArgs e) => TryInvoke(() => _created.Invoke(sender, e));
        private void OnChanged(object sender, FileSystemEventArgs e) => TryInvoke(() => _changed.Invoke(sender, e));
        private void OnRenamed(object sender, RenamedEventArgs e) => TryInvoke(() => _renamed.Invoke(sender, e));
        private void OnDeleted(object sender, FileSystemEventArgs e) => TryInvoke(() => _deleted.Invoke(sender, e));

        protected override void OnRefereceLost(FileSystemWatcher source)
        {
            source.Created -= OnCreated;
            source.Changed -= OnChanged;
            source.Renamed -= OnRenamed;
            source.Deleted -= OnDeleted;
        }
    }
}
