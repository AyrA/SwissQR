using QRCoder;
using SwissQR.Validation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SwissQR
{
    public class QR : Validated, IQRTransferrable
    {
        private const string CRLF = "\r\n";
        private const string SWISS_CROSS = "iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFAQMAAAC3ob" +
            "SmAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAGUExURQAAAP///6XZn90AA" +
            "AAJcEhZcwAADsMAAA7DAcdvqGQAAAASSURBVBjTY2BgUGAoYFBgYAAAA3oAsZvq6LIA" +
            "AAAASUVORK5CYII=";
        private const string SWISS_CROSS_RED = "iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFAQMAAA" +
            "C3obSmAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAGUExURf8AAP///0EdN" +
            "BEAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAASSURBVBjTY2BgUGAoYFBgYAAAA3oAsZvq" +
            "6LIAAAAASUVORK5CYII=";
        [NotNull]
        public Header Header { get; set; } = new Header();

        [NotNull]
        public CdtrInf Creditor { get; set; } = new CdtrInf();

        [NotNull]
        public Spacer UltimateCreditor { get; set; } = new Spacer(7);

        [NotNull]
        public CcyAmt CurrencyAmount { get; set; } = new CcyAmt();

        [NotNull]
        public Cdtr UltimateDebitor { get; set; } = new Cdtr();

        [NotNull]
        public RmtInf PaymentReference { get; set; } = new RmtInf();

        [Length(100)]
        public string AltPmt1 { get; set; }

        [Length(100)]
        public string AltPmt2 { get; set; }

        public string[] Export()
        {
            Validate();
            var Exp = new List<string>(Header.Export());
            Exp.AddRange(Creditor.Export());
            Exp.AddRange(UltimateCreditor.Export());
            Exp.AddRange(CurrencyAmount.Export());
            Exp.AddRange(UltimateDebitor.Export());
            Exp.AddRange(PaymentReference.Export());
            Exp.Add(AltPmt1);
            Exp.Add(AltPmt2);
            return Exp.ToArray();
        }

        public int GetFieldCount()
        {
            return Header.GetFieldCount() +
                Creditor.GetFieldCount() +
                UltimateCreditor.GetFieldCount() +
                CurrencyAmount.GetFieldCount() +
                PaymentReference.GetFieldCount() +
                UltimateDebitor.GetFieldCount();
        }

        public void Import(string[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value.Length < GetFieldCount())
            {
                throw new ArgumentException($"Expected at least {GetFieldCount()} fields but got {value.Length}");
            }

            var _tempHeader = Header;
            var _tempCreditorInfo = Creditor;
            var _tempUltimateCreditor = UltimateCreditor;
            var _tempCurrencyAmount = CurrencyAmount;
            var _tempUltimateDebitor = UltimateDebitor;
            var _tempPaymentReference = PaymentReference;
            var _tempAltPmt1 = AltPmt1;
            var _tempAltPmt2 = AltPmt2;

            try
            {
                Header = new Header();
                Header.Import(value.Take(Header.GetFieldCount()).ToArray());

                value = value.Skip(Header.GetFieldCount()).ToArray();
                Creditor = new CdtrInf();
                Creditor.Import(value.Take(Creditor.GetFieldCount()).ToArray());

                value = value.Skip(Creditor.GetFieldCount()).ToArray();
                UltimateCreditor = new Spacer(7);
                UltimateCreditor.Import(value.Take(UltimateCreditor.GetFieldCount()).ToArray());

                value = value.Skip(UltimateCreditor.GetFieldCount()).ToArray();
                CurrencyAmount = new CcyAmt();
                CurrencyAmount.Import(value.Take(CurrencyAmount.GetFieldCount()).ToArray());

                value = value.Skip(CurrencyAmount.GetFieldCount()).ToArray();
                UltimateDebitor = new Cdtr();
                UltimateDebitor.Import(value.Take(UltimateDebitor.GetFieldCount()).ToArray());

                value = value.Skip(UltimateDebitor.GetFieldCount()).ToArray();
                PaymentReference = new RmtInf();
                PaymentReference.Import(value.Take(PaymentReference.GetFieldCount()).ToArray());

                value = value.Skip(PaymentReference.GetFieldCount()).ToArray();
                AltPmt1 = AltPmt2 = null;
                if (value.Length > 0)
                {
                    AltPmt1 = value[0];
                }
                if (value.Length > 1)
                {
                    AltPmt1 = value[1];
                }

                Validate();
            }
            catch
            {
                Header = _tempHeader;
                Creditor = _tempCreditorInfo;
                UltimateCreditor = _tempUltimateCreditor;
                CurrencyAmount = _tempCurrencyAmount;
                UltimateDebitor = _tempUltimateDebitor;
                PaymentReference = _tempPaymentReference;
                AltPmt1 = _tempAltPmt1;
                AltPmt2 = _tempAltPmt2;
                throw;
            }
        }

        public Bitmap GetQR(bool Colored)
        {
            using QRCodeGenerator qrGenerator = new QRCodeGenerator();
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(string.Join(CRLF, Export()), QRCodeGenerator.ECCLevel.M);
            using QRCode qrCode = new QRCode(qrCodeData);
            using var IN = new MemoryStream(Convert.FromBase64String(Colored ? SWISS_CROSS_RED : SWISS_CROSS));
            using var Cross = (Bitmap)Image.FromStream(IN);
            //Expand the image with super fast crisp drawing
            using var Scaled = new Bitmap(100, 100);
            using Graphics G = Graphics.FromImage(Scaled);
            G.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            G.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            G.DrawImage(Cross, 0.0f, 0.0f, 100f, 100f);
            //Render final code
            return qrCode.GetGraphic(20, Color.Black, Color.White, Scaled, 15, 20, true);
        }
    }
}
