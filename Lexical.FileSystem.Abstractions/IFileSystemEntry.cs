﻿// --------------------------------------------------------
// Copyright:      Toni Kalajainen
// Date:           14.6.2019
// Url:            http://lexical.fi
// --------------------------------------------------------
using System;

namespace Lexical.FileSystem
{
    // <doc>
    /// <summary>
    /// Generic entry used by <see cref="IFileSystem"/>.
    /// 
    /// The entry represents the snapshot state at the time of creation.
    /// 
    /// See sub-interfaces:
    /// <list type="bullet">
    ///     <item><see cref="IFileSystemEntryFile"/></item>
    ///     <item><see cref="IFileSystemEntryDirectory"/></item>
    ///     <item><see cref="IFileSystemEntryDrive"/></item>
    /// </list>    
    /// </summary>
    public interface IFileSystemEntry
    {
        /// <summary>
        /// (optional) Associated file system.
        /// </summary>
        IFileSystem FileSystem { get; }

        /// <summary>
        /// Path that is relative to the <see cref="IFileSystem"/>.
        /// Separator is "/".
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Entry name in its parent context.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Date time of last modification. In UTC time, if possible.
        /// </summary>
        DateTimeOffset LastModified { get; }
    }

    /// <summary>
    /// File entry
    /// </summary>
    public interface IFileSystemEntryFile : IFileSystemEntry
    {
        /// <summary>
        /// Tests if entry represents a file.
        /// </summary>
        bool IsFile { get; }

        /// <summary>
        /// File length. -1 if is length is unknown.
        /// </summary>
        long Length { get; }
    }

    /// <summary>
    /// Directory entry that can be browsed for contents with <see cref="IFileSystemBrowse"/>.
    /// </summary>
    public interface IFileSystemEntryDirectory : IFileSystemEntry
    {
        /// <summary>
        /// Tests if entry represents a directory.
        /// </summary>
        bool IsDirectory { get; }
    }
    // </doc>

    /// <summary>
    /// Drive entry. 
    /// 
    /// If drive class is browsable, then the implementation also implements <see cref="IFileSystemEntryDirectory"/>.
    /// </summary>
    public interface IFileSystemEntryDrive : IFileSystemEntry
    {
        /// <summary>
        /// Tests if entry represents a drive.
        /// </summary>
        bool IsDrive { get; }
    }

    /// <summary>
    /// Entry that is actually a decoration.
    /// </summary>
    public interface IFileSystemEntryDecoration : IFileSystemEntry
    {
        /// <summary>
        /// (Optional) Original entry that is being decorated.
        /// </summary>
        IFileSystemEntry Original { get; }
    }

    /// <summary>
    /// Extension methods for <see cref="IFileSystemEntry"/>.
    /// </summary>
    public static partial class IFileSystemEntryExtensions
    {
        /// <summary>
        /// Get file legnth
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static long Length(this IFileSystemEntry entry)
            => entry is IFileSystemEntryFile file ? file.Length : -1L;

        /// <summary>
        /// Tests if <paramref name="entry"/> represents a file.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static bool IsFile(this IFileSystemEntry entry)
            => entry is IFileSystemEntryFile file ? file.IsFile : false;

        /// <summary>
        /// Tests if <paramref name="entry"/> represents a directory.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static bool IsDirectory(this IFileSystemEntry entry)
            => entry is IFileSystemEntryDirectory dir ? dir.IsDirectory : false;

        /// <summary>
        /// Tests if <paramref name="entry"/> represents a drive.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static bool IsDrive(this IFileSystemEntry entry)
            => entry is IFileSystemEntryDrive drive ? drive.IsDrive : false;
    }

}