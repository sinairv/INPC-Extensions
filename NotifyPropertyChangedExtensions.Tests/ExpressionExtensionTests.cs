using System;
using System.Linq.Expressions;
using NUnit.Framework;
// ReSharper disable InconsistentNaming

namespace NotifyPropertyChangedExtensions.Tests
{
    [TestFixture]
    public class ExpressionExtensionTests
    {
        [Test]
        public void IsNotOfMemberAccess_ReturnsFalseForRegularBooleanMemberAccess()
        {
            // Arrange and Act
            Expression<Func<TestClass, bool>> expression = testObj => testObj.TheBoolean;

            // Assert
            Assert.IsFalse(expression.IsNotOfMemberAccess());
        }

        [Test]
        public void IsNotOfMemberAccess_ReturnsTrueForANegatedBooleanMemberAccess()
        {
            // Arrange and Act
            Expression<Func<TestClass, bool>> expression = testObj => !testObj.TheBoolean;

            // Assert
            Assert.IsTrue(expression.IsNotOfMemberAccess());
        }

    }

    public class TestClass
    {
        public bool TheBoolean { get; set; }

        public int TheInteger { get; set; }
    
    }
}
