using SwissQR.Validation;
using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var QRCode = new SwissQR.QR()
            {
                Creditor = new SwissQR.CdtrInf()
                {
                    Creditor = new SwissQR.Cdtr()
                    {
                        AddrType = SwissQR.AddressType.S,
                        Name = "Der Empfänger",
                        StrtNmOrAdrLine1 = "Musterstrasse",
                        BldgNbOrAdrLine2 = "1234",
                        PostCode = "6000",
                        Town = "Luzern",
                        Country = "CH"
                    },
                    IBAN = "CH3600000000000000000"
                },
                UltimateDebitor = new SwissQR.Cdtr()
                {
                    AddrType = SwissQR.AddressType.K,
                    Name = "Der Absender",
                    StrtNmOrAdrLine1 = "Musterstrasse 9876",
                    BldgNbOrAdrLine2 = "8000 Zürich"
                },
                CurrencyAmount = new SwissQR.CcyAmt()
                {
                    Amount = 1234.56M,
                    Currency = SwissQR.Currency.CHF
                },
                PaymentReference = new SwissQR.RmtInf()
                {
                    Message = "Custom message",
                    ReferenceType = SwissQR.ReferenceType.NON
                }
            };
            try
            {
                using var img1 = QRCode.GetQR(true);
                img1.Save("qr.png");
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = "qr.png"
                });
                Console.WriteLine("qr.png created");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine(@"Validation failed:
Type: {0}
Field: {1}
Message: {2}", ex.GetType(), ex.FieldName, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Validation failed:
Type: {0}
Message: {1}", ex.GetType(), ex.Message);
            }
            Console.WriteLine("#END");
        }
    }
}
