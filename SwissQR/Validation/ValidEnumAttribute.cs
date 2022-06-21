using System;
using System.Linq;

namespace SwissQR.Validation
{
    public class ValidEnumAttribute : ValidationAttribute
    {
        public override void Validate(object value, string fieldName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (fieldName is null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            var T = value.GetType();
            if (!T.IsEnum)
            {
                throw new ValidationException(fieldName, $"'{value}' of type '{T}' is not an enum");
            }
            var Values = Enum.GetValues(T);
            var numeric = Convert.ToInt64(value);
            var Sum = 0L;
            foreach (var V in Values)
            {
                var enumNumeric = Convert.ToInt64(V);
                if (enumNumeric == numeric)
                {
                    return;
                }
                Sum |= Convert.ToInt64(V);
            }
            if (T.CustomAttributes.Any(m => m.AttributeType == typeof(FlagsAttribute)))
            {
                
                if (numeric != (Sum & numeric))
                {
                    //Extra flags not in sum
                    throw new ValidationException(fieldName, $"'{value}' has unknown flags of '{T}'");
                }
            }
            else
            {
                throw new ValidationException(fieldName, $"'{value}' is not a valid value of enum '{T}'");
            }
        }
    }
}