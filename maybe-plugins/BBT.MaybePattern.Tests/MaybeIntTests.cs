namespace BBT.MaybePattern.Tests
{
    using BBT.MaybePattern;
    using BBT.MaybePattern.Tests.TestData;
    using Shouldly;
    using Xunit;

    public sealed class MaybeIntTests
    {
        public sealed class SerializeAndDeserialize
        {
            /// <summary>
            /// Serialize and deserialize a maybe of int.
            /// </summary>
            [Fact]
            public void Should_Work_For_None()
            {
                // Arrange
                var maybe = Maybe.None<object>();
                var testData = new TestDataClass() { Maybe = maybe };

                // Act & Assert
                using (var stream = TestUtils.SerializeToStream(testData))
                {
                    var testDataDeserialized = (TestDataClass)TestUtils.DeserializeFromStream(stream);

                    testDataDeserialized.Maybe.ShouldBeOfType<Maybe<object>>();
                    testDataDeserialized.Maybe.HasValue.ShouldBeFalse();
                }
            }

            /// <summary>
            /// Serialize and deserialize a maybe of int.
            /// </summary>
            [Fact]
            public void Should_Work_For_Some()
            {
                // Arrange
                var maybe = Maybe.Some<object>(new object());
                var testData = new TestDataClass() { Maybe = maybe };

                // Act & Assert
                using (var stream = TestUtils.SerializeToStream(testData))
                {
                    var testDataDeserialized = (TestDataClass)TestUtils.DeserializeFromStream(stream);

                    testDataDeserialized.Maybe.ShouldBeOfType<Maybe<object>>();
                    testDataDeserialized.Maybe.HasValue.ShouldBeTrue();
                }
            }
        }
    }
}
