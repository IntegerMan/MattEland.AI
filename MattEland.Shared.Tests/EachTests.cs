using System;
using System.Collections.Generic;
using MattEland.Shared.Collections;
using Shouldly;
using Xunit;

namespace MattEland.Shared.Tests
{
    /// <summary>
    /// Tests related to the Each series of extension methods
    /// </summary>
    public class EachTests
    {
        [Fact]
        public void EachShouldReturnTheSourceCollection()
        {
            // Arrange
            var collection = new List<string> {"Foo", "Bar"};

            // Act
            var result = collection.Each(c => { });

            // Assert
            result.ShouldBe(collection);
        }

        [Fact]
        public void EachShouldOperateOnEachItemIntheCollection()
        {
            // Arrange
            int sum = 0;
            var collection = new List<int> { 1, 2, 3};

            // Act
            collection.Each(i => sum += i);

            // Assert
            sum.ShouldBe(6);
        }

        [Fact]
        public void EachShouldNotErrorOnNullCollections()
        {
            // Arrange
            List<int> collection = null;

            // Act
            collection.Each(i => { });

            // Assert - if we got here, it means no exception occurred so we pass
        }        

        [Fact]
        public void EachShouldNotErrorOnEmptyCollections()
        {
            // Arrange
            var collection = new List<int>();

            // Act
            collection.Each(i => { });

            // Assert - if we got here, it means no exception occurred so we pass
        }        
        
        [Fact]
        public void EachShouldThrowExceptionIfNoOperation()
        {
            // Arrange
            var collection = new List<int> { 1 };

            // Act / Assert
            Should.Throw<ArgumentNullException>(() => collection.Each(null));
        }

        [Fact]
        public void EachIntShouldRepeatTheExpectedAmountOfTimes()
        {
            // Arrange
            int num = 4;
            int sum = 0;

            // Act
            num.Each(i => sum += i);

            // Assert
            sum.ShouldBe(6); // 0 + 1 + 2 + 3
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-500)]
        [InlineData(int.MinValue)]
        public void EachIntShouldNotInvokeForBadValues(int value)
        {
            // Act / Assert
            value.Each(i => throw new ShouldAssertException($"Method should not have been invoked but was with {i}"));
        }

        [Fact]
        public void EachDictionaryShouldAllowNullDictionaries()
        {
            // Arrange
            IDictionary<int, int> dict = null;

            // Act / Assert
            dict.Each((key, value) => throw new ShouldAssertException($"Method should not have been invoked but was with key:{key}"));
        }

        [Fact]
        public void EachDictionaryShouldAllowEmptyDictionaries()
        {
            // Arrange
            IDictionary<int, int> dict = new Dictionary<int, int>();

            // Act / Assert
            dict.Each((key, value) => throw new ShouldAssertException($"Method should not have been invoked but was with key:{key}"));
        }

        [Fact]
        public void EachDictionaryIncludeEachKey()
        {
            // Arrange
            IDictionary<int, int> dict = new Dictionary<int, int>
            {
                { 22, 1 },
                { 20, 1 }
            };
            int keySum = 0;

            // Act
            dict.Each((key, value) => keySum += key);

            // Assert
            keySum.ShouldBe(42);
        }

        [Fact]
        public void EachDictionaryIncludesCorrectValues()
        {
            // Arrange
            IDictionary<int, int> dict = new Dictionary<int, int>
            {
                { 22, 1 },
                { 20, 5 }
            };

            // Act / Assert
            dict.Each((key, value) => dict[key].ShouldBe(value));
        }

        [Fact]
        public void EachDictionaryThrowsOnNoOperation()
        {
            // Arrange
            IDictionary<int, int> dict = new Dictionary<int, int>();

            // Act / Assert
            Should.Throw<ArgumentNullException>(() => dict.Each(null));
        }
    }
}
