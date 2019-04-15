using MattEland.Shared.Numerics;
using Shouldly;
using Xunit;

namespace MattEland.Shared.Tests
{
    public class NumberTests
    {
        [Theory]
        [InlineData(7, 7)]
        [InlineData(4.2, 4)]
        [InlineData(3.1459, 3)]
        [InlineData(0, 0)]
        [InlineData(-1.5, -1)]
        [InlineData(9.999999999, 9)]
        public void DecimalFloorShouldProduceACorrectInteger(decimal value, int expected)
        {
            // Act
            int actual = value.ToInt();

            // Assert
            actual.ShouldBe(expected);
        }
    }
}