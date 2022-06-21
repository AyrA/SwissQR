using System;

namespace SwissQR
{
    public class Header
    {
        public static readonly string CurrentVersion = "2.0";

        [Length(3, 3)]
        public QRType QRType { get; } = QRType.SPC;
        public Version Version { get; } = new Version(CurrentVersion);
    }
}
