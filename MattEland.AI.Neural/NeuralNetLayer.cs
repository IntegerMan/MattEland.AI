using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace MattEland.AI.Neural
{
    public class NeuralNetLayer : IEnumerable<Neuron>
    {
        private readonly IList<Neuron> _neurons;

        private NeuralNetLayer _nextLayer;

        public NeuralNetLayer(int numNeurons)
        {
            _neurons = new List<Neuron>(numNeurons);

            for (int i = 0; i < numNeurons; i++)
            {
                _neurons.Add(new Neuron());
            }
        }

        public IEnumerable<Neuron> Neurons => _neurons;

        public void SetValues([NotNull] IEnumerable<decimal> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Count() != _neurons.Count)
            {
                throw new ArgumentException("The number of inputs must match the number of neurons in a layer", nameof(values));
            }

            int i = 0;
            foreach (var value in values)
            {
                _neurons[i++].Value = value;
            }
        }

        public IEnumerator<Neuron> GetEnumerator()
        {
            return _neurons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Evaluate()
        {
            foreach (var neuron in _neurons)
            {
                neuron.Evaluate();
            }

            _nextLayer?.Evaluate();
        }

        public void ConnectTo([NotNull] NeuralNetLayer nextLayer)
        {
            _nextLayer = nextLayer ?? throw new ArgumentNullException(nameof(nextLayer));

            foreach (var neuron in _neurons)
            {
                foreach (var nextNeuron in nextLayer)
                {
                    neuron.ConnectTo(nextNeuron);
                }
            }
        }

        public void SetWeights(IList<decimal> weights)
        {
            int weightIndex = 0;

            foreach (var neuron in _neurons)
            {
                foreach (var connection in neuron.OutgoingConnections)
                {
                    connection.Weight = weights[weightIndex++];
                }
            }
        }
    }
}