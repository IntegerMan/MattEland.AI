using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MattEland.Shared.Collections;

namespace MattEland.AI.Neural
{
    /// <summary>
    /// Represents a layer in a neural network. This could be an input, output, or hidden layer.
    /// </summary>
    public class NeuralNetLayer : IEnumerable<Neuron>
    {
        private readonly IList<Neuron> _neurons;

        [CanBeNull]
        private NeuralNetLayer _nextLayer;

        /// <summary>
        /// Creates a new neural network layer with the given count of neurons.
        /// </summary>
        /// <param name="numNeurons">The number of neurons in the layer</param>
        public NeuralNetLayer(int numNeurons)
        {
            _neurons = new List<Neuron>(numNeurons);

            numNeurons.Each(n => _neurons.Add(new Neuron()));
        }

        /// <summary>
        /// Gets the Neurons belonging to this layer.
        /// </summary>
        public IEnumerable<Neuron> Neurons => _neurons;

        public void SetValues([NotNull] IEnumerable<decimal> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (values.Count() != _neurons.Count) throw new ArgumentException("The number of inputs must match the number of neurons in a layer", nameof(values));

            int i = 0;
            values.Each(v => _neurons[i++].Value = v);
        }

        /// <inheritdoc />
        public IEnumerator<Neuron> GetEnumerator() => _neurons.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Evaluates each node in the layer, as well as the next layer if one is present.
        /// </summary>
        /// <returns>The outputs from the Output layer</returns>
        internal IEnumerable<decimal> Evaluate()
        {
            // Calculate all neurons.
            _neurons.Each(n => n.Evaluate());

            // If this is the last layer, return its values, otherwise delegate to the next layer and return its results
            return _nextLayer == null 
                ? _neurons.Select(n => n.Value) 
                : _nextLayer.Evaluate();
        }

        /// <summary>
        /// Connects this layer to the <paramref name="nextLayer"/>, forming connections between each node in this
        /// layer and each node in the next layer.
        /// </summary>
        /// <param name="nextLayer">The layer to connect to</param>
        internal void ConnectTo([NotNull] NeuralNetLayer nextLayer)
        {
            _nextLayer = nextLayer ?? throw new ArgumentNullException(nameof(nextLayer));

            _neurons.Each(source => nextLayer.Each(source.ConnectTo));
        }

        /// <summary>
        /// Sets the weights in the layer to the values provided
        /// </summary>
        /// <param name="weights">The weights to use to set in the connections</param>
        public void SetWeights(IList<decimal> weights)
        {
            int weightIndex = 0;
            _neurons.Each(neuron => neuron.OutgoingConnections.Each(c => c.Weight = weights[weightIndex++]));
        }
    }
}