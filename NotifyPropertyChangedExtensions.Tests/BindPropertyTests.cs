using System;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace NotifyPropertyChangedExtensions.Tests
{
    [TestFixture]
    public class NotifyPropertyChangedExtensionTests
    {
        [Test]
        public void RaisePropertyChangedMethodWorks()
        {
            // Arrange
            var source = new SourceClass();

            string propertyName = null;

            source.PropertyChanged += (sender, args) =>
            {
                propertyName = args.PropertyName;
            };

            // Act
            source.Number = 10;

            // Assert
            Assert.AreEqual("Number", propertyName);
        }

        [Test]
        public void IntegerPropertiesAreBound()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.Number, target, t => t.Number);
            // Act
            source.Number = 10;

            // Assert
            Assert.AreEqual(10, target.Number);
        }

        [Test]
        public void IntegerPropertiesAreInitialised_IfInitialiseOptionIsSet()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();
            source.Number = 10;

            // Act
            source.BindProperty(src => src.Number, target, t => t.Number, BindPropertyOptions.InitialiseTarget);

            // Assert
            Assert.AreEqual(10, target.Number);
        }


        [Test]
        public void IntegerPropertiesAreNotInitialised_IfInitialiseOptionIsNotSet()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();
            source.Number = 10;

            // Act
            source.BindProperty(src => src.Number, target, t => t.Number, BindPropertyOptions.None);

            // Assert
            Assert.AreEqual(0, target.Number);
        }

        [Test]
        public void BoolPropertiesAreBound()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.Flag, target, t => t.Flag);
            // Act
            source.Flag = true;

            // Assert
            Assert.AreEqual(true, target.Flag);
        }

        [Test]
        public void DateTimePropertiesAreBound()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.Date, target, t => t.Date);
            // Act
            source.Date = new DateTime(2012, 10, 3);

            // Assert
            Assert.AreEqual(new DateTime(2012, 10, 3), target.Date);
        }

        [Test]
        public void NonNullStringPropertiesAreBound()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.String, target, t => t.String);
            // Act
            source.String = "Hi";

            // Assert
            Assert.AreEqual("Hi", target.String);
        }

        [Test]
        public void NullStringPropertiesAreBound()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.String = "Src";
            target.String = "Target";

            source.BindProperty(src => src.String, target, t => t.String);
            // Act
            source.String = null;

            // Assert
            Assert.AreEqual(null, target.String);
        }

        [Test]
        public void IntergerIsBindableToNullableOfInteger()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.Number, target, t => t.OptionalNumber);
            // Act
            source.Number = 10;

            // Assert
            Assert.AreEqual(10, target.OptionalNumber);
        }

        [Test]
        public void NullableIntergerIsBindableToInteger_WhenSourceIsNotNull()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.OptionalNumber, target, t => t.Number);
            // Act
            source.OptionalNumber = 10;

            // Assert
            Assert.AreEqual(10, target.Number);
        }

        [Test]
        public void NullableIntergerIsBindableToInteger_WhenSourceIsNull_UpdateTargetToDefault()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            target.Number = 10;

            source.BindProperty(src => src.OptionalNumber, target, t => t.Number);
            // Act
            source.OptionalNumber = null;

            // Assert
            Assert.AreEqual(0, target.Number);
        }

        [Test]
        public void NullableIntergerIsBindableToInteger_WhenSourceIsNull_DontUpdateTarget_IfOptionIsSpecified()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            target.Number = 10;

            source.BindProperty(src => src.OptionalNumber, target, t => t.Number, BindPropertyOptions.DontUpdateWhenSourceIsNullAndTargetIsNotNullable);

            // Act
            source.OptionalNumber = null;

            // Assert
            Assert.AreEqual(10, target.Number);
        }

        [Test]
        public void BooleanCanBeBoundToANegatedBoolean()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.Flag, target, t => !t.Flag);

            // Act
            source.Flag = false;

            // Assert
            Assert.AreEqual(true, target.Flag);
        }

        [Test]
        public void BooleanCanBeInitialisedWithANegatedBoolean()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();
            source.Flag = false;

            // Act
            source.BindProperty(src => src.Flag, target, t => !t.Flag, BindPropertyOptions.InitialiseTarget);

            // Assert
            Assert.AreEqual(true, target.Flag);
        }

        [Test]
        public void NullableBooleanCanBeBoundToANegatedBoolean_WhenItsNotNull()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.OptionalFlag, target, t => !t.Flag);

            // Act
            source.OptionalFlag = false;

            // Assert
            Assert.AreEqual(true, target.Flag);
        }

        [Test]
        public void NullableBooleanCanBeBoundToANegatedBoolean_WhenItsNull()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();
            target.Flag = false;

            source.BindProperty(src => src.OptionalFlag, target, t => !t.Flag);

            // Act
            source.OptionalFlag = null;

            // Assert
            Assert.AreEqual(true, target.Flag);
        }


        [Test]
        public void NullableBooleanCanBeBoundToANegatedBoolean_DontUpdateTargetWhenItsNull_AndOptionIsSet()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();
            target.Flag = false;

            source.BindProperty(src => src.OptionalFlag, target, t => !t.Flag, BindPropertyOptions.DontUpdateWhenSourceIsNullAndTargetIsNotNullable);

            // Act
            source.OptionalFlag = null;

            // Assert
            Assert.AreEqual(false, target.Flag);
        }

        [Test]
        public void BooleanCanBeBoundToANegatedNullableBoolean()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();
            target.OptionalFlag = null;

            source.BindProperty(src => src.Flag, target, t => !t.OptionalFlag);

            // Act
            source.Flag = false;

            // Assert
            Assert.AreEqual(true, target.OptionalFlag);
        }

        [Test]
        public void NullableBooleanCanBeBoundToANegatedNullableBoolean()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.OptionalFlag, target, t => !t.OptionalFlag);

            // Act
            source.OptionalFlag = false;

            // Assert
            Assert.AreEqual(true, target.OptionalFlag);
        }


        [Test]
        public void NullableBooleanCanBeBoundToANegatedNullableBoolean_WhenItsNullTargetWillBeTrue()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.OptionalFlag, target, t => !t.OptionalFlag);

            // Act
            source.OptionalFlag = null;

            // Assert
            Assert.AreEqual(true, target.OptionalFlag);
        }

        [Test]
        public void NullableBooleanCanBeBoundToANegatedNullableBoolean_WhenItsNullTargetWillBeTrue_EvenWhenOptionIsSet()
        {
            // Arrange
            var source = new SourceClass();
            var target = new TargetClass();

            source.BindProperty(src => src.OptionalFlag, target, t => !t.OptionalFlag, BindPropertyOptions.DontUpdateWhenSourceIsNullAndTargetIsNotNullable);

            // Act
            source.OptionalFlag = null;

            // Assert
            Assert.AreEqual(true, target.OptionalFlag);
        }


        [Test]
        public void CanAssociateEventHandlersToPropertyChangedEvents()
        {
            // Arrange
            var source = new SourceClass();

            source.Number = 10;

            SourceClass eventSender = null;

            source.HandleOnPropertyChanged(src => src.Number, sender =>
            {
                eventSender = sender;
            });

            // Act
            source.Number = 12;

            // Assert
            Assert.AreSame(source, eventSender);
        }

        [Test]
        public void TheAssociatedEventHandler_IsNotInvokedForUnrelatedProperties()
        {
            // Arrange
            var source = new SourceClass();

            source.Number = 10;

            bool isCalled = false;

            source.HandleOnPropertyChanged(src => src.Number, sender =>
            {
                isCalled = true;
            });

            // Act
            source.Date = DateTime.Now;

            // Assert
            Assert.IsFalse(isCalled);
        }

    }
}
