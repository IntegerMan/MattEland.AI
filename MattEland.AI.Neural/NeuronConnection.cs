using System;
using JetBrains.Annotations;

namespace MattEland.AI.Neural
{
    /// <summary>
    /// Represents a unidirectional connection between two <see cref="Neuron"/> instances.
    /// </summary>
    public class NeuronConnection
    {
        [NotNull] private readonly Neuron _target;

        /// <summary>
        /// Creates a new <see cref="NeuronConnection"/> between a source neuron and
        /// a target neuron.
        /// </summary>
        /// <param name="target">The neuron that the connection flows to</param>
        public NeuronConnection([NotNull] Neuron target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));

            target.RegisterIncomingConnection(this);
        }

        /// <summary>
        /// Fires the connection, passing off the value to the target Neuron, taking the connection's
        /// Weight into account.
        /// </summary>
        /// <param name="value">The value to pass on to the Target.</param>
        internal void Fire(decimal value) => _target.Receive(value * Weight);

        /// <summary>
        /// Gets or sets the weight of the connection. This is the importance of that connection
        /// as far as the calculations go. This is typically a value between -1 and 1.
        /// </summary>
        public decimal Weight { get; set; } = 1;

    }
}