﻿// --------------------------------------------------------
// Copyright:      Toni Kalajainen
// Date:           11.9.2019
// Url:            http://lexical.fi
// --------------------------------------------------------

namespace Lexical.FileSystem
{
    // <doc>
    /// <summary>
    /// File system that is actually a decoration.
    /// </summary>
    public interface IFileSystemDecoration : IFileSystem
    {
        /// <summary>
        /// (Optional) Original file system that is being decorated.
        /// </summary>
        IFileSystem Original { get; }
    }
    // </doc>

}