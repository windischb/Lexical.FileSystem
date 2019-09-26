﻿// --------------------------------------------------------
// Copyright:      Toni Kalajainen
// Date:           9.9.2019
// Url:            http://lexical.fi
// --------------------------------------------------------
using Lexical.FileSystem.Internal;
using Lexical.FileSystem.Utility;
using System;

namespace Lexical.FileSystem
{
    /// <summary>
    /// Base implementation filesystem observer
    /// </summary>
    public abstract class FileSystemObserverHandleBase : DisposeList, IFileSystemObserver
    {
        /// <summary>
        /// The file system where the observer was attached.
        /// </summary>
        public IFileSystem FileSystem { get; protected set; }

        /// <summary>
        /// File glob-pattern filter.
        /// </summary>
        public string Filter { get; protected set; }

        /// <summary>
        /// Callback.
        /// </summary>
        public IObserver<IFileSystemEvent> Observer { get; protected set; }

        /// <summary>
        /// State object that was attached at construction.
        /// </summary>
        public object State { get; protected set; }

        /// <summary>
        /// Create observer.
        /// </summary>
        /// <param name="filesystem"></param>
        /// <param name="filter"></param>
        /// <param name="observer"></param>
        /// <param name="state"></param>
        protected FileSystemObserverHandleBase(IFileSystem filesystem, string filter, IObserver<IFileSystemEvent> observer, object state)
        {
            this.FileSystem = filesystem;
            Filter = filter;
            Observer = observer;
            State = state;

            // Catch dispose of parent filesystem
            if (filesystem is IDisposeList disposeList) disposeList.AddDisposable(this);
        }

        /// <summary>
        /// Dispose observer
        /// </summary>
        /// <param name="disposeErrors"></param>
        protected override void InnerDispose(ref StructList4<Exception> disposeErrors)
        {
            var _observer = Observer;

            // Remove watcher from dispose list.
            IFileSystem _filesystem = FileSystem;
            if (_filesystem is IDisposeList _disposelist) _disposelist.RemoveDisposable(this);

            // Call OnCompleted
            if (_observer != null)
            {
                Observer = null;
                try
                {
                    _observer.OnCompleted();
                }
                catch (Exception e)
                {
                    disposeErrors.Add(e);
                }
            }
        }
    }

    /// <summary>
    /// Base class for observer handle decoration.
    /// </summary>
    public class FileSystemObserverHandleDecoration : IFileSystemObserver
    {
        /// <summary>
        /// Decorate FileSystem value.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="newFileSystem"></param>
        /// <returns>decoration</returns>
        public static IFileSystemObserver DecorateFileSystem(IFileSystemObserver original, IFileSystem newFileSystem)
            => new NewFileSystem(original, newFileSystem);

        /// <inheritdoc/>
        public virtual IFileSystem FileSystem => original.FileSystem;
        /// <inheritdoc/>
        public virtual string Filter => original.Filter;
        /// <inheritdoc/>
        public virtual IObserver<IFileSystemEvent> Observer => original.Observer;
        /// <inheritdoc/>
        public virtual object State => original.State;
        /// <summary>
        /// Original observer handle
        /// </summary>
        protected IFileSystemObserver original;

        /// <summary>
        /// Create filesystem observer handle decoration.
        /// </summary>
        /// <param name="original"></param>
        public FileSystemObserverHandleDecoration(IFileSystemObserver original)
        {
            this.original = original ?? throw new ArgumentNullException(nameof(original));
        }

        /// <inheritdoc/>
        public virtual void Dispose()
            => original.Dispose();

        /// <summary>Class with overridden filesystem.</summary>
        protected class NewFileSystem : FileSystemObserverHandleDecoration
        {
            /// <summary>Overriding filesystem.</summary>
            protected IFileSystem newFilesystem;
            /// <summary>Return overridden filesystem.</summary>
            public override IFileSystem FileSystem => newFilesystem;
            /// <summary>
            /// Create decoration with new filesystem.
            /// </summary>
            /// <param name="original"></param>
            /// <param name="newFilesystem"></param>
            public NewFileSystem(IFileSystemObserver original, IFileSystem newFilesystem) : base(original)
            {
                this.newFilesystem = newFilesystem;
            }
        }
    }
}
