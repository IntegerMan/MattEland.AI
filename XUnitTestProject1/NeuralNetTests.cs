using System.Collections.Generic;
using System.Linq;
using MattEland.AI.Neural;
using Newtonsoft.Json;
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

        [Fact]
        public void NeuralNetsShouldSerialize()
        {
            // Arrange
            var net = new NeuralNet(2, 2);
            net.AddHiddenLayer(3);
            
            // Act
            net.Evaluate(new List<decimal> {1, -1});

            // Assert
            var json = JsonConvert.SerializeObject(net);
            json.ShouldNotBeNullOrWhiteSpace();
        }
        
        [Fact]
        public void NeuralNetWeightsShouldFunction()
        {
            // Arrange
            var net = new NeuralNet(1, 1);
            net.AddHiddenLayer(3);
            net.SetWeights(new List<decimal> {-1, 0.5m, -0.5m, 0.5m, 1m, -0.15m});
            
            // Act
            var output = net.Evaluate(new List<decimal> {0.00625m});

            // Assert
            output.Single().ShouldBe(1);
        }
    }
}
