using System;
using JetBrains.Annotations;

namespace MattEland.Shared.Strings
{
    /// <summary>
    /// A simple <see cref="IDisposable"/> that will invoke Indent and Outdent on a <see cref="IStringBuilder"/> on scope entered and exited.
    /// </summary>
    internal class IndentScopeHelper : IDisposable
    {
        [NotNull]
        private readonly IStringBuilder _sb;

        /// <summary>
        /// Creates a new scope and indents the <paramref name="stringBuilder"/>
        /// </summary>
        /// <param name="stringBuilder">The source <see cref="IStringBuilder"/></param>
        public IndentScopeHelper([NotNull] IStringBuilder stringBuilder)
        {
            _sb = stringBuilder ?? throw new ArgumentNullException(nameof(stringBuilder));

            _sb.Indent();
        }

        /// <summary>
        /// Disposes the object and outdents the StringBuilder
        /// </summary>
        public void Dispose()
        {
            _sb.Outdent();
        }
    }
}