namespace BBT.MaybePattern.Tests
{
    using System;
    using System.Runtime.Serialization;
    using BBT.MaybePattern;
    using BBT.MaybePattern.Tests.TestData;
    using Shouldly;
    using Xunit;

    public sealed class MaybeTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Value_If_Argument_Is_Not_Null()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybe = new Maybe<BaseClass>(baseClass);

                // Act & Assert
                maybe.HasValue.ShouldBeTrue();
                var value = maybe.ValueOrException(nameof(maybe));
                value.ShouldBe(baseClass);
            }

            [Fact]
            public void Should_Set_Value_To_Null_If_Argument_Is_Null()
            {
                // Arrange
                var maybe = new Maybe<BaseClass>(null);

                // Act & Assert
                maybe.HasValue.ShouldBeFalse();
            }

            [Fact]
            public void Should_Throw_ArgumentNullException_If_SerializationInfo_Is_Null()
            {
                // Arrange & Act
                var exception = Record.Exception(() => new Maybe<BaseClass>(null, default(StreamingContext)));

                // Assert
                exception.ShouldBeOfType<ArgumentNullException>();
            }

            [Fact]
            public void Should_Set_Value_If_SerializationInfo_Contains_Value()
            {
                // Arrange
                var baseClass = new BaseClass();
                var serializationInfo = new SerializationInfo(typeof(Maybe<>), new FormatterConverter());
                serializationInfo.AddValue("value", baseClass, typeof(BaseClass)); 

                // Act
                var maybe = new Maybe<BaseClass>(serializationInfo, default(StreamingContext));

                // Assert
                maybe.HasValue.ShouldBeTrue();
                var value = maybe.ValueOrException(nameof(maybe));
                value.ShouldBe(baseClass);
            }
        }

        public sealed class TheHasValueMethod
        {
            [Fact]
            public void Should_Return_True_If_Called_For_Some_Maybe()
            {
                // Arrange
                var someMaybe = Maybe.Some(new BaseClass());

                // Act & Assert
                someMaybe.HasValue.ShouldBeTrue();
            }

            [Fact]
            public void Should_Return_False_If_Called_For_None_Maybe()
            {
                // Arrange
                var someMaybe = Maybe.None<BaseClass>();

                // Act & Assert
                someMaybe.HasValue.ShouldBeFalse();
            }
        }

        public sealed class TheSomeMethod
        {
            [Fact]
            public void Called_With_Projection_Func_To_Class_Of_Base_Type_Should_Return_Some_Maybe_Of_Type_Base()
            {
                // Arrange
                var derivedClass = new DerivedClass();
                var maybeDerivedClass = Maybe.Some(derivedClass);

                // Act
                var maybeBaseClass = maybeDerivedClass.Some<BaseClass>(x => x);

                // Assert
                maybeBaseClass.ShouldBe(Maybe.Some<BaseClass>(derivedClass));
            }

            [Fact]
            public void Called_With_Projection_Func_To_Not_Null_Referencing_Class_Should_Return_Some_Maybe_Of_Type_Referencing_Class()
            {
                // Arrange
                var referencedClass = new ReferencedClass();
                var referencingClass = new BaseClass();
                referencedClass.ReferencingClass = referencingClass;

                // Act
                var maybeReferencingClass = Maybe.Some(referencedClass).Some(x => x.ReferencingClass);

                // Assert
                maybeReferencingClass.ShouldBe(Maybe.Some<BaseClass>(referencingClass));
            }

            [Fact]
            public void Called_With_Projection_Func_To_Null_Referencing_Class_Should_Return_None_Maybe_Of_Type_Referencing_Class()
            {
                // Arrange
                var maybeReferencedClass = Maybe.None<ReferencedClass>();

                // Act
                var maybeReferencingClass = maybeReferencedClass.Some(x => x.ReferencingClass);

                // Assert
                maybeReferencingClass.ShouldBe(Maybe.None<BaseClass>());
            }

            [Fact]
            public void Called_With_Projection_Func_To_Maybe_Type_Should_Return_Maybe_Of_Type_Maybe_Type()
            {
                // Arrange
                var referencedClass = new ReferencedClass();
                var referencingClass = new BaseClass();
                referencedClass.ReferencingClass = referencingClass;

                // Act
                var maybeReferencingClass = Maybe.Some(referencedClass).Some(x => Maybe.Some(x.ReferencingClass));

                // Assert
                maybeReferencingClass.ShouldBe(Maybe.Some<BaseClass>(referencingClass));
            }
        }

        public sealed class TheSomeStructMethod
        {
            [Fact]
            public void Called_With_Projection_Func_To_Not_Null_Referencing_Struct_Should_Return_Some_Maybe_Of_Type_Referencing_Struct()
            {
                // Arrange
                var referencedClass = new ReferencedClass();
                var referencingStruct = default(BaseStruct);
                referencedClass.ReferencingStruct = referencingStruct;

                // Act
                var maybeReferencingStruct = Maybe.Some(referencedClass)
                    .SomeStruct<BaseStruct>(x => x.ReferencingStruct);

                // Assert
                maybeReferencingStruct.ShouldBe(Maybe.SomeStruct<BaseStruct>(referencingStruct));
            }

            [Fact]
            public void Called_With_Projection_Func_To_Null_Referencing_Class_Should_Return_None_Maybe_Of_Type_Referencing_Class()
            {
                // Arrange
                var maybeReferencedClass = Maybe.None<ReferencedClass>();

                // Act
                var maybeReferencingStruct = maybeReferencedClass.SomeStruct<BaseStruct>(x => x.ReferencingStruct);

                // Assert
                maybeReferencingStruct.ShouldBe(Maybe.NoneStruct<BaseStruct>());
            }

            [Fact]
            public void Called_With_Projection_Func_To_Maybe_Type_Should_Return_Maybe_Of_Type_Maybe_Type()
            {
                // Arrange
                var referencedClass = new ReferencedClass();
                var referencingStruct = default(BaseStruct);
                referencedClass.ReferencingStruct = referencingStruct;

                // Act
                var maybeReferencingStruct = Maybe.Some(referencedClass).SomeStruct(x => Maybe.SomeStruct<BaseStruct>(x.ReferencingStruct));

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
                var maybeNone = Maybe.None<BaseClass>();
                var called = false;

                // Act
                maybeNone.Do(x => called = true);

                // Assert
                called.ShouldBeFalse();
            }

            [Fact]
            public void Should_Call_Function_If_Maybe_Is_Some()
            {
                // Arrange
                var maybeNone = Maybe.Some<BaseClass>(new BaseClass());
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
                var maybeNone = Maybe.None<BaseClass>();
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
                var maybeNone = Maybe.Some<BaseClass>(new BaseClass());
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
                var maybeNone = Maybe.None<BaseClass>();
                var maybeNone2 = Maybe.None<BaseClass>();

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void Should_Return_False_If_Both_Are_None_Mabye_But_Of_Base_And_Derived_Type()
            {
                // Arrange
                var maybeNone = Maybe.None<BaseClass>();
                var maybeNone2 = Maybe.None<DerivedClass>();

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_True_If_Both_Are_Some_Mabye_And_Of_Same_Type()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybeNone = Maybe.Some(baseClass);
                var maybeNone2 = Maybe.Some(baseClass);

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void Should_Return_False_If_Both_Represent_Equal_Object_But_Mabyes_Are_Of_Base_And_Derived_Type()
            {
                // Arrange
                var derivedClass = new DerivedClass();
                var maybeNone = Maybe.Some<DerivedClass>(derivedClass);
                var maybeNone2 = Maybe.Some<BaseClass>(derivedClass);

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_False_If_Mabyes_Are_Of_Same_Type_But_Represent_Different_Objects()
            {
                // Arrange
                var baseClass = new BaseClass();
                var baseClass2 = new BaseClass();
                var maybeNone = Maybe.Some(baseClass);
                var maybeNone2 = Maybe.Some(baseClass2);

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

                // Assert
                isEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_False_If_One_Is_Some_And_Other_Is_None_Maybe()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybeNone = Maybe.Some<BaseClass>(baseClass);
                var maybeNone2 = Maybe.None<BaseClass>();

                // Act
                var isEqual = object.Equals(maybeNone, maybeNone2);

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
                var maybe = Maybe.None<BaseClass>();
                var baseClass = new BaseClass();

                // Act
                var result = maybe.ValueOrDefault(baseClass);

                // Assert
                result.ShouldBe(baseClass);
            }

            [Fact]
            public void Should_Return_Value_If_Called_For_Some_Mabye()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybe = Maybe.Some(baseClass);

                // Act
                var result = maybe.ValueOrDefault(new BaseClass());

                // Assert
                result.ShouldBe(baseClass);
            }
        }

        public sealed class TheValueOrExceptionMethod
        {
            [Fact]
            public void Should_Throw_InvalidOperationException_With_TypeName_If_Called_Without_Args_For_None_Mabye()
            {
                // Arrange
                var maybeNone = Maybe.None<BaseClass>();

                // Act
                var exception = Record.Exception(() => maybeNone.ValueOrException());

                // Assert
                exception.ShouldBeOfType<InvalidOperationException>();
                exception.Message.ShouldContain(nameof(BaseClass));
            }

            [Fact]
            public void Should_Throw_InvalidOperationException_If_Called_With_Args_For_None_Mabye()
            {
                // Arrange
                var maybeNone = Maybe.None<BaseClass>();

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
                var baseClass = new BaseClass();
                var maybeNone = Maybe.Some<BaseClass>(baseClass);

                // Act
                var result = maybeNone.ValueOrException(nameof(maybeNone));

                // Assert
                result.ShouldBe(baseClass);
            }
        }

        public sealed class TheNotEqualOperator
        {
            [Fact]
            public void Should_Return_False_If_Both_Are_None_Mabye_And_Of_Same_Type()
            {
                // Arrange
                var maybeNone = Maybe.None<BaseClass>();
                var maybeNone2 = Maybe.None<BaseClass>();

                // Act
                var isNotEqual = maybeNone != maybeNone2;

                // Assert
                isNotEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_False_If_Both_Are_Some_Mabye_And_Of_Same_Type()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybeNone = Maybe.Some(baseClass);
                var maybeNone2 = Maybe.Some(baseClass);

                // Act
                var isNotEqual = maybeNone != maybeNone2;

                // Assert
                isNotEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_True_If_Mabyes_Are_Of_Same_Type_But_Represent_Different_Objects()
            {
                // Arrange
                var baseClass = new BaseClass();
                var baseClass2 = new BaseClass();
                var maybeNone = Maybe.Some(baseClass);
                var maybeNone2 = Maybe.Some(baseClass2);

                // Act
                var isNotEqual = maybeNone != maybeNone2;

                // Assert
                isNotEqual.ShouldBeTrue();
            }

            [Fact]
            public void Should_Return_True_If_One_Is_Some_And_Other_Is_None_Maybe()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybeNone = Maybe.Some<BaseClass>(baseClass);
                var maybeNone2 = Maybe.None<BaseClass>();

                // Act
                var isNotEqual = maybeNone != maybeNone2;

                // Assert
                isNotEqual.ShouldBeTrue();
            }
        }

        public sealed class TheImplicitOperator
        {
            [Fact]
            public void Should_Return_Some_Maybe_If_Assigned_With_Some()
            {
                // Arrange
                var some = new BaseClass();

                // Act
                Maybe<BaseClass> maybeSome = some;

                // Assert
                maybeSome.ShouldBeOfType<Maybe<BaseClass>>();
                maybeSome.HasValue.ShouldBeTrue();
                maybeSome.ValueOrException().ShouldBe(some);
            }

            [Fact]
            public void Should_Return_None_Maybe_If_Assigned_With_Null()
            {
                // Act
                Maybe<BaseClass> maybeNone = null;

                // Assert
                maybeNone.ShouldBeOfType<Maybe<BaseClass>>();
                maybeNone.HasValue.ShouldBeFalse();
                maybeNone.ShouldBe(Maybe.None<BaseClass>());
            }
        }

        public sealed class TheGetHashCodeMethod
        {
            [Fact]
            public void Should_Return_HashCode_Of_Value_If_Some()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybe = Maybe.Some(baseClass);

                // Act
                var hashCode = maybe.GetHashCode();

                // Assert
                hashCode.ShouldBe(baseClass.GetHashCode());
            }

            [Fact]
            public void Should_Return_HashCode_Of_Base_If_None()
            {
                // Arrange
                var maybe = Maybe.None<BaseClass>();

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
                var maybe = Maybe.None<BaseClass>();

                // Act
                var exception = Record.Exception(() => maybe.GetObjectData(null, default(StreamingContext)));

                // Assert
                exception.ShouldBeOfType<ArgumentNullException>();
            }

            [Fact]
            public void Should_Add_Value_To_SerializationInfo()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybe = Maybe.Some(baseClass);
                var serializationInfo = new SerializationInfo(typeof(Maybe<>), new FormatterConverter());

                // Act
                maybe.GetObjectData(serializationInfo, default(StreamingContext));

                // Assert
                var value = serializationInfo.GetValue("value", typeof(BaseClass));
                value.ShouldBe(baseClass);
            }
        }

        public sealed class TheToStringMethod
        {
            [Fact]
            public void Should_Return_String_Representation_Of_Value_If_Some()
            {
                // Arrange
                var baseClass = new BaseClass();
                var maybe = Maybe.Some(baseClass);

                // Act
                var stringRepresentation = maybe.ToString();

                // Assert
                stringRepresentation.ShouldBe(baseClass.ToString());
            }

            [Fact]
            public void Should_Return_Emptry_String_If_None()
            {
                // Arrange
                var maybe = Maybe.None<BaseClass>();

                // Act
                var stringRepresentation = maybe.ToString();

                // Assert
                stringRepresentation.ShouldBe(string.Empty);
            }
        }
    }
}
