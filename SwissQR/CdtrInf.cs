using SwissQR.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwissQR
{
    public class CdtrInf : Validated, IQRTransferrable
    {
        [IBAN, NotNull]
        public string IBAN { get; set; }

        [NotNull]
        public Cdtr Creditor { get; set; } = new Cdtr();

        public string[] Export()
        {
            Validate();
            var Exp = new List<string>() { IBAN };
            Exp.AddRange(Creditor.Export());
            return Exp.ToArray();
        }

        public int GetFieldCount()
        {
            return 1 + Creditor.GetFieldCount();
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

            string _tempIBAN = IBAN;
            Cdtr _tempCreditor = Creditor;
            try
            {
                IBAN = value[0];
                Creditor = new Cdtr();
                Creditor.Import(value.Skip(1).ToArray());
                Validate();
            }
            catch
            {
                IBAN = _tempIBAN;
                Creditor = _tempCreditor;
                throw;
            }
        }
    }
}