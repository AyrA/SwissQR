using System;

namespace SwissQR.Validation
{
    public class LengthAttribute : ValidationAttribute
    {
        public int MinLength { get; }
        public int MaxLength { get; }

        public LengthAttribute(int min, int max)
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max));
            }
            if (min < 0 || min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min));
            }
            MinLength = min;
            MaxLength = max;
        }

        public LengthAttribute(int max) : this(0, max) { }

        public override void Validate(object value, string fieldName)
        {
            if (value == null)
            {
                return;
            }
            var v = value.ToString();
            if (v.Length >= MinLength && v.Length <= MaxLength)
            {
                return;
            }
            throw new ValidationException(fieldName, $"'{v}' must be between {MinLength} and {MaxLength} characters inclusive but is {v.Length}");
        }
    }
}