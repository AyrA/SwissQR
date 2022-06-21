using System;
using System.Linq;

namespace SwissQR
{
    public class Spacer : IQRTransferrable
    {
        public int FieldCount { get; }
        public bool ForceEmptyOnImport { get; }


        public Spacer(int fieldCount, bool forceEmptyOnImport)
        {
            if (fieldCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(fieldCount));
            }
            FieldCount = fieldCount;
            ForceEmptyOnImport = forceEmptyOnImport;
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
            if (ForceEmptyOnImport)
            {
                if (!value.All(string.IsNullOrEmpty))
                {
                    throw new ArgumentException($"{nameof(ForceEmptyOnImport)} is set but at least one value is not null or empty");
                }
            }
        }
    }
}