using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MattEland.Shared.Collections;

namespace MattEland.AI.Neural
{
    /// <summary>
    /// Represents a Neuron in a layer of a Neural Network.
    /// </summary>
    public class Neuron
    {
        /// <summary>
        /// Gets or sets the value of the Neuron.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="Neuron"/>.
        /// </summary>
        public Neuron()
        {
            IncomingConnections = new List<NeuronConnection>();
            OutgoingConnections = new List<NeuronConnection>();
        }

        /// <summary>
        /// Connects this Neuron to the <paramref name="nextNeuron" />
        /// </summary>
        /// <param name="nextNeuron">The Neuron to connect to.</param>
        public void ConnectTo([NotNull] Neuron nextNeuron)
        {
            if (nextNeuron == null) throw new ArgumentNullException(nameof(nextNeuron));

            OutgoingConnections.Add(new NeuronConnection(this, nextNeuron));
        }

        private decimal _sum;

        /// <summary>
        /// Evaluates the values from the incoming connections, averages them by the count of connections,
        /// and calculates the Neuron's Value, which is then passed on to any outgoing connections.
        /// </summary>
        public void Evaluate()
        {
            if (IncomingConnections.Any())
            {
                Value = _sum / IncomingConnections.Count;
                _sum = 0;
            }

            OutgoingConnections.Each(c => c.Fire(Value));
        }

        /// <summary>
        /// The list of outgoing Neuron connections
        /// </summary>
        public IList<NeuronConnection> OutgoingConnections { get; }

        /// <summary>
        /// The list of incoming Neuron connections
        /// </summary>
        /// <remarks>
        /// This presently has no use as nothing flows backwards.
        /// </remarks>
        public IList<NeuronConnection> IncomingConnections { get; }

        /// <summary>
        /// Receives a value from a connection.
        /// </summary>
        /// <param name="value">The value to receive</param>
        internal void Receive(decimal value) => _sum += value;

        /// <summary>
        /// Registers an incoming connection from another neuron.
        /// </summary>
        /// <param name="neuronConnection">The connection</param>
        internal void RegisterIncomingConnection([NotNull] NeuronConnection neuronConnection)
        {
            if (neuronConnection == null) throw new ArgumentNullException(nameof(neuronConnection));

            // TODO: I actually don't even need to track this.
            IncomingConnections.Add(neuronConnection);
        }
    }
}