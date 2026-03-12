using Niras.Jordflytning.IO.Fakturering.KMDOpus.Attributes;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Niras.Jordflytning.IO.Fakturering.KMDOpus
{
    public abstract class Line<T>
    {
        private readonly IConfigProvider _configProvider;

        private string Clean(string input)
        {
            return Regex.Replace(Regex.Replace(input, @"\r\n?|\n|\t", " "), @"\s+", " ").Trim();
        }

        protected string CreateLine(T owner)
        {
            var properties = typeof(T)
                .GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(FieldAttribute)))
                .OrderBy(p => ((FieldAttribute)p.GetCustomAttributes(typeof(FieldAttribute), true).Single()).Order)
                .ToArray();

            var line = new string[properties.Length];

            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var currentValue = property.GetValue(owner);
                try
                {                    
                    var fieldAttribute = (FieldAttribute)property.GetCustomAttributes(typeof(FieldAttribute), true).Single();

                    // Check for a config provided value, and set this if nothing else is set:
                    if (Attribute.IsDefined(property, typeof(ConfigEntryAttribute)) && EqualsDefault(property.PropertyType, currentValue))
                    {
                        var configAttribute = (ConfigEntryAttribute)property.GetCustomAttributes(typeof(ConfigEntryAttribute), true).Single();
                        currentValue = _configProvider.GetValue(property.PropertyType, configAttribute.ConfigKey);
                    }
                    line[i] = Clean(fieldAttribute.CreateString(currentValue));
                }
                catch (Exception ex) 
                {
                    line[i] = ex.Message;
                }
            }

            return string.Join(";", line);
        }

        private static bool EqualsDefault(Type type, object current)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return null == current;
            if (type.IsValueType)
                return Activator.CreateInstance(type).Equals(current);
            return null == current;
        }

        protected Line(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }
    }
}