using System;
using System.Collections.Generic;
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
            OutgoingConnections = new List<NeuronConnection>();
        }

        /// <summary>
        /// Connects this Neuron to the <paramref name="nextNeuron" />
        /// </summary>
        /// <param name="nextNeuron">The Neuron to connect to.</param>
        internal void ConnectTo([NotNull] Neuron nextNeuron)
        {
            if (nextNeuron == null) throw new ArgumentNullException(nameof(nextNeuron));

            OutgoingConnections.Add(new NeuronConnection(this, nextNeuron));
        }

        private decimal _sum;
        private int _numInputs;

        /// <summary>
        /// Evaluates the values from the incoming connections, averages them by the count of connections,
        /// and calculates the Neuron's Value, which is then passed on to any outgoing connections.
        /// </summary>
        internal void Evaluate()
        {
            if (_numInputs > 0)
            {
                Value = _sum / _numInputs;
                _sum = 0;
            }

            OutgoingConnections.Each(c => c.Fire(Value));
        }

        /// <summary>
        /// The list of outgoing Neuron connections
        /// </summary>
        [NotNull, ItemNotNull]
        public IList<NeuronConnection> OutgoingConnections { get; }

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

            _numInputs++;
        }
    }
}