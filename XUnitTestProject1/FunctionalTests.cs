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
            foreach (var v in values)
            {
                neurons[index++].Value.ShouldBe(v);
            }
        }
    }
}
