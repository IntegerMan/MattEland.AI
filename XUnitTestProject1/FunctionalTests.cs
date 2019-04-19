using System;
using System.Collections.Generic;
using System.Linq;
using MattEland.AI.Neural.Functional;
using MattEland.Shared.Collections;
using Shouldly;
using Xunit;
using Neuron = MattEland.AI.Neural.Functional.Neuron;
using NeuronConnection = MattEland.AI.Neural.Functional.NeuronConnection;

namespace MattEland.AI.Tests
{
    public class FunctionalTests
    {

        [Fact]
        public void AddConnectionRegistersConnection()
        {
            // Arrange
            var neuron = new Neuron();
            var connection = new NeuronConnection(neuron, 1m);

            // Act
            neuron.AddIncomingConnection(connection);

            // Assert
            neuron.Inputs.Single().ShouldBe(connection);
        }

        [Fact]
        public void NeuronShouldRespectInitialValue()
        {
            // Arrange / Act
            var neuron = new Neuron(42);

            // Assert
            neuron.Value.ShouldBe(42);
        }

        [Theory]
        [InlineData( 1,  1,  1)]
        [InlineData( 1, -1, -1)]
        [InlineData( 1, 0, 0)]
        [InlineData( 0, 1, 0)]
        [InlineData( -1, 0.5, -0.5)]
        public void EvaluateNeuronShouldResultInCorrectTotal(decimal input, decimal weight, decimal expected)
        {
            // Arrange
            var source = new Neuron();
            var target = new Neuron();
            var connection = new NeuronConnection(source, weight);
            target.AddIncomingConnection(connection);
            source.Value = input;

            // Act
            decimal sum = target.Evaluate();

            // Assert
            sum.ShouldBe(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(42)]
        public void NeuralNetLayerShouldHaveCorrectNumberOfNodes(int expectedNodes)
        {
            // Arrange / Act
            var layer = new NeuralNetLayer(expectedNodes);

            // Assert
            layer.Neurons.ShouldNotBeNull();
            layer.Neurons.Count().ShouldBe(expectedNodes);
            layer.Neurons.Each(n => n.ShouldNotBeNull());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void InvalidInputsToNeuralNetsShouldThrowArgEx(int numNodes)
        {
            // Arrange / Act / Assert
            Should.Throw<ArgumentException>(() => new NeuralNetLayer(numNodes));
        }

        [Fact]
        public void SetNodeValuesShouldModifyNodes()
        {
            // Arrange
            var layer = new NeuralNetLayer(3);
            var values = new List<decimal> {1,2,3};

            // Act
            layer.SetValues(values);

            // Assert
            var neurons = layer.Neurons.ToList();
            int index = 0;
            values.Each(v => neurons[index++].Value.ShouldBe(v));
        }

        [Theory]
        [InlineData(5, 3)]
        [InlineData(1, 10)]
        public void CreateNetworkShouldHaveCorrectNodeCounts(int numInputs, int numOutputs)
        {
            // Arrange / Act
            var network = new NeuralNet(numInputs, numOutputs);

            // Assert
            network.Layers.Count().ShouldBe(2);
            network.InputLayer.Neurons.Count().ShouldBe(numInputs);
            network.OutputLayer.Neurons.Count().ShouldBe(numOutputs);
        }

        [Theory]
        [InlineData(5, 3)]
        [InlineData(1, 10)]
        public void EvaluateNetworkShouldMatchOutputLayer(int numInputs, int numOutputs)
        {
            // Arrange
            var network = new NeuralNet(numInputs, numOutputs);
            network.InputLayer.Neurons.Each(n => n.Value = 0.5M);

            // Act
            var evalResult = network.Evaluate().ToList();

            // Assert
            int index = 0;
            network.OutputLayer.Neurons.Each(n => n.Value.ShouldBe(evalResult[index++]));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.5)]
        [InlineData(1)]
        [InlineData(-0.25)]
        [InlineData(-1)]
        public void EvaluateNetworkShouldCompute(decimal value)
        {
            // Arrange
            int numInputs = 3;
            var network = new NeuralNet(numInputs, 1);
            network.InputLayer.Neurons.Each(n => n.Value = value);

            // Act
            network.Evaluate().ToList();

            // Assert
            network.OutputLayer.Neurons.Each(n => n.Value.ShouldBe(value * numInputs / numInputs));
        }
        
        [Fact]
        public void HiddenLayersShouldBeIncluded()
        {
            // Arrange
            var network = new NeuralNet(3, 1);
            var hidden = new NeuralNetLayer(5);

            // Act
            network.AddHiddenLayer(hidden
);

            // Assert
            var layers = network.Layers.ToList();
            layers.Count.ShouldBe(3);
            layers[0].ShouldBe(network.InputLayer);
            layers[1].ShouldBe(hidden);
            layers[2].ShouldBe(network.OutputLayer);
        }

        [Fact]
        public void HiddenLayersShouldBeConnectionTargets()
        {
            // Arrange
            var numInputLayer = 3;
            var network = new NeuralNet(3, 1);
            var hidden = new NeuralNetLayer(2);

            // Act
            network.AddHiddenLayer(hidden);
            network.Connect();

            // Assert
            hidden.Neurons.Each(n => n.Inputs.Count().ShouldBe(numInputLayer));
        }

        [Fact]
        public void SetWeightsShouldWork()
        {
            // Arrange
            var network = new NeuralNet(2, 1);
            var hidden = new NeuralNetLayer(2);
            network.AddHiddenLayer(hidden);
            var weights = new List<decimal> {1, -1, 0.5M, -0.5M, 1, -1};

            // Act
            network.SetWeights(weights);

            // Assert
            hidden.Neurons.First().Inputs.First().Weight.ShouldBe(1);
            hidden.Neurons.First().Inputs.Last().Weight.ShouldBe(-1);
            hidden.Neurons.Last().Inputs.First().Weight.ShouldBe(0.5M);
            hidden.Neurons.Last().Inputs.Last().Weight.ShouldBe(-0.5M);
            network.OutputLayer.Neurons.Single().Inputs.First().Weight.ShouldBe(1);
            network.OutputLayer.Neurons.Single().Inputs.Last().Weight.ShouldBe(-1);
        }

        [Fact]
        public void NeuralNetEvaluation()
        {
            // Arrange
            var network = new NeuralNet(2, 1);
            var hidden = new NeuralNetLayer(2);
            network.AddHiddenLayer(hidden);
            var weights = new List<decimal> {1, -1, 0.5M, -0.5M, 1, -1};
            var values = new List<decimal> {1, -1};

            // Act
            network.SetWeights(weights);
            network.InputLayer.SetValues(values);
            network.Evaluate();

            // Assert
            network.InputLayer.Neurons.First().Value.ShouldBe(1);
            network.InputLayer.Neurons.Last().Value.ShouldBe(-1);
            hidden.Neurons.First().Value.ShouldBe(1);
            hidden.Neurons.Last().Value.ShouldBe(0.5M);
            network.OutputLayer.Neurons.Single().Value.ShouldBe(0.25M);
        }

        [Fact]
        public void NodeEvaluationShouldSetNewNodeValue()
        {
            // Arrange
            var input = new NeuralNetLayer(1);
            var target = new NeuralNetLayer(1);
            input.Neurons.First().Connect(target.Neurons.First());
            input.Neurons.First().Value = 0.5M;

            // Act
            var evalResult = target.Evaluate();

            // Assert
            target.Neurons.First().Inputs.Single().Weight.ShouldBe(1M);
            target.Neurons.First().Inputs.Single().Source.ShouldBe(input.Neurons.First());
            target.Neurons.First().Value.ShouldBe(0.5M);
        }
    }
}
