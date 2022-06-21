using SwissQR.Validation;
using System;

namespace SwissQR
{
    public class Header : Validated, IQRTransferrable
    {
        public static readonly string CurrentVersion = "2.0";

        [ValidEnum]
        public QRType QRType { get; set; } = QRType.SPC;
        
        [VersionField, NotNull]
        public Version Version { get; set; } = new Version(CurrentVersion);
        
        [ConstValue('1')]
        public char Coding { get; set; } = '1';

        public string[] Export()
        {
            Validate();
            return new string[]
            {
                QRType.ToString(),
                Version.ToString(2),
                Coding.ToString()
            };
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

            QRType _tempQr = QRType;
            Version _tempVersion = Version;
            char _tempCoding = Coding;
            try
            {
                QRType = Enum.Parse<QRType>(value[0]);
                Version = Version.Parse(value[1]);
                Coding = value[2][0];
                if (value[2].Length != 1)
                {
                    throw new ArgumentException($"{nameof(Coding)} requires 1 character but got {value[2].Length}");
                }
                Validate();
            }
            catch
            {
                //Undo partial import
                QRType = _tempQr;
                Version = _tempVersion;
                Coding = _tempCoding;
                throw;
            }
        }

        public int GetFieldCount()
        {
            return 3;
        }
    }
}
