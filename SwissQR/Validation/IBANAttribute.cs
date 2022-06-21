using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SwissQR.Validation
{
    public class IBANAttribute : ValidationAttribute
    {
        /// <summary>
        /// Generic IBAN regex
        /// </summary>
        private const string REGEX = @"^([A-Z]{2})(\d{2})([A-Z0-9]{1,30})$";

        public override void Validate(object value, string fieldName)
        {
            if (value == null)
            {
                return;
            }
            if (value is string iban)
            {
                if (iban != iban.ToUpper())
                {
                    throw new ValidationException(fieldName, "IBAN must be uppercase");
                }
                if (!iban.StartsWith("CH") && !iban.StartsWith("LI"))
                {
                    throw new ValidationException(fieldName, "Only CH and LI IBAN supported");
                }
                if (iban.Any(char.IsWhiteSpace))
                {
                    throw new ValidationException(fieldName, "IBAN must not contain whitespace");
                }
                if (iban.Length != 21)
                {
                    throw new ValidationException(fieldName, "IBAN must be 21 characters");
                }
                //Format validation
                if (!Regex.IsMatch(iban, REGEX))
                {
                    throw new ValidationException(fieldName, "IBAN format is invalid");
                }
                //Checksum validation
                //See: https://en.wikipedia.org/wiki/International_Bank_Account_Number#Validating_the_IBAN
                //1. Rearrange IBAN so the first 4 characters are at the end
                var digits = (iban[4..] + iban[..4])
                    //2. Replace range A-Z with range 10-36
                    .Select(ToNumber)
                    .ToArray();
                //3. Treat the IBAN as a long integer and compute modulo 97
                var bignum = System.Numerics.BigInteger.Parse(string.Concat(digits));
                var bigresult = (int)(bignum % System.Numerics.BigInteger.Parse("97"));

                //4. Check digit must be 1
                if (bigresult != 1)
                {
                    throw new ValidationException(fieldName, "IBAN checksum test failed");
                }
                //If you need to generate a check digit:
                //1. Replace the two digits following the country code with zeros
                //2. Calculate the checksum as per the algorithm above
                //3. Subtract the checksum from 98
                //4. Replace the zeros from step 1 with the result. Pad with zero if necessary
                return;
            }
            throw new ValidationException(fieldName, "Field is not a string");
        }

        private static int ToNumber(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            if (c >= 'A' && c <= 'Z')
            {
                return c - 'A' + 10;
            }
            if (c >= 'a' && c <= 'z')
            {
                return c - 'a' + 10;
            }
            throw new ArgumentException($"Not a valid IBAN character: '{c}'");
        }
    }
}
