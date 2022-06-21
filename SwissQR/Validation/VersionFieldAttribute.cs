using System;

namespace SwissQR.Validation
{
    public class VersionFieldAttribute : ValidationAttribute
    {
        public override void Validate(object value, string fieldName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (fieldName is null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            var Parsed = TypeCheck<Version>(value);
            if (Parsed.Revision > 0 || Parsed.Build > 0)
            {
                throw new ValidationException(fieldName, $"Version cannot contain '{nameof(Version.Revision)}' or '{nameof(Version.Build)}' components");
            }
            if (Parsed.CompareTo(Version.Parse(Header.CurrentVersion)) != 0)
            {
                throw new ValidationException(fieldName, "Version 2.0 is the only supported version");
            }
        }
    }
}