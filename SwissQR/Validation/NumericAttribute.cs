using System.Text.RegularExpressions;

namespace SwissQR.Validation
{
    public class NumericAttribute : ValidationAttribute
    {
        /// <summary>
        /// Generic number format validation string
        /// </summary>
        /// <remarks>Matches empty string, integers, floating point and exponential notation</remarks>
        private const string REGEX = @"^[+\-]?[^Ee]\d*(.\d+)?([eE][+\-]?\d+)?$";

        public override void Validate(object value, string fieldName)
        {
            if (value is char c)
            {
                if (c >= '0' && c <= '9')
                {
                    return;
                }
            }
            if (value is string s)
            {
                if (s.Length > 0 && Regex.IsMatch(s, @"^[+\-]?\d*(.\d+)?([eE][+\-]?\d+)?$"))
                {
                    return;
                }
                throw new ValidationException(fieldName, "Field is not numeric");
            }
            throw new ValidationException(fieldName, "Field is not a string or character");
        }
    }
}
