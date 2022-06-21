using System;

namespace SwissQR.Validation
{
    public class ValidationException : Exception
    {
        public string FieldName { get; }

        public ValidationException(string FieldName, string Message) : base(Message)
        {
            if (Message is null)
            {
                throw new ArgumentNullException(nameof(Message));
            }

            this.FieldName = FieldName ?? throw new ArgumentNullException(nameof(FieldName));
        }
    }
}
