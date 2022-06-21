using System;

namespace SwissQR.Validation
{
    public class DecimalAttribute : ValidationAttribute
    {
        public int Decimals { get; }

        public DecimalAttribute(int Decimals)
        {
            if (Decimals < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Decimals));
            }
            this.Decimals = Decimals;
        }

        public override void Validate(object value, string fieldName)
        {
            if(value is null)
            {
                return;
            }
            var d = Convert.ToDecimal(value);
            var parts = d.ToString().Split('.');
            if (parts.Length > 1)
            {
                if (parts[1].Length > Decimals)
                {
                    throw new ValidationException(fieldName, $"Too many decimals. Got '{parts[1].Length}' but expect at most '{Decimals}'");
                }
            }
        }
    }
}
