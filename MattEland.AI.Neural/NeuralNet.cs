using System;
using System.Collections.Generic;
using System.Linq;

namespace MattEland.AI.Neural
{
    /// <summary>
    /// Represent a neural network consisting of an input layer, an output layer, and 0 to many hidden layers.
    /// Neural networks can compute values and return a set of output values, allowing for computation to occur
    /// between layers.
    /// </summary>
    public class NeuralNet
    {
        private readonly IList<NeuralNetLayer> _hiddenLayers = new List<NeuralNetLayer>();

        /// <summary>
        /// Creates a new instance of a <see cref="NeuralNet"/>
        /// </summary>
        /// <param name="numInputs">The number of nodes in the input layer</param>
        /// <param name="numOutputs">The number of nodes in the output layer</param>
        public NeuralNet(int numInputs, int numOutputs)
        {
            if (numInputs <=0) throw new ArgumentOutOfRangeException(nameof(numInputs), "You must have at least one input node");
            if (numOutputs <=0) throw new ArgumentOutOfRangeException(nameof(numOutputs), "You must have at least one output node");

            Inputs = new NeuralNetLayer(numInputs);
            Outputs = new NeuralNetLayer(numOutputs);
        }

        /// <summary>
        /// Adds a hidden layer to the neural net and returns the new layer.
        /// </summary>
        /// <param name="numNeurons">The number of neurons in the layer</param>
        /// <returns>The newly-created layer</returns>
        public NeuralNetLayer AddHiddenLayer(int numNeurons)
        {
            if (numNeurons <= 0) throw new ArgumentOutOfRangeException(nameof(numNeurons), "You cannot add a hidden layer without any nodes" );

            var layer = new NeuralNetLayer(numNeurons);

            _hiddenLayers.Add(layer);

            return layer;
        }

        /// <summary>
        /// Evaluates the result of the neural network given the specified set of <paramref name="inputs"/>.
        /// </summary>
        /// <param name="inputs">The inputs to evaluate.</param>
        /// <returns>The values outputted from the output layer</returns>
        public IEnumerable<decimal> Evaluate(IEnumerable<decimal> inputs)
        {
            // Don't force people to explicitly connect
            if (!IsConnected)
            {
                Connect();
            }

            // Pipe the inputs into the network and evaluate the results
            Inputs.SetValues(inputs);

            return Inputs.Evaluate();
        }

        /// <summary>
        /// Declares that the network is now complete and that connections should be created.
        /// </summary>
        private void Connect()
        {
            if (IsConnected) throw new InvalidOperationException("The Network has already been connected");

            if (_hiddenLayers.Any())
            {
                // Connect input to the first hidden layer
                Inputs.ConnectTo(_hiddenLayers.First());

                // Connect hidden layers to each other
                if (_hiddenLayers.Count > 1)
                {
                    for (int i = 0; i < _hiddenLayers.Count - 1; i++)
                    {
                        _hiddenLayers[i].ConnectTo(_hiddenLayers[i + 1]);
                    }
                }

                // Connect the last hidden layer to the output layer
                _hiddenLayers.Last().ConnectTo(Outputs);
            }
            else
            {
                // No hidden layers, connect the input layer to the output layer
                Inputs.ConnectTo(Outputs);
            }

            IsConnected = true;
        }

        /// <summary>
        /// Determines whether or not the nodes in the network have been connected.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// The input layer
        /// </summary>
        public NeuralNetLayer Inputs { get; }

        /// <summary>
        /// The output layer
        /// </summary>
        public NeuralNetLayer Outputs { get; }

        /// <summary>
        /// Gets all layers in the network, in order from first to last, including the Input layer,
        /// output layer, and any hidden layers.
        /// </summary>
        public IEnumerable<NeuralNetLayer> Layers
        {
            get
            {
                yield return Inputs;

                foreach (var layer in _hiddenLayers)
                {
                    yield return layer;
                }

                yield return Outputs;
            }
        }

        /// <summary>
        /// Sets the weights of all connections in the network. This is a convenience method for loading
        /// weight values from JSON and restoring them into the network.
        /// </summary>
        /// <param name="weights">The weight values from -1 to 1 for every connector in the network.</param>
        public void SetWeights(IList<decimal> weights)
        {
            ConnectorCount = 0;

            int weightIndex = 0;

            foreach (var layer in Layers)
            {
                foreach (var neuron in layer.Neurons)
                {
                    foreach (var connection in neuron.OutgoingConnections)
                    {
                        // Early exit if we've run out of weights to go around
                        if (weightIndex >= weights.Count)
                        {
                            break;
                        }

                        connection.Weight = weights[weightIndex++];
                        ConnectorCount++;
                    }
                }                
            }
        }

        /// <summary>
        /// Gets the total connector count in the neural net.
        /// </summary>
        public int ConnectorCount { get; private set; }
    }
}