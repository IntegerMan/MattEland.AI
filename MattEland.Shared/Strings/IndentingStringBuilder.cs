using System;
using System.Linq;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using MattEland.Shared.Collections;

namespace MattEland.Shared.Strings
{
    /// <summary>
    /// A StringBuilder wrapper that allows for indenting and outdenting code
    /// </summary>
    public class IndentingStringBuilder : IStringBuilder
    {
        [NotNull]
        private readonly StringBuilder _sb;
        private int _indentLevel;

        /// <summary>
        /// Creates a new instance of the IndentingStringBuilder, using a new underlying StringBuilder
        /// </summary>
        public IndentingStringBuilder() : this(new StringBuilder())
        {
        }

        /// <summary>
        /// Creates a new instance of the IndentingStringBuilder, using a new underlying StringBuilder that starts
        /// with the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The string to start with in the string builder</param>
        [UsedImplicitly]
        public IndentingStringBuilder(string value) : this(new StringBuilder(value))
        {
        }

        /// <summary>
        /// Creates a new instance of the IndentingStringBuilder, using an existing StringBuilder.
        /// </summary>
        /// <param name="source">The existing <see cref="StringBuilder"/> to wrap around</param>
        public IndentingStringBuilder(StringBuilder source)
        {
            _sb = source;
        }

        /// <inheritdoc />
        public override string ToString() => _sb.ToString();

        /// <inheritdoc />
        public void Indent() => Interlocked.Increment(ref _indentLevel);

        /// <inheritdoc />
        public void Outdent() => Interlocked.Decrement(ref _indentLevel);

        /// <inheritdoc />
        public void AppendLine() => _sb.AppendLine();

        /// <inheritdoc />
        public void AppendLine(string message) => _sb.AppendLine($"{IndentString}{message}");

        private string IndentString
        {
            get
            {
                var level = _indentLevel;
                if (level <= 0) return string.Empty;

                string indent = string.Empty;

                Enumerable.Range(0, level).Each(l => indent += "\t");

                return indent;
            }
        }

        /// <inheritdoc />
        public IDisposable IndentScope() => new IndentScopeHelper(this);

    }
}
