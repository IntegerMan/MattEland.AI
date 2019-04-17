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
    }
}
