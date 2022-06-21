using SwissQR.Validation;
using System;

namespace SwissQR
{
    public class Cdtr : Validated, IQRTransferrable
    {
        [ValidEnum]
        public AddressType AddrType { get; set; }

        /// <summary>
        /// Name of account holder.
        /// Preferred format: "Firstname Lastname"
        /// </summary>
        [NotNull, Length(1, 70)]
        public string Name { get; set; }

        /// <summary>
        /// Structured: Road
        /// Combined: Road + Number (if any) + postbox (if any)
        /// </summary>
        [NotNull, Length(1, 70)]
        public string StrtNmOrAdrLine1 { get; set; }

        /// <summary>
        /// Structured: House number (max 16 characters)
        /// Combined: Zipcode + Town
        /// </summary>
        [Length(70)]
        public string BldgNbOrAdrLine2 { get; set; }

        /// <summary>
        /// Structured: Post code
        /// Combined: Empty
        /// </summary>
        [Length(16)]
        public string PostCode { get; set; }

        [Length(35)]
        /// <summary>
        /// Structured: Town name
        /// Combined: Empty
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// Two character country code
        /// </summary>
        [NotNull, Length(2, 2)]
        public string Country { get; set; } = "CH";

        public override void Validate()
        {
            base.Validate();
            //Do dynamic validation based on address type
            if (AddrType == AddressType.S)
            {
                if (BldgNbOrAdrLine2 != null && BldgNbOrAdrLine2.Length > 16)
                {
                    throw new ValidationException(nameof(BldgNbOrAdrLine2), "Field can be at most 16 character in structured mode");
                }
            }
            else if (AddrType == AddressType.K)
            {
                if (string.IsNullOrEmpty(BldgNbOrAdrLine2))
                {
                    throw new ValidationException(nameof(BldgNbOrAdrLine2), "Field is required in combined address mode");
                }
                if (!string.IsNullOrEmpty(PostCode))
                {
                    throw new ValidationException(nameof(PostCode), "Field must not be supplied in combined address mode");
                }
                if (!string.IsNullOrEmpty(Town))
                {
                    throw new ValidationException(nameof(PostCode), "Field must not be supplied in combined address mode");
                }
            }
            else
            {
                throw new NotImplementedException($"Address type not implemented: {AddrType}");
            }
        }

        public string[] Export()
        {
            Validate();
            return new string[]
            {
                AddrType.ToString(),
                Name,
                StrtNmOrAdrLine1,
                BldgNbOrAdrLine2,
                PostCode,
                Town,
                Country
            };
        }

        public int GetFieldCount()
        {
            return GetType().GetFields().Length;
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

            var _tempName = Name;
            var _tempAddrType = AddrType;
            var _tempStrtNmOrAdrLine1 = StrtNmOrAdrLine1;
            var _tempBldgNbOrAdrLine2 = BldgNbOrAdrLine2;
            var _tempPostCode = PostCode;
            var _tempTown = Town;
            var _tempCountry = Country;

            try
            {
                Name = value[0];
                AddrType = Enum.Parse<AddressType>(value[1]);
                StrtNmOrAdrLine1 = value[2];
                BldgNbOrAdrLine2 = value[3];
                PostCode = value[4];
                Town = value[5];
                Country = value[6];
                Validate();
            }
            catch
            {
                Name = _tempName;
                AddrType = _tempAddrType;
                StrtNmOrAdrLine1 = _tempStrtNmOrAdrLine1;
                BldgNbOrAdrLine2 = _tempBldgNbOrAdrLine2;
                PostCode = _tempPostCode;
                Town = _tempTown;
                Country = _tempCountry;
                throw;
            }

        }
    }
}