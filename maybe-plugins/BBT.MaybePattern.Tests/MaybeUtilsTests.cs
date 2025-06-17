using System;
using System.Runtime.Serialization;
using BBT.MaybePattern.Tests.TestData;
using Shouldly;
using Xunit;

namespace BBT.MaybePattern.Tests
{
    public sealed class MaybeUtilsTests
    {
        public sealed class TheCheckArgumentNotNullMethod
        {
            [Fact]
            public void Should_Set_Return_Argument_If_Argument_Is_Not_Null()
            {
                // Arrange
                var baseClass = new BaseClass();

                // Act
                var value = MaybeUtils.CheckArgumentNotNull(baseClass, nameof(baseClass));

                //Assert
                value.ShouldBe(baseClass);
            }

            [Fact]
            public void Should_Throw_ArgumentNullException_If_Argument_Is_Null()
            {
                // Arrange
                var argumentName = "argumentName";

                // Act
                var exception = Record.Exception(() => MaybeUtils.CheckArgumentNotNull<BaseClass>(null, argumentName));

                // Assert
                exception.ShouldBeOfType<ArgumentNullException>();
                exception.Message.ShouldContain(argumentName);
            }
        }

        public sealed class TheCheckParameterNotNullMethod
        {
            [Fact]
            public void Should_Return_Parameter_If_Parameter_Is_Not_Null()
            {
                // Arrange
                var baseClass = new BaseClass();

                // Act
                var value = MaybeUtils.CheckParameterNotNull(baseClass, nameof(baseClass));

                //Assert
                value.ShouldBe(baseClass);
            }

            [Fact]
            public void Should_Throw_InvalidOperationException_If_Parameter_Is_Null()
            {
                // Arrange
                var parameterName = "parameterName";

                // Act
                var exception = Record.Exception(() => MaybeUtils.CheckParameterNotNull<BaseClass>(null, parameterName));

                // Assert
                exception.ShouldBeOfType<InvalidOperationException>();
                exception.Message.ShouldContain(parameterName);
            }

            [Fact]
            public void Should_Throw_InvalidOperationException_With_Additional_Message_If_Parameter_Is_Null()
            {
                // Arrange
                var parameterName = "parameterName";
                var additionalMsg = "additionalMsg";

                // Act
                var exception = Record.Exception(() => MaybeUtils.CheckParameterNotNull<BaseClass>(null, parameterName, additionalMsg));

                // Assert
                exception.ShouldBeOfType<InvalidOperationException>();
                exception.Message.ShouldContain(parameterName);
                exception.Message.ShouldContain(additionalMsg);
            }
        }

        public sealed class TheGetDeserializedValueMethod
        {
            [Fact]
            public void Should_Return_Value_If_SerializationInfo_Contains_Value()
            {
                // Arrange
                var baseClass = new BaseClass();
                var key = "key";
                var serializationInfo = new SerializationInfo(typeof(Maybe<>), new FormatterConverter());
                serializationInfo.AddValue(key, baseClass, typeof(BaseClass));

                // Act
                var value = MaybeUtils.GetDeserializedValue<BaseClass>(serializationInfo, key);

                //Assert
                value.ShouldBe(baseClass);
            }

            [Fact]
            public void Should_Throw_ArgumentNullException_If_SerializationInfo_Is_Null()
            {
                // Arrange & Act
                var exception = Record.Exception(() => MaybeUtils.GetDeserializedValue<BaseClass>(null, "key"));

                // Assert
                exception.ShouldBeOfType<ArgumentNullException>();
            }
        }

        public sealed class TheSetDataMethod
        {
            [Fact]
            public void Should_Throw_ArgumentNullException_If_SerializationInfo_Is_Null()
            {
                // Arrange & Act
                var exception = Record.Exception(() => MaybeUtils.SetData<BaseClass>(null, "key", new BaseClass()));

                // Assert
                exception.ShouldBeOfType<ArgumentNullException>();
            }

            [Fact]
            public void Should_Add_Value_To_SerializationInfo()
            {
                // Arrange
                var baseClass = new BaseClass();
                var key = "key";
                var serializationInfo = new SerializationInfo(typeof(BaseClass), new FormatterConverter());

                // Act
                MaybeUtils.SetData<BaseClass>(serializationInfo, key, baseClass);

                // Assert
                var value = serializationInfo.GetValue(key, typeof(BaseClass));
                value.ShouldBe(baseClass);
            }
        }
    }
}
