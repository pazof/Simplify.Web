﻿using System;
using Moq;
using NUnit.Framework;
using Simplify.DI;
using Simplify.Web.Model.Validation.Attributes;
using Simplify.Web.Modules.Data;

namespace Simplify.Web.Tests.Model.Validation.Attributes
{
	[TestFixture]
	public class RequiredAttributeTests : AttributesTestBase
	{
		private readonly string _defaultMessage = $"Required property '{nameof(TestEntityWithProperty.Prop1)}' is null or empty";
		private readonly string _customMessage = "Hello world!";

		[OneTimeSetUp]
		public void SetupAttribute()
		{
			Attr = new RequiredAttribute();
		}

		[Test]
		public void Validate_NotNullReference_NoExceptions()
		{
			TestAttributeForValidValue(new object());
		}

		[Test]
		public void Validate_DefaultInt_NoExceptions()
		{
			TestAttributeForValidValue(default(int));
		}

		[Test]
		public void Validate_NullReference_DefaultModelValidationException()
		{
			TestAttribute(null, _defaultMessage);
		}

		[Test]
		public void Validate_DefaultDateTime_DefaultModelValidationException()
		{
			TestAttribute(default(DateTime), _defaultMessage);
		}

		[Test]
		public void Validate_NullReferenceWithCustomError_ModelValidationExceptionWithCustomError()
		{
			// Assign

			var attr = new RequiredAttribute(_customMessage, false);

			TestAttribute(default(DateTime), _customMessage, attr);
		}

		[Test]
		public void Validate_NullReferenceWithMessageFromStringTable_ModelValidationExceptionWithMessageFromStringTable()
		{
			// Assign

			var attr = new RequiredAttribute("MyKey");
			var st = Mock.Of<IStringTable>(x => x.GetItem(It.Is<string>(s => s == "MyKey")) == _customMessage);
			Resolver = Mock.Of<IDIResolver>(x => x.Resolve(It.Is<Type>(t => t == typeof(IStringTable))) == st);

			// Act
			TestAttribute(null, _customMessage, attr);
		}
	}
}