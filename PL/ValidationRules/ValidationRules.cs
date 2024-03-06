using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL.ValidationRules;

/// <summary>
/// Validation rule for cost field
/// </summary>
internal class CostValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        //If no value was entered
        if (string.IsNullOrEmpty(value.ToString()))
            return new ValidationResult(false, "Enter value");

        double d;
        if (double.TryParse(value.ToString(), out d) && d > 0)
            return ValidationResult.ValidResult;
        return new ValidationResult(false, "Invalid Cost");
    }
}

/// <summary>
/// Validation rule for id field
/// </summary>
internal class IdValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        //If no value was entered
        if (string.IsNullOrEmpty(value.ToString()))
            return new ValidationResult(false, "Enter value");

        int d;
        if (int.TryParse(value.ToString(), out d) && d > 0)
            return ValidationResult.ValidResult;
        return new ValidationResult(false, "Invalid Id");
    }
}

/// <summary>
/// Validation rule for email field
/// </summary>
internal class EmailValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        //If no value was entered
        if (string.IsNullOrEmpty(value.ToString()))
            return new ValidationResult(false, "Enter value");

        // Use the EmailAddressAttribute to validate the email address
        if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(value.ToString()))
            return new ValidationResult(false, "Invalid Email");

        return ValidationResult.ValidResult;
    }
}

/// <summary>
/// Validation rule for string fields
/// </summary>
internal class StringValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        //If no value was entered
        if (string.IsNullOrEmpty(value.ToString()))
            return new ValidationResult(false, "Enter value");

        return ValidationResult.ValidResult;
    }
}

/// <summary>
/// Validation rule for TimeSpan fields
/// </summary>
internal class TimeSpanValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        //If no value was entered
        if (string.IsNullOrEmpty(value.ToString()))
            return new ValidationResult(false, "Enter value");

        TimeSpan d;
        if (TimeSpan.TryParse(value.ToString(), out d) && d > TimeSpan.Zero)
            return ValidationResult.ValidResult;
        return new ValidationResult(false, "Invalid duration");
    }
}