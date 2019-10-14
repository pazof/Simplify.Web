﻿using NUnit.Framework;
using Simplify.Web.Model.Validation;

namespace Simplify.Web.Tests.Model.Validation
{
	[TestFixture]
	public class ObjectPropertiesValidatorTests
	{
		private ValidationAttributesExecutor _validator;

		[SetUp]
		public void Initialize()
		{
			_validator = new ValidationAttributesExecutor();
		}

		// TODO refactor
		//[Test]
		//public void Validate_RequiredListFieldIsNull_ExceptionThrown()
		//{
		//	// Assign
		//	var model = new TestModelRequired { Prop2 = new List<string>() };

		//	// Act & Assert
		//	Assert.Throws<ModelValidationException>(() => _validator.Validate(model));
		//}

		//[Test]
		//public void Validate_RequiredStringFieldIsNull_ExceptionThrown()
		//{
		//	// Assign
		//	var model = new TestModelRequired { Prop1 = "test" };

		//	// Act & Assert
		//	Assert.Throws<ModelValidationException>(() => _validator.Validate(model));
		//}

		//[Test]
		//public void Validate_RequiredFieldsIsNormal_Ok()
		//{
		//	// Assign
		//	var model = new TestModelRequired { Prop1 = "test", Prop2 = new List<string>(), Prop3 = new TestModelEMail() };

		//	// Act & Assert
		//	_validator.Validate(model);
		//}

		//[Test]
		//public void Validate_UserTypeRequiredFieldIsNullOtherIsOk_ExceptionThrown()
		//{
		//	// Assign
		//	var model = new TestModelRequired { Prop1 = "test", Prop2 = new List<string>() };

		//	// Act & Assert
		//	Assert.Throws<ModelValidationException>(() => _validator.Validate(model));
		//}

		//[Test]
		//public void Validate_RequiredCustomArrayFieldIsNull_ExceptionThrown()
		//{
		//	// Assign
		//	var model = new TestModelRequiredCustomArray();

		//	// Act & Assert
		//	Assert.Throws<ModelValidationException>(() => _validator.Validate(model));
		//}

		//[Test]
		//public void Validate_RequiredCustomArrayFieldIsNotNull_NoExceptions()
		//{
		//	// Assign
		//	var model = new TestModelRequiredCustomArray { Prop1 = new TestModelEMail[1] { null } };

		//	// Act & Assert
		//	_validator.Validate(model);
		//}

		//[Test]
		//public void Validate_RequiredCustomListFieldIsNull_ExceptionThrown()
		//{
		//	// Assign
		//	var model = new TestModelRequiredCustomGenericList();

		//	// Act & Assert
		//	Assert.Throws<ModelValidationException>(() => _validator.Validate(model));
		//}

		//[Test]
		//public void Validate_RequiredCustomListFieldIsNotNull_NoExceptions()
		//{
		//	// Assign
		//	var model = new TestModelRequiredCustomGenericList() { Prop1 = new List<TestModelEMail>() };

		//	// Act & Assert
		//	_validator.Validate(model);
		//}
	}
}