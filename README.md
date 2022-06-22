# SwissQR

QR code generator for the new swiss QR bill

This library (as opposed to most others) validates your inputs according to the swiss QR payment standard, and will refuse to export data or generate a QR code before all mistakes are fixed.
This makes it impossible to generate invalid bills.

## Installation

Download the latest DLL from the releases section and reference it in your project.

## Usage

*Also see example project*

Simply instantiate an instance of `SwissQR.QR` and populate all fields as necessary.
Then either use `.Export()` to get the raw data for the QR code to pass into another tool,
or use the `.GetQR(bool)` function to create a QR code directly.

```C#
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
	using var img = QRCode.GetQR(true);
	img.Save("qr.png");
	//Open image
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
```
