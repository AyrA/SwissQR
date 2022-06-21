using System;

namespace SwissQR.Validation
{
    public abstract class ValidationAttribute : Attribute
    {
        public abstract void Validate(object value, string fieldName);

        public static T TypeCheck<T>(object o)
        {
            TypeCheck(o, typeof(T));
            return (T)o;
        }

        public static void TypeCheck(object o, Type tExpect)
        {
            if (o is null)
            {
                throw new InvalidTypeException(null, tExpect);
            }
            if(o.GetType() != tExpect)
            {
                throw new InvalidTypeException(o.GetType(), tExpect);
            }
        }
    }
}
