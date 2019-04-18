using System;
using System.Globalization;
using System.Net;
using System.Windows.Controls;

namespace Watcher
{
    //класс проверкикорректного ввода значений процентов
    public class RulesForPersent : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int val;
            try
            {
                val = int.Parse((string)value);
            }
            catch
            {
                return new ValidationResult(false, "Недопустимые символы. Или значение больше " + int.MaxValue);
            }

            if (val < 0 || val > 100)
                return new ValidationResult(false, "Выход за границы диапазона от 0 до 100 процентов");
            else
                return new ValidationResult(true, null);

        }
    }

    //проверка корректного ввода целочисленных параметров
    public class RulesForInteger : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int val;
            try
            {
                val = int.Parse((string)value);
            }
            catch
            {
                return new ValidationResult(false, "Недопустимые символы. Или значение больше " + int.MaxValue);
            }

            return new ValidationResult(true, null);
        }
    }

    //проверка корректного ввода IP адреса
    public class RulesForIp : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            IPAddress val;

            if ((string)value == "")
                return new ValidationResult(true, null);

            try
            {
                val = IPAddress.Parse((string)value);
            }
            catch
            {
                return new ValidationResult(false, $"Недопустимое значение IP");
            }

            return new ValidationResult(true, null);
        }
    }

    //проверка корректного ввода порта
    public class RulesForPort : ValidationRule
    {
        static readonly int bottomLimit = 1;
        static readonly int topLimit = 65535;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int val;
            try
            {
                val = int.Parse((string)value);
                if (val < bottomLimit || val > topLimit)
                    throw new Exception();
            }
            catch
            {
                return new ValidationResult(false, $"Недопустимое значение порта. Диапазон [{bottomLimit}; {topLimit}]");
            }

            return new ValidationResult(true, null);
        }
    }
}
