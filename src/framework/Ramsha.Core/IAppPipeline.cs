using System;
using System.Collections.Generic;
using System.Threading;

namespace Ramsha
{
    public interface IAppPipeline<TApp>
    {
        IAppPipeline<TApp> Use(string name, Action<TApp> configure, int priority = 0);
        IAppPipeline<TApp> UseBefore(string targetName, string name, Action<TApp> configure);
        IAppPipeline<TApp> UseAfter(string targetName, string name, Action<TApp> configure);

        IAppPipeline<TApp> Use(Action<TApp> configure, int priority = 0);
        IAppPipeline<TApp> UseBefore(string targetName, Action<TApp> configure);
        IAppPipeline<TApp> UseAfter(string targetName, Action<TApp> configure);

        IAppPipeline<TApp> MoveBefore(string entryName, string targetName);
        IAppPipeline<TApp> MoveAfter(string entryName, string targetName);
        bool Remove(string name);
        void Apply(TApp app);
        IReadOnlyList<PipelineEntry<TApp>> GetEntries();
    }

    public class AppPipeline<TApp> : IAppPipeline<TApp>
    {
        private readonly List<PipelineEntry<TApp>> _entries = new();
        private static int _anonymousIndex;

        public IReadOnlyList<PipelineEntry<TApp>> GetEntries() => _entries.AsReadOnly();

        private static string GetAnonymousName() => $"__anonymous_{Interlocked.Increment(ref _anonymousIndex)}";

        #region Use Overloads

        public IAppPipeline<TApp> Use(string name, Action<TApp> configure, int priority = 0)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            return AddEntry(configure, priority, name);
        }

        public IAppPipeline<TApp> Use(Action<TApp> configure, int priority = 0)
        {
            return AddEntry(configure, priority, GetAnonymousName());
        }

        #endregion

        #region UseBefore Overloads

        public IAppPipeline<TApp> UseBefore(string targetName, string name, Action<TApp> configure)
        {
            if (string.IsNullOrEmpty(targetName)) throw new ArgumentNullException(nameof(targetName));
            return InsertBefore(targetName, configure, name);
        }

        public IAppPipeline<TApp> UseBefore(string targetName, Action<TApp> configure)
        {
            return InsertBefore(targetName, configure, GetAnonymousName());
        }

        #endregion

        #region UseAfter Overloads

        public IAppPipeline<TApp> UseAfter(string targetName, string name, Action<TApp> configure)
        {
            if (string.IsNullOrEmpty(targetName)) throw new ArgumentNullException(nameof(targetName));
            return InsertAfter(targetName, configure, name);
        }

        public IAppPipeline<TApp> UseAfter(string targetName, Action<TApp> configure)
        {
            return InsertAfter(targetName, configure, GetAnonymousName());
        }

        #endregion

        #region Move & Remove

        public IAppPipeline<TApp> MoveBefore(string entryName, string targetName)
        {
            var index = _entries.FindIndex(e => e.Name == entryName);
            var targetIndex = _entries.FindIndex(e => e.Name == targetName);
            if (index < 0 || targetIndex < 0 || index == targetIndex) return this;

            var entry = _entries[index];
            _entries.RemoveAt(index);
            targetIndex = _entries.FindIndex(e => e.Name == targetName);
            _entries.Insert(targetIndex, entry);
            return this;
        }

        public IAppPipeline<TApp> MoveAfter(string entryName, string targetName)
        {
            var index = _entries.FindIndex(e => e.Name == entryName);
            var targetIndex = _entries.FindIndex(e => e.Name == targetName);
            if (index < 0 || targetIndex < 0 || index == targetIndex) return this;

            var entry = _entries[index];
            _entries.RemoveAt(index);
            targetIndex = _entries.FindIndex(e => e.Name == targetName);
            _entries.Insert(targetIndex + 1, entry);
            return this;
        }

        public bool Remove(string name)
        {
            var index = _entries.FindIndex(e => e.Name == name);
            if (index >= 0)
            {
                _entries.RemoveAt(index);
                return true;
            }
            return false;
        }

        #endregion

        #region Apply

        public void Apply(TApp app)
        {
            foreach (var entry in _entries)
                entry.Action(app);
        }

        #endregion

        #region Internal Helpers

        private IAppPipeline<TApp> AddEntry(Action<TApp> configure, int priority, string name)
        {
            var entry = new PipelineEntry<TApp>(configure, priority, name);
            _entries.Add(entry);
            SortEntries();
            return this;
        }

        private IAppPipeline<TApp> InsertBefore(string targetName, Action<TApp> configure, string name)
        {
            var index = _entries.FindIndex(e => e.Name == targetName);
            var entry = new PipelineEntry<TApp>(configure, index >= 0 ? _entries[index].Priority : 0, name);
            if (index >= 0) _entries.Insert(index, entry);
            else _entries.Insert(0, entry);
            SortEntries();
            return this;
        }

        private IAppPipeline<TApp> InsertAfter(string targetName, Action<TApp> configure, string name)
        {
            var index = _entries.FindIndex(e => e.Name == targetName);
            var entry = new PipelineEntry<TApp>(configure, index >= 0 ? _entries[index].Priority : 0, name);
            if (index >= 0) _entries.Insert(index + 1, entry);
            else _entries.Add(entry);
            SortEntries();
            return this;
        }

        private void SortEntries()
        {
            _entries.Sort((a, b) =>
            {
                var priorityCompare = a.Priority.CompareTo(b.Priority);
                return priorityCompare != 0 ? priorityCompare : a.InsertionIndex.CompareTo(b.InsertionIndex);
            });
        }

        #endregion
    }

    public class PipelineEntry<TApp>
    {
        private static int _globalIndex;

        public int InsertionIndex { get; }
        public Action<TApp> Action { get; }
        public int Priority { get; }
        public string Name { get; }

        public PipelineEntry(Action<TApp> action, int priority = 0, string name = "")
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
            Priority = priority;
            Name = string.IsNullOrEmpty(name) ? $"__unnamed_{Interlocked.Increment(ref _globalIndex)}" : name;
            InsertionIndex = Interlocked.Increment(ref _globalIndex);
        }

        public override string ToString()
        {
            return $"PipelineEntry(Name={Name}, Priority={Priority}, Index={InsertionIndex})";
        }
    }
}
