using System;

namespace SwissQR.Validation
{
    public class LengthAttribute : Attribute
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
    }
}