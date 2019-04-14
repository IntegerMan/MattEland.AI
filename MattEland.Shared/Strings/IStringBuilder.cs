using System;
using JetBrains.Annotations;

namespace MattEland.Shared.Strings
{
    /// <summary>
    /// An interface for a string builder wrapper.
    /// </summary>
    /// <remarks>
    /// This is a bit cleaner than passing IndentingStringBuilder around everywhere, but I still don't love it.
    /// </remarks>
    public interface IStringBuilder
    {
        /// <summary>
        /// Indents the display, causing future lines to be indented.
        /// </summary>
        [UsedImplicitly]
        void Indent();

        /// <summary>
        /// Outdents the display, causing future lines to be less indented.
        /// </summary>
        [UsedImplicitly]
        void Outdent();

        /// <summary>
        /// Appends an empty line.
        /// </summary>
        void AppendLine();

        /// <summary>
        /// Appends a line to the display, respecting the current indentation level
        /// </summary>
        void AppendLine(string message);

        /// <summary>
        /// Creates an IDisposable scope that will invoke Indent on enter and Outdent on dispose.
        /// </summary>
        /// <returns>An IDisposable indentation scope, intended to be stored in a using Statement</returns>
        IDisposable IndentScope();
    }
}