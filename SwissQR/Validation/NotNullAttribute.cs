namespace SwissQR.Validation
{
    public class NotNullAttribute : ValidationAttribute
    {
        public override void Validate(object value, string fieldName)
        {
            if(value is null)
            {
                throw new ValidationException(fieldName, "This value cannot be null");
            }
        }
    }
}
