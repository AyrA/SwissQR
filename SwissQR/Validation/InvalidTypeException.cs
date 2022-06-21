using System;

namespace SwissQR.Validation
{
    public class InvalidTypeException : Exception
    {
        private const string INVALID_TYPE = "Invalid type of argument. Expecting '{0}' but got '{1}'";
        public InvalidTypeException(Type tIs, Type tExpect) : base(string.Format(INVALID_TYPE, tExpect, tIs))
        {
            if (tExpect is null)
            {
                throw new ArgumentNullException(nameof(tExpect));
            }
            if (tIs == tExpect)
            {
                throw new ArgumentException($"Types supplied to {nameof(InvalidTypeException)} are identical");
            }
        }
    }
}
