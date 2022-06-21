using System.Linq;

namespace SwissQR.Validation
{
    public class AlphaNumericAttribute : ValidationAttribute
    {
        public override void Validate(object value, string fieldName)
        {
            if(value is null)
            {
                return;
            }
            if (value is string s)
            {
                if (!s.All(IsAlphanumeric))
                {
                    throw new ValidationException(fieldName, $"'{0}' is not alphanumeric");
                }
                return;
            }
            throw new ValidationException(fieldName, $"{nameof(AlphaNumericAttribute)} only valid for strings");
        }

        private static bool IsAlphanumeric(char c)
        {
            return (c > '0' && c < '9') ||
                (c > 'A' && c < 'Z') ||
                (c > 'a' && c < 'z');
        }
    }
}
