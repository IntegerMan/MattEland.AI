using System;
using System.Collections.Generic;
using System.Text;
using MattEland.AI.Neural.Functional;
using Microsoft.FSharp.Collections;
using Shouldly;
using Xunit;

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
            neuron.Inputs.Length.ShouldBe(1);
            neuron.Inputs[0].ShouldBe(connection);
        }

        [Theory]
        [InlineData( 1,  1,  1)]
        [InlineData( 1, -1, -1)]
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
    }
}
