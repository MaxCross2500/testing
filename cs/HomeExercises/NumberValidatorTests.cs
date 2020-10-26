﻿using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises
{
	public class NumberValidatorTests
	{
		[TestCase(-1, 2, TestName = "Ctor_OnNegativePrecision_Throws")]
		[TestCase(0, 2, TestName = "Ctor_OnZeroPrecision_Throws")]
		[TestCase(1, -1, TestName = "Ctor_OnNegativePrecision_Throws")]
		[TestCase(2, -1, TestName = "Ctor_OnNegativeScale_Throws")]
		[TestCase(2, 3, TestName = "Ctor_OnPrecisionLessThanScale_Throws")]
		[TestCase(2, 2, TestName = "Ctor_OnPrecisionEqualsScale_Throws")]
		public void Ctor_Throws(int precision, int scale = 0, bool onlyPositive = false)
		{
			((Action) (() => new NumberValidator(precision, scale, onlyPositive))).Should().Throw<ArgumentException>();
		}

		[TestCase(1, 0, true, TestName = "Ctor_OnZeroScale_NotThrows")]
		[TestCase(2, 1, false)]
		[TestCase(3, 2, true)]
		[TestCase(5, 3, false)]
		[TestCase(7, 2, true)]
		public void Ctor_OnGoodParameters_NotThrows(int precision, int scale = 0, bool onlyPositive = false)
		{
			((Action) (() => new NumberValidator(precision, scale, onlyPositive))).Should().NotThrow<ArgumentException>();
		}

		[Test]
		public void IsValidNumber_OnNotNegativeIntegers_ShouldBeTrue()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber("0").Should().BeTrue();
			validator.IsValidNumber("000").Should().BeTrue();
			validator.IsValidNumber("1").Should().BeTrue();
			validator.IsValidNumber("123").Should().BeTrue();
			validator.IsValidNumber("12345").Should().BeTrue();
			validator.IsValidNumber("1234567").Should().BeTrue();
			
			validator = new NumberValidator(7, 5, false);
			validator.IsValidNumber("0").Should().BeTrue();
			validator.IsValidNumber("000").Should().BeTrue();
			validator.IsValidNumber("1").Should().BeTrue();
			validator.IsValidNumber("123").Should().BeTrue();
			validator.IsValidNumber("12345").Should().BeTrue();
			validator.IsValidNumber("1234567").Should().BeTrue();
		}
		
		[Test]
		public void IsValidNumber_OnNotNegativeIntegersWithSign_ShouldBeTrue()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber("+0").Should().BeTrue();
			validator.IsValidNumber("+000").Should().BeTrue();
			validator.IsValidNumber("+1").Should().BeTrue();
			validator.IsValidNumber("+123").Should().BeTrue();
			validator.IsValidNumber("+12345").Should().BeTrue();
			validator.IsValidNumber("+123456").Should().BeTrue();
			
			validator = new NumberValidator(7, 5, false);
			validator.IsValidNumber("+0").Should().BeTrue();
			validator.IsValidNumber("+000").Should().BeTrue();
			validator.IsValidNumber("+1").Should().BeTrue();
			validator.IsValidNumber("+123").Should().BeTrue();
			validator.IsValidNumber("+12345").Should().BeTrue();
			validator.IsValidNumber("+123456").Should().BeTrue();
		}
		
		[Test]
		public void IsValidNumber_OnGoodNegativeIntegers_ShouldBeTrue()
		{
			var validator = new NumberValidator(7, 5, false);
			validator.IsValidNumber("-1").Should().BeTrue();
			validator.IsValidNumber("-123").Should().BeTrue();
			validator.IsValidNumber("-12345").Should().BeTrue();
			validator.IsValidNumber("-123456").Should().BeTrue();
		}
		
		[Test]
		public void IsValidNumber_OnUnexpectedNegativeIntegers_ShouldBeFalse()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber("-1").Should().BeFalse();
			validator.IsValidNumber("-123").Should().BeFalse();
			validator.IsValidNumber("-12345").Should().BeFalse();
			validator.IsValidNumber("-123456").Should().BeFalse();
		}

		[Test]
		public void IsValidNumber_OnTooLargeIntegers_ShouldBeFalse()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber("12345678").Should().BeFalse();
			validator.IsValidNumber("1234567890123456").Should().BeFalse();
			
			validator = new NumberValidator(7, 5, false);
			validator.IsValidNumber("-1234567").Should().BeFalse();
			validator.IsValidNumber("-1234567890123456").Should().BeFalse();
		}

		[Test]
		public void IsValidNumber_OnGoodNotNegativeFloats_ShouldBeTrue()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber("0.0").Should().BeTrue();
			validator.IsValidNumber("00.0").Should().BeTrue();
			validator.IsValidNumber("00.000").Should().BeTrue();
			validator.IsValidNumber("123.4567").Should().BeTrue();
			validator.IsValidNumber("023.0567").Should().BeTrue();
			
			validator = new NumberValidator(7, 5, false);
			validator.IsValidNumber("0.0").Should().BeTrue();
			validator.IsValidNumber("00.0").Should().BeTrue();
			validator.IsValidNumber("00.000").Should().BeTrue();
			validator.IsValidNumber("123.4567").Should().BeTrue();
			validator.IsValidNumber("023.0567").Should().BeTrue();
		}
		
		[Test]
		public void IsValidNumber_OnGoodNotNegativeFloatsWithSign_ShouldBeTrue()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber("+0.0").Should().BeTrue();
			validator.IsValidNumber("+00.0").Should().BeTrue();
			validator.IsValidNumber("+00.000").Should().BeTrue();
			validator.IsValidNumber("+123.456").Should().BeTrue();
			validator.IsValidNumber("+023.056").Should().BeTrue();
			
			validator = new NumberValidator(7, 5, false);
			validator.IsValidNumber("+0.0").Should().BeTrue();
			validator.IsValidNumber("+00.0").Should().BeTrue();
			validator.IsValidNumber("+00.000").Should().BeTrue();
			validator.IsValidNumber("+123.456").Should().BeTrue();
			validator.IsValidNumber("+023.056").Should().BeTrue();
		}
		
		[Test]
		public void IsValidNumber_OnUnexpectedFloats_ShouldBeFalse()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber("-0.0").Should().BeFalse();
			validator.IsValidNumber("-00.0").Should().BeFalse();
			validator.IsValidNumber("-00.000").Should().BeFalse();
			validator.IsValidNumber("-123.456").Should().BeFalse();
			validator.IsValidNumber("-023.056").Should().BeFalse();
		}
		
		[Test]
		public void IsValidNumber_OnTooLargeFloats_ShouldBeFalse()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber("123.45678").Should().BeFalse();
			validator.IsValidNumber("1.123456").Should().BeFalse();
			validator.IsValidNumber("123456.123456").Should().BeFalse();
			
			validator = new NumberValidator(7, 5, false);
			validator.IsValidNumber("123.45678").Should().BeFalse();
			validator.IsValidNumber("1.123456").Should().BeFalse();
			validator.IsValidNumber("123456.123456").Should().BeFalse();
			validator.IsValidNumber("-123.4567").Should().BeFalse();
			validator.IsValidNumber("-1.123456").Should().BeFalse();
			validator.IsValidNumber("-123456.123456").Should().BeFalse();
		}

		[Test]
		public void IsValidNumber_OnComaSeparatedFloats_ShouldBeTrue()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber("0,0").Should().BeTrue();
			validator.IsValidNumber("00,0").Should().BeTrue();
			validator.IsValidNumber("00,000").Should().BeTrue();
			validator.IsValidNumber("123,4567").Should().BeTrue();
			validator.IsValidNumber("023,0567").Should().BeTrue();
		}

		[Test]
		public void IsValidNumber_OnNotNullOrEmptyString_ShouldBeFalse()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber(null).Should().BeFalse();
			validator.IsValidNumber("").Should().BeFalse();
		}

		[Test]
		public void IsValidNumber_OnNotNumbers_ShouldBeFalse()
		{
			var validator = new NumberValidator(7, 5, true);
			validator.IsValidNumber(".0").Should().BeFalse();
			validator.IsValidNumber("0.").Should().BeFalse();
			validator.IsValidNumber(".").Should().BeFalse();
			validator.IsValidNumber(",").Should().BeFalse();
			
			validator.IsValidNumber("abc").Should().BeFalse();
			validator.IsValidNumber("abc.def").Should().BeFalse();
			validator.IsValidNumber("Hi").Should().BeFalse();
			validator.IsValidNumber("What's up?").Should().BeFalse();
			validator.IsValidNumber("hi.world").Should().BeFalse();
		}
	}

	public class NumberValidator
	{
		private readonly Regex numberRegex;
		private readonly bool onlyPositive;
		private readonly int precision;
		private readonly int scale;

		public NumberValidator(int precision, int scale = 0, bool onlyPositive = false)
		{
			this.precision = precision;
			this.scale = scale;
			this.onlyPositive = onlyPositive;
			if (precision <= 0)
				throw new ArgumentException("precision must be a positive number");
			if (scale < 0 || scale >= precision)
				throw new ArgumentException("precision must be a non-negative number less or equal than precision");
			numberRegex = new Regex(@"^([+-]?)(\d+)([.,](\d+))?$", RegexOptions.IgnoreCase);
		}

		public bool IsValidNumber(string value)
		{
			// Проверяем соответствие входного значения формату N(m,k), в соответствии с правилом, 
			// описанным в Формате описи документов, направляемых в налоговый орган в электронном виде по телекоммуникационным каналам связи:
			// Формат числового значения указывается в виде N(m.к), где m – максимальное количество знаков в числе, включая знак (для отрицательного числа), 
			// целую и дробную часть числа без разделяющей десятичной точки, k – максимальное число знаков дробной части числа. 
			// Если число знаков дробной части числа равно 0 (т.е. число целое), то формат числового значения имеет вид N(m).

			if (string.IsNullOrEmpty(value))
				return false;

			var match = numberRegex.Match(value);
			if (!match.Success)
				return false;

			// Знак и целая часть
			var intPart = match.Groups[1].Value.Length + match.Groups[2].Value.Length;
			// Дробная часть
			var fracPart = match.Groups[4].Value.Length;

			if (intPart + fracPart > precision || fracPart > scale)
				return false;

			if (onlyPositive && match.Groups[1].Value == "-")
				return false;
			return true;
		}
	}
}