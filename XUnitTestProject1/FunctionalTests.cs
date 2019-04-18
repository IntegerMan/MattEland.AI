using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MattEland.AI.Neural.Functional;
using MattEland.Shared.Collections;
using Microsoft.FSharp.Collections;
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
            network.OutputLayer.Neurons.Each(n => n.Value.ShouldBe(value * numInputs));
        }
    }
}
