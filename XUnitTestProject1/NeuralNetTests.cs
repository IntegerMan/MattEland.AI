using System.Collections.Generic;
using System.Linq;
using MattEland.AI.Neural;
using Shouldly;
using Xunit;

namespace MattEland.AI.Tests
{
    public class NeuralNetTests
    {
        [Fact]
        public void NeuralNetShouldCalculate()
        {
            // Arrange
            var net = new NeuralNet(1, 1);
            var inputs = new List<decimal> { 1 };
            
            // Act
            var outputs = net.Evaluate(inputs);

            // Assert
            outputs.ShouldNotBeNull();
            outputs.First().ShouldBe(1);
        }
    }
}
