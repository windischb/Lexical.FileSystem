﻿// --------------------------------------------------------
// Copyright:      Toni Kalajainen
// Date:           14.6.2019
// Url:            http://lexical.fi
// --------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading.Tasks;

namespace Lexical.FileSystem
{
    /// <summary>File system options for browse.</summary>
    [Operations(typeof(BrowseOptionOperations))]
    // <IBrowseOption>
    public interface IBrowseOption : IOption
    {
        /// <summary>Has Browse capability.</summary>
        bool CanBrowse { get; }
        /// <summary>Has GetEntry capability.</summary>
        bool CanGetEntry { get; }
    }
    // </IBrowseOption>

    // <IFileSystemBrowse>
    /// <summary>
    /// File system that can browse directories.
    /// </summary>
    public interface IFileSystemBrowse : IFileSystem, IBrowseOption
    {
        /// <summary>
        /// Browse a directory for child entries.
        /// 
        /// <paramref name="path"/> should end with directory separator character '/', for example "mydir/".
        /// </summary>
        /// <param name="path">path to a directory, "" is root, separator is "/"</param>
        /// <param name="option">(optional) operation specific option; capability constraint, a session, security token or credential. Used for authenticating, authorizing or restricting the operation.</param>
        /// <returns>
        ///     Returns a snapshot of file and directory entries.
        ///     Note, that the returned array be internally cached by the implementation, and therefore the caller must not modify the array.
        /// </returns>
        /// <exception cref="IOException">On unexpected IO error</exception>
        /// <exception cref="SecurityException">If caller did not have permission</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> contains only white space, or contains one or more invalid characters</exception>
        /// <exception cref="NotSupportedException">The <see cref="IFileSystem"/> doesn't support browse</exception>
        /// <exception cref="UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified path, such as when access is Write or ReadWrite and the file or directory is set for read-only access.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters.</exception>
        /// <exception cref="InvalidOperationException">If <paramref name="path"/> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc.</exception>
        /// <exception cref="ObjectDisposedException"/>
        IDirectoryContent Browse(string path, IOption option = null);

        /// <summary>
        /// Get entry of a single file or directory.
        /// </summary>
        /// <param name="path">path to a directory or to a single file, "" is root, separator is "/"</param>
        /// <param name="option">(optional) operation specific option; capability constraint, a session, security token or credential. Used for authenticating, authorizing or restricting the operation.</param>
        /// <returns>entry, or null if entry is not found</returns>
        /// <exception cref="IOException">On unexpected IO error</exception>
        /// <exception cref="SecurityException">If caller did not have permission</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> contains only white space, or contains one or more invalid characters</exception>
        /// <exception cref="NotSupportedException">The <see cref="IFileSystem"/> doesn't support exists</exception>
        /// <exception cref="UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified path, such as when access is Write or ReadWrite and the file or directory is set for read-only access.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters.</exception>
        /// <exception cref="InvalidOperationException">If <paramref name="path"/> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc.</exception>
        /// <exception cref="ObjectDisposedException"/>
        IEntry GetEntry(string path, IOption option = null);
    }
    // </IFileSystemBrowse>

    // <IFileSystemBrowseAsync>
    /// <summary>
    /// File system that can browse directories.
    /// </summary>
    public interface IFileSystemBrowseAsync : IFileSystem, IBrowseOption
    {
        /// <summary>
        /// Browse a directory for child entries.
        /// 
        /// <paramref name="path"/> should end with directory separator character '/', for example "mydir/".
        /// </summary>
        /// <param name="path">path to a directory, "" is root, separator is "/"</param>
        /// <param name="option">(optional) operation specific option; capability constraint, a session, security token or credential. Used for authenticating, authorizing or restricting the operation.</param>
        /// <returns>
        ///     Returns a snapshot of file and directory entries.
        ///     Note, that the returned array be internally cached by the implementation, and therefore the caller must not modify the array.
        /// </returns>
        /// <exception cref="IOException">On unexpected IO error</exception>
        /// <exception cref="SecurityException">If caller did not have permission</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> contains only white space, or contains one or more invalid characters</exception>
        /// <exception cref="NotSupportedException">The <see cref="IFileSystem"/> doesn't support browse</exception>
        /// <exception cref="UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified path, such as when access is Write or ReadWrite and the file or directory is set for read-only access.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters.</exception>
        /// <exception cref="InvalidOperationException">If <paramref name="path"/> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc.</exception>
        /// <exception cref="ObjectDisposedException"/>
        Task<IDirectoryContent> BrowseAsync(string path, IOption option = null);

        /// <summary>
        /// Get entry of a single file or directory.
        /// </summary>
        /// <param name="path">path to a directory or to a single file, "" is root, separator is "/"</param>
        /// <param name="option">(optional) operation specific option; capability constraint, a session, security token or credential. Used for authenticating, authorizing or restricting the operation.</param>
        /// <returns>entry, or null if entry is not found</returns>
        /// <exception cref="IOException">On unexpected IO error</exception>
        /// <exception cref="SecurityException">If caller did not have permission</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> contains only white space, or contains one or more invalid characters</exception>
        /// <exception cref="NotSupportedException">The <see cref="IFileSystem"/> doesn't support exists</exception>
        /// <exception cref="UnauthorizedAccessException">The access requested is not permitted by the operating system for the specified path, such as when access is Write or ReadWrite and the file or directory is set for read-only access.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters.</exception>
        /// <exception cref="InvalidOperationException">If <paramref name="path"/> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc.</exception>
        /// <exception cref="ObjectDisposedException"/>
        Task<IEntry> GetEntryAsync(string path, IOption option = null);
    }
    // </IFileSystemBrowseAsync>

    // <IDirectoryContent>
    /// <summary>
    /// Browse result. Result is a snapshot of entries. The contents are immutable and reflect the contents at the time of browsing.
    /// </summary>
    public interface IDirectoryContent : IReadOnlyList<IEntry>
    {
        /// <summary>
        /// The filesystem where the browse was issued.
        /// </summary>
        IFileSystem FileSystem { get; }

        /// <summary>
        /// The browsed path at <see cref="FileSystem"/>.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// <see cref="Path"/> exists.
        /// </summary>
        bool Exists { get; }
    }
    // </IDirectoryContent>

}
