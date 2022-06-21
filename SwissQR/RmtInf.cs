using SwissQR.Validation;
using System;
using System.Linq;

namespace SwissQR
{
    public class RmtInf : Validated, IQRTransferrable
    {
        [ValidEnum]
        public ReferenceType ReferenceType { get; set; }

        [Length(27), AlphaNumeric]
        public string ReferenceValue { get; set; }

        [Length(140)]
        public string Message { get; set; }

        [ValidEnum]
        public Trailer Trailer { get; set; }

        [Length(140)]
        public string BillInfo { get; set; }

        public override void Validate()
        {
            base.Validate();
            if (ReferenceType == ReferenceType.NON && !string.IsNullOrEmpty(ReferenceValue))
            {
                throw new ValidationException(nameof(ReferenceValue), $"'{nameof(ReferenceValue)}' cannot be supplied when '{nameof(ReferenceType)}' is '{ReferenceType}'");
            }
            if (ReferenceType == ReferenceType.QRR)
            {
                if (ReferenceValue == null || ReferenceValue.Length != 27)
                {
                    throw new ValidationException(nameof(ReferenceValue), $"Reference value must be 27 characters if '{nameof(ReferenceType)}' is '{ReferenceType}'");
                }
                new NumericAttribute().Validate(ReferenceValue, nameof(ReferenceValue));
                var Checksum = ReferenceValue[..25].Select(m => m - '0').Sum() % 10;
                var Expected = ReferenceValue[^1] - '0';
                if (Checksum != Expected)
                {
                    throw new ValidationException(nameof(ReferenceValue), $"Checksum is invalid. Is '{Checksum}' but should be '{Expected}'");
                }
            }
            if (ReferenceType == ReferenceType.SCOR)
            {
                if (!string.IsNullOrEmpty(ReferenceValue))
                {
                    new LengthAttribute(5, 25).Validate(ReferenceValue, nameof(ReferenceValue));
                }
                else
                {
                    throw new ValidationException(nameof(ReferenceValue), $"Reference value must be 5-25 characters if '{nameof(ReferenceType)}' is '{ReferenceType}'");
                }
                //Reference is in valid IBAN format
                new IBANAttribute().Validate(ReferenceValue, nameof(ReferenceValue));
            }
        }

        public string[] Export()
        {
            Validate();
            return new string[]
            {
                ReferenceType.ToString(),
                ReferenceValue,
                Message,
                Trailer.ToString(),
                BillInfo
            };
        }

        public int GetFieldCount()
        {
            return 5;
        }

        public void Import(string[] value)
        {
            var _tempReferenceType = ReferenceType;
            var _tempReferenceValue = ReferenceValue;
            var _tempMessage = Message;
            var _tempTrailer = Trailer;
            var _tempBillInfo = BillInfo;

            try
            {
                ReferenceType = Enum.Parse<ReferenceType>(value[0]);
                ReferenceValue = value[1];
                Message = value[2];
                Trailer = Enum.Parse<Trailer>(value[3]);
                BillInfo = value[4];
                Validate();
            }
            catch
            {
                ReferenceType = _tempReferenceType;
                ReferenceValue = _tempReferenceValue;
                Message = _tempMessage;
                Trailer = _tempTrailer;
                BillInfo = _tempBillInfo;
                throw;
            }
        }
    }
}