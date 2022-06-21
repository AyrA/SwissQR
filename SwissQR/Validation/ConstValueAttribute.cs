namespace SwissQR.Validation
{
    internal class ConstValueAttribute : ValidationAttribute
    {
        public object Value { get; }

        public ConstValueAttribute(object value)
        {
            Value = value;
        }

        public override void Validate(object value, string fieldName)
        {
            if (value == null)
            {
                if (Value == null)
                {
                    return;
                }
                throw new ValidationException(fieldName, $"Value must be '{Value}' but is null");
            }
            if (!value.Equals(Value))
            {
                throw new ValidationException(fieldName, $"Value must be '{Value}' but is '{value}'");
            }
        }
    }
}