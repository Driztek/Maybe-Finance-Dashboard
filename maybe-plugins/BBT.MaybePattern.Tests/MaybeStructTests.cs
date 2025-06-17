namespace BBT.MaybePattern.Tests
{
    using System;
    using System.Runtime.Serialization;
    using BBT.MaybePattern;
    using BBT.MaybePattern.Tests.TestData;
    using Shouldly;
    using Xunit;

    public sealed class MaybeStructTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Value_If_Argument_Is_Not_Null()
            {
                // Arrange
                var baseStruct = new BaseStruct();
                var maybe = new MaybeStruct<BaseStruct>(baseStruct);

                // Act & Assert
                maybe.HasValue.ShouldBeTrue();
                var value = maybe.ValueOrException(nameof(maybe));
                value.ShouldBe(baseStruct);
            }

            [Fact]
            public void Should_Set_Value_To_Null_If_Argument_Is_Null()
            {
                // Arrange
                var maybe = new MaybeStruct<BaseStruct>(null);

                // Act & Assert
                maybe.HasValue.ShouldBeFalse();
            }

            [Fact]
            public void Should_Throw_ArgumentNullException_If_SerializationInfo_Is_Null()
            {
                // Arrange & Act
                var exception = Record.Exception(() => new MaybeStruct<BaseStruct>(null, default(StreamingContext)));

                // Assert
                exception.ShouldBeOfType<ArgumentNullException>();
            }

            [Fact]
            public void Should_Set_Value_If_SerializationInfo_Contains_Value()
            {
                // Arrange
                var baseStruct = new BaseStruct();
                var serializationInfo = new SerializationInfo(typeof(MaybeStruct<>), new FormatterConverter());
                serializationInfo.AddValue("value", baseStruct, typeof(BaseStruct));

                // Act
                var maybe = new MaybeStruct<BaseStruct>(serializationInfo, default(StreamingContext));

                // Assert
                maybe.HasValue.ShouldBeTrue();
                var value = maybe.ValueOrException(nameof(maybe));
                value.ShouldBe(baseStruct);
            }
        }

        public sealed class TheHasValueMethod
        {
            [Fact]
            public void Should_Return_True_If_Called_For_Some_Maybe()
            {
                // Arrange
                var someMaybe = Maybe.SomeStruct<BaseStruct>(default(BaseStruct));

                // Act & Assert
                someMaybe.HasValue.ShouldBeTrue();
            }

            [Fact]
            public void Should_Return_False_If_Called_For_None_Maybe()
            {
                // Arrange
                var someMaybe = Maybe.NoneStruct<BaseStruct>();

                // Act & Assert
                someMaybe.HasValue.ShouldBeFalse();
            }
        }

        public sealed class TheSomeMethod
        {
            [Fact]
            public void Called_With_Projection_Func_To_Not_Null_Referencing_Class_Should_Return_Some_Maybe_Of_Type_Referencing_Class()
            {
                // Arrange
                var referencedStruct = default(ReferencedStruct);
                var referencingClass = new BaseClass();
                referencedStruct.ReferencingClass = referencingClass;

                // Act
                var maybeReferencingClass = Maybe.SomeStruct<ReferencedStruct>(referencedStruct)
                    .Some(x => x.ReferencingClass);

                // Assert
                maybeReferencingClass.ShouldBe(Maybe.Some<BaseClass>(referencingClass));
            }

            [Fact]
            public void Called_With_Projection_Func_To_Null_Referencing_Class_Should_Return_None_Maybe_Of_Type_Referencing_Class()
            {
                // Arrange
                var maybeReferencedStruct = Maybe.NoneStruct<ReferencedStruct>();

                // Act
                var maybeReferencingClass = maybeReferencedStruct.Some(x => x.ReferencingClass);

                // Assert
                maybeReferencingClass.ShouldBe(Maybe.None<BaseClass>());
            }

            [Fact]
            public void Called_With_Projection_Func_To_Maybe_Should_Return_Some_Maybe_Of_Type_Maybe()
            {
                // Arrange
                var referencedStruct = default(ReferencedStruct);
                var referencingClass = new BaseClass();
                referencedStruct.ReferencingClass = referencingClass;

                // Act
                var maybeReferencingStruct = Maybe.SomeStruct<ReferencedStruct>(referencedStruct)
                    .Some(x => Maybe.Some(x.ReferencingClass));

                // Assert
                maybeReferencingStruct.ShouldBe(Maybe.Some<BaseClass>(referencingClass));
            }
        }

        public sealed class TheSomeStructMethod
        {
            [Fact]
            public void Called_With_Projection_Func_To_Not_Null_Referencing_Struct_Should_Return_Some_Maybe_Of_Type_Referencing_Struct()
            {
                // Arrange
                var referencedStruct = default(ReferencedStruct);
                var referencingStruct = default(BaseStruct);
                referencedStruct.ReferencingStruct = referencingStruct;

                // Act
                var maybeReferencingStruct = Maybe.SomeStruct<ReferencedStruct>(referencedStruct)
                    .SomeStruct<BaseStruct>(x => x.ReferencingStruct);

                // Assert
                maybeReferencingStruct.ShouldBe(Maybe.SomeStruct<BaseStruct>(referencingStruct));
            }

            [Fact]
            public void Called_With_Projection_Func_To_Null_Referencing_Struct_Should_Return_None_Maybe_Of_Type_Referencing_Struct()
            {
                // Arrange
                var maybeReferencedStruct = Maybe.NoneStruct<ReferencedStruct>();

                // Act
                var maybeReferencingStruct = maybeReferencedStruct.SomeStruct<BaseStruct>(x => x.ReferencingStruct);

                // Assert
                maybeReferencingStruct.ShouldBe(Maybe.NoneStruct<BaseStruct>());
            }

            [Fact]
            public void Called_With_Projection_Func_To_MaybeStruct_Should_Return_Some_Maybe_Of_Type_MaybeStruct()
            {
                // Arrange
                var referencedStruct = default(ReferencedStruct);
                var referencingStruct = default(BaseStruct);
                referencedStruct.ReferencingStruct = referencingStruct;

                // Act
                var maybeReferencingStruct = Maybe.SomeStruct<ReferencedStruct>(referencedStruct)
                    .SomeStruct<BaseStruct>(x => Maybe.SomeStruct<BaseStruct>(x.ReferencingStruct));

                // Assert
                maybeReferencingStruct.ShouldBe(Maybe.SomeStruct<BaseStruct>(referencingStruct));
            }
        }

        public sealed class TheDoMethod
        {
            [Fact]
            public void Should_Not_Call_Function_If_Maybe_Is_None()
            {
                // Arrange
                var maybeNone = Maybe.NoneStruct<BaseStruct>();
                var called = false;

                // Act
                maybeNone.Do(x => called = true);

                // Assert
                called.ShouldBeFalse();
            }

            /// <summary>
            /// Test that <see cref="Maybe{T}.Do"/> does call delegate if maybe represents not-null case.
            /// </summary>
            [Fact]
            public void Should_Call_Function_If_Maybe_Is_Some()
            {
                // Arrange
                var maybeNone = Maybe.SomeStruct<BaseStruct>(default(BaseStruct));
                var called = false;

                // Act
                maybeNone.Do(x => called = true);

                // Assert
                called.ShouldBeTrue();
            }
        }

        public sealed class TheDoIfNoneMethod
        {
            [Fact]
            public void Should_Call_Action_If_Maybe_Is_None()
            {
                // Arrange
                var maybeNone = Maybe.NoneStruct<BaseStruct>();
                var called = false;

                // Act
                maybeNone.Do(x => { }).DoIfNone(() => called = true);

                // Assert
                called.ShouldBeTrue();
            }

            [Fact]
            public void Should_Not_Call_Action_If_Maybe_Is_Some()
            {
                // Arrange
                var maybeNone = Maybe.SomeStruct<BaseStruct>(default(BaseStruct));
                var called = false;

                // Act
                maybeNone.Do(x => { }).DoIfNone(() => called = true);

                // Assert
                called.ShouldBeFalse();
            }
        }

        public sealed class TheEqualsMethod
        {
            [Fact]
            public void Should_Return_True_If_Both_Are_None_Mabye_And_Of_Same_Type()
            {
                // Arrange
                var maybeNone = Maybe.NoneStruct<BaseStruct>();
                var maybeNone2 = Maybe.NoneStruct<BaseStruct>();

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void Should_Return_True_If_Both_Mabye_Are_Of_Same_Type_Represent_Equal_Struct()
            {
                // Arrange
                var baseStruct = default(BaseStruct);
                var maybeNone = Maybe.SomeStruct<BaseStruct>(baseStruct);
                var maybeNone2 = Maybe.SomeStruct<BaseStruct>(baseStruct);

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void Should_Return_False_If_One_Is_Some_And_Other_Is_None_Maybe()
            {
                // Arrange
                var baseStruct = default(BaseStruct);
                var maybeNone = Maybe.SomeStruct<BaseStruct>(baseStruct);
                var maybeNone2 = Maybe.NoneStruct<BaseStruct>();

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_False_If_Input_Is_Not_Of_Same_Type()
            {
                // Arrange
                var maybe = Maybe.NoneStruct<BaseStruct>();
                var otherType = new ReferencedStruct();

                // Act
                var isEqual = maybe.Equals(otherType);

                // Assert
                isEqual.ShouldBeFalse();
            }
        }

        public sealed class TheValueOrDefaultMethod
        {
            [Fact]
            public void Should_Return_Value_If_Called_For_None_Mabye()
            {
                // Arrange
                var maybe = Maybe.NoneStruct<BaseStruct>();
                var baseStruct = default(BaseStruct);

                // Act
                var result = maybe.ValueOrDefault(baseStruct);

                // Assert
                result.ShouldBe(baseStruct);
            }

            [Fact]
            public void Should_Return_Value_If_Called_For_Some_Mabye()
            {
                // Arrange
                var baseStruct = default(BaseStruct);
                var maybe = Maybe.SomeStruct<BaseStruct>(baseStruct);

                // Act
                var result = maybe.ValueOrDefault(new BaseStruct("other"));

                // Assert
                result.ShouldBe(baseStruct);
            }
        }

        public sealed class TheValueOrExceptionMethod
        {
            [Fact]
            public void Should_Throw_InvalidOperationException_With_TypeName_If_Called_Without_Args_For_None_Mabye()
            {
                // Arrange
                var maybeNone = Maybe.NoneStruct<BaseStruct>();

                // Act
                var exception = Record.Exception(() => maybeNone.ValueOrException());

                // Assert
                exception.ShouldBeOfType<InvalidOperationException>();
                exception.Message.ShouldContain(nameof(BaseStruct));
            }

            [Fact]
            public void Should_Throw_InvalidOperationException_If_Called_With_Args_For_None_Mabye()
            {
                // Arrange
                var maybeNone = Maybe.NoneStruct<BaseStruct>();

                // Act
                var exception = Record.Exception(() => maybeNone.ValueOrException(nameof(maybeNone)));

                // Assert
                exception.ShouldBeOfType<InvalidOperationException>();
                exception.Message.ShouldContain(nameof(maybeNone));
            }

            [Fact]
            public void Should_Return_Value_If_Called_For_Some_Mabye()
            {
                // Arrange
                var baseStruct = default(BaseStruct);
                var maybeNone = Maybe.SomeStruct<BaseStruct>(baseStruct);

                // Act
                var result = maybeNone.ValueOrException(nameof(maybeNone));

                // Assert
                result.ShouldBe(baseStruct);
            }
        }

        public sealed class TheNotEqualOperator
        {
            [Fact]
            public void Should_Return_False_If_Both_Are_None_Mabye_And_Of_Same_Type()
            {
                // Arrange
                var maybeNone = Maybe.NoneStruct<BaseStruct>();
                var maybeNone2 = Maybe.NoneStruct<BaseStruct>();

                // Act
                var isNotEqual = maybeNone != maybeNone2;

                // Assert
                isNotEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_False_If_Both_Are_Some_Mabye_And_Of_Same_Type()
            {
                // Arrange
                var baseStruct = default(BaseStruct);
                var maybeNone = Maybe.SomeStruct<BaseStruct>(baseStruct);
                var maybeNone2 = Maybe.SomeStruct<BaseStruct>(baseStruct);

                // Act
                var isNotEqual = maybeNone != maybeNone2;

                // Assert
                isNotEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_True_If_Mabyes_Are_Of_Same_Type_But_Represent_Different_Values()
            {
                // Arrange
                var baseStruct = default(BaseStruct);
                var baseStruct2 = new BaseStruct("other");
                var maybeNone = Maybe.SomeStruct<BaseStruct>(baseStruct);
                var maybeNone2 = Maybe.SomeStruct<BaseStruct>(baseStruct2);

                // Act
                var isNotEqual = maybeNone != maybeNone2;

                // Assert
                isNotEqual.ShouldBeTrue();
            }

            [Fact]
            public void Should_Return_True_If_One_Is_Some_And_Other_Is_None_Maybe()
            {
                // Arrange
                var baseStruct = default(BaseStruct);
                var maybeNone = Maybe.SomeStruct<BaseStruct>(baseStruct);
                var maybeNone2 = Maybe.NoneStruct<BaseStruct>();

                // Act
                var isNotEqual = maybeNone != maybeNone2;

                // Assert
                isNotEqual.ShouldBeTrue();
            }
        }

        public sealed class TheImplicitOperator
        {
            [Fact]
            public void Should_Return_Some_MaybeStruct_If_Assigned_With_Some_Struct()
            {
                // Arrange
                var someStruct = new BaseStruct();

                // Act
                MaybeStruct<BaseStruct> maybeSomeStruct = someStruct;

                // Assert
                maybeSomeStruct.ShouldBeOfType<MaybeStruct<BaseStruct>>();
                maybeSomeStruct.HasValue.ShouldBeTrue();
                maybeSomeStruct.ValueOrException().ShouldBe(someStruct);
            }

            [Fact]
            public void Should_Return_None_Maybe_If_Assigned_With_Null()
            {
                // Act
                MaybeStruct<BaseStruct> maybeNone = null;

                // Assert
                maybeNone.ShouldBeOfType<MaybeStruct<BaseStruct>>();
                maybeNone.HasValue.ShouldBeFalse();
                maybeNone.ShouldBe(Maybe.NoneStruct<BaseStruct>());
            }
        }

        public sealed class TheGetHashCodeMethod
        {
            [Fact]
            public void Should_Return_HashCode_Of_Value_If_Some()
            {
                // Arrange
                var baseStruct = new BaseStruct();
                var maybe = Maybe.SomeStruct<BaseStruct>(baseStruct);

                // Act
                var hashCode = maybe.GetHashCode();

                // Assert
                hashCode.ShouldBe(baseStruct.GetHashCode());
            }

            [Fact]
            public void Should_Return_HashCode_Of_Base_If_None()
            {
                // Arrange
                var maybe = Maybe.NoneStruct<BaseStruct>();

                // Act
                var hashCode = maybe.GetHashCode();

                // Assert
                hashCode.ShouldBe(((object)maybe).GetHashCode());
            }
        }

        public sealed class TheGetObjectDataMethod
        {
            [Fact]
            public void Should_Throw_ArgumentNullException_If_SerializationInfo_Is_Null()
            {
                // Arrange
                var maybe = Maybe.NoneStruct<BaseStruct>();

                // Act
                var exception = Record.Exception(() => maybe.GetObjectData(null, default(StreamingContext)));

                // Assert
                exception.ShouldBeOfType<ArgumentNullException>();
            }

            [Fact]
            public void Should_Add_Value_To_SerializationInfo()
            {
                // Arrange
                var baseStruct = new BaseStruct();
                var maybe = Maybe.SomeStruct<BaseStruct>(baseStruct);
                var serializationInfo = new SerializationInfo(typeof(Maybe<>), new FormatterConverter());

                // Act
                maybe.GetObjectData(serializationInfo, default(StreamingContext));

                // Assert
                var value = serializationInfo.GetValue("value", typeof(BaseStruct));
                value.ShouldBe(baseStruct);
            }
        }

        public sealed class TheToStringMethod
        {
            [Fact]
            public void Should_Return_String_Representation_Of_Value_If_Some()
            {
                // Arrange
                var baseStruct = new BaseStruct();
                var maybe = Maybe.SomeStruct<BaseStruct>(baseStruct);

                // Act
                var stringRepresentation = maybe.ToString();

                // Assert
                stringRepresentation.ShouldBe(baseStruct.ToString());
            }

            [Fact]
            public void Should_Return_Emptry_String_If_None()
            {
                // Arrange
                var maybe = Maybe.NoneStruct<BaseStruct>();

                // Act
                var stringRepresentation = maybe.ToString();

                // Assert
                stringRepresentation.ShouldBe(string.Empty);
            }
        }
    }
}
