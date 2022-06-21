using SwissQR.Validation;
using System;
using System.Globalization;

namespace SwissQR
{
    public class CcyAmt : Validated, IQRTransferrable
    {
        public const decimal MIN = 0.01M;
        public const decimal MAX = 9999999999.99M;

        [Decimal(2)]
        public decimal Amount { get; set; }

        [ValidEnum]
        public Currency Currency { get; set; }

        public override void Validate()
        {
            base.Validate();
            if (Amount < MIN || Amount > MAX)
            {
                throw new ValidationException(nameof(Amount), "Amount outside of permitted bounds");
            }
        }

        public string[] Export()
        {
            Validate();
            return new string[]
            {
                Amount.ToString(CultureInfo.InvariantCulture),
                Currency.ToString()
            };
        }

        public int GetFieldCount()
        {
            return 2;
        }

        public void Import(string[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value.Length != GetFieldCount())
            {
                throw new ArgumentException($"Expected {GetFieldCount()} fields but got {value.Length}");
            }
            var _tempAmount = Amount;
            var _tempCurrency = Currency;
            try
            {
                Amount = decimal.Parse(value[0], CultureInfo.InvariantCulture);
                Currency = Enum.Parse<Currency>(value[1]);
                Validate();
            }
            catch
            {
                Amount = _tempAmount;
                Currency = _tempCurrency;
                throw;
            }
        }
    }
}