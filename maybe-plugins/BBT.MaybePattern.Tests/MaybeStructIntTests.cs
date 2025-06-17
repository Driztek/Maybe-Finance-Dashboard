namespace BBT.MaybePattern.Tests
{
    using BBT.MaybePattern;
    using BBT.MaybePattern.Tests.TestData;
    using Shouldly;
    using Xunit;

    public sealed class MaybeStructIntTests
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
                var maybeStruct = Maybe.NoneStruct<int>();
                var testData = new TestDataStruct() { MaybeStruct = maybeStruct };

                // Act & Assert
                using (var stream = TestUtils.SerializeToStream(testData))
                {
                    var testDataDeserialized = (TestDataStruct)TestUtils.DeserializeFromStream(stream);

                    testDataDeserialized.MaybeStruct.ShouldBeOfType<MaybeStruct<int>>();
                    testDataDeserialized.MaybeStruct.HasValue.ShouldBeFalse();
                }
            }

            /// <summary>
            /// Serialize and deserialize a maybe of int.
            /// </summary>
            [Fact]
            public void Should_Work_For_Some()
            {
                // Arrange
                var maybeStruct = Maybe.SomeStruct<int>(5);
                var testData = new TestDataStruct() { MaybeStruct = maybeStruct };

                // Act & Assert
                using (var stream = TestUtils.SerializeToStream(testData))
                {
                    var testDataDeserialized = (TestDataStruct)TestUtils.DeserializeFromStream(stream);

                    testDataDeserialized.MaybeStruct.ShouldBeOfType<MaybeStruct<int>>();
                    testDataDeserialized.MaybeStruct.HasValue.ShouldBeTrue();
                }
            }
        }
    }
}
