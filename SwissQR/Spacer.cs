using System;
using System.Linq;

namespace SwissQR
{
    public class Spacer : IQRTransferrable
    {
        public int FieldCount;

        public Spacer(int fieldCount)
        {
            if (fieldCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldCount));
            }
            FieldCount = fieldCount;
        }

        public string[] Export()
        {
            return Enumerable
                .Range(0, FieldCount)
                .Select(m => string.Empty)
                .ToArray();
        }

        public int GetFieldCount()
        {
            return FieldCount;
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
            //NOOP
        }
    }
}