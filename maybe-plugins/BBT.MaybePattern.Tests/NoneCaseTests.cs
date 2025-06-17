namespace BBT.MaybePattern.Tests
{
    using System;
    using BBT.MaybePattern;
    using BBT.MaybePattern.Tests.TestData;
    using Shouldly;
    using Xunit;

    public sealed class NoneCaseTests
    {
        public sealed class TheDoIfNoneMethod
        {
            [Fact]
            public void Should_Not_Call_Action_If_Case_Is_Not_None()
            {
                // Arrange
                var noneCase = new NoneCase(false);
                var called = false;

                // Act
                noneCase.DoIfNone(() => called = true);

                // Assert
                called.ShouldBeFalse();
            }

            [Fact]
            public void Should_Call_Action_If_Case_Is_None()
            {
                // Arrange
                var noneCase = new NoneCase(true);
                var called = false;

                // Act
                noneCase.DoIfNone(() => called = true);

                // Assert
                called.ShouldBeTrue();
            }

            [Fact]
            public void Should_Throw_ArguemntNullException_If_DoAction_Is_Null()
            {
                // Arrange
                var noneCase = new NoneCase(true);

                // Act
                var exception = Record.Exception(() => noneCase.DoIfNone(null));

                // Assert
                exception.ShouldBeOfType<ArgumentNullException>();
            }
        }

        public sealed class TheEqualsMethod
        {
            [Fact]
            public void Should_Return_True_If_Both_Are_None()
            {
                // Arrange
                var noneCase = new NoneCase(true);
                var noneCase2 = new NoneCase(true);

                // Act
                var isEqual = object.Equals(noneCase, noneCase2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void Should_Return_True_If_Both_Are_Not_None()
            {
                // Arrange
                var noneCase = new NoneCase(false);
                var noneCase2 = new NoneCase(false);

                // Act
                var isEqual = object.Equals(noneCase, noneCase2);

                // Assert
                isEqual.ShouldBeTrue();
            }

            [Fact]
            public void Should_Return_False_If_One_Is_None_And_One_Is_Not_None()
            {
                // Arrange
                var noneCase = new NoneCase(false);
                var noneCase2 = new NoneCase(true);

                // Act
                var isEqual = object.Equals(noneCase, noneCase2);

                // Assert
                isEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_False_If_Object_Is_Not_Of_Type_NoneCase()
            {
                // Arrange
                var baseClass = new BaseClass();
                var noneCase = new NoneCase(true);

                // Act
                var isEqual = noneCase.Equals(baseClass);

                // Assert
                isEqual.ShouldBeFalse();
            }
        }

        public sealed class TheNotEqualOperator
        {
            [Fact]
            public void Should_Return_False_If_Both_Are_None_Cases()
            {
                // Arrange
                var caseNone = new NoneCase(true);
                var caseNone2 = new NoneCase(true);

                // Act
                var isNotEqual = caseNone != caseNone2;

                // Assert
                isNotEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_False_If_Both_Are_NotNone_Cases()
            {
                // Arrange
                var caseNone = new NoneCase(false);
                var caseNone2 = new NoneCase(false);

                // Act
                var isNotEqual = caseNone != caseNone2;

                // Assert
                isNotEqual.ShouldBeFalse();
            }

            [Fact]
            public void Should_Return_True_If_One_Is_Non_And_One_Is_NotNon_Case()
            {
                // Arrange

                var caseNone = new NoneCase(true);
                var caseNone2 = new NoneCase(false);

                // Act
                var isNotEqual = caseNone != caseNone2;

                // Assert
                isNotEqual.ShouldBeTrue();
            }
        }

        public sealed class TheGetHasCodeMethod
        {
            [Fact]
            public void Should_Return_HashCode_Of_Boolean()
            {
                // Arrange
                var isNoneCase = true;
                var caseNone = new NoneCase(isNoneCase);

                // Act
                var hashCode = caseNone.GetHashCode();

                // Assert
                hashCode.ShouldBe(isNoneCase.GetHashCode());
            }
        }
    }
}
