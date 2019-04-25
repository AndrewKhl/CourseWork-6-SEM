using System;
using System.Globalization;
using System.Net;
using System.Windows.Controls;

namespace Watcher
{
    public class RulesForPersent : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if ((value as string)?.Trim() == string.Empty)
                return new ValidationResult(false, "This field is required");

            if (!int.TryParse(value.ToString(),out int val))
                return new ValidationResult(false, "Incorrect symbols or lentgh grather than 9");
            
            if (val < 1 || val > 100)
                return new ValidationResult(false, "Out of range 1..100%");
            else
                return new ValidationResult(true, null);
        }
    }

    public class RulesFromTime : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if ((value as string)?.Trim() == string.Empty)
                return new ValidationResult(false, "This field is required");

            if (!int.TryParse(value.ToString(), out int val))
                return new ValidationResult(false, "Incorrect symbols or lentgh grather than 9");

            if (val < 5)
                return new ValidationResult(false, "Value less than 5");
            else
                return new ValidationResult(true, null);
        }
    }

    public class RulesForInteger : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if ((value as string)?.Trim() == string.Empty)
                return new ValidationResult(false, "This field is required");

            if (!int.TryParse(value.ToString(), out int val))
                return new ValidationResult(false, "Incorrect symbols or lentgh grather than 9");

            return new ValidationResult(true, null);
        }
    }

    public class RulesForIp : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if ((value as string)?.Trim() == string.Empty)
                return new ValidationResult(true, null);

            if (!IPAddress.TryParse((string)value, out IPAddress val))
                return new ValidationResult(false, $"Incorrect IP adress");

            return new ValidationResult(true, null);
        }
    }

    public class RulesForPort : ValidationRule
    {
        static readonly int bottomLimit = 1;
        static readonly int topLimit = 65535;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if ((value as string)?.Trim() == string.Empty)
                return new ValidationResult(true, null);

            if (!int.TryParse(value.ToString(), out int val))
                return new ValidationResult(false, "Incorrect symbols or lentgh grather than 9");
        
            if (val < bottomLimit || val > topLimit)
                return new ValidationResult(false, $"Out of range {bottomLimit}..{topLimit}");
            else
                return new ValidationResult(true, null);
        }
    }
}
