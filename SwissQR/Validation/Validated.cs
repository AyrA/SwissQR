using System;
using System.Collections.Generic;
using System.Reflection;

namespace SwissQR.Validation
{
    public abstract class Validated
    {
        /// <summary>
        /// Fields and properties to validate
        /// </summary>
        /// <remarks>
        /// Add <see cref="BindingFlags.NonPublic"/>
        /// to also validate private fields.
        /// Note that this will validate public properties twice
        /// </remarks>
        const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

        public virtual void Validate()
        {
            var T = GetType();
            var Fields = T.GetFields(Flags);
            var Props = T.GetProperties(Flags);
            foreach (var F in Fields)
            {
                Validate(F.GetCustomAttributes(), F.Name, F.GetValue(this));
            }
            foreach (var P in Props)
            {
                Validate(P.GetCustomAttributes(), P.Name, P.GetValue(this));
            }
        }

        private void Validate(IEnumerable<Attribute> attributes, string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            foreach (var attr in attributes)
            {
                if (attr is ValidationAttribute attribute)
                {
                    attribute.Validate(value, name);
                }
            }
            //Recursively validate values
            if(value is Validated validated)
            {
                validated.Validate();
            }

            if(value is string s)
            {
                if(s.Contains('\r') || s.Contains('\n'))
                {
                    throw new ValidationException(name, "Value cannot contain CR, LF, or both");
                }
            }
        }
    }
}
