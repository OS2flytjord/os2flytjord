using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Niras.Jordflytning.IO.Fakturering.KMDOpus.Attributes
{
    public enum NumberModes
    {
        Integer,
        Decimal,
        StringAsInteger,
        Long
    }

    public enum PaddingModes
    {
        Fill,
        None
    }

    public abstract class FieldAttribute : Attribute
    {
        protected static readonly CultureInfo Culture;

        private readonly int _length;
        private readonly bool _required;
        private readonly char? _leftChar;
        private readonly char? _rightChar;

        public int Order { get; private set; }

        static FieldAttribute()
        {
            Culture = new CultureInfo("da-DK");
        }

        public FieldAttribute(int length, bool required, char? leftChar, char? rightChar, int order)
        {
            _length = length;
            _required = required;
            _leftChar = leftChar;
            _rightChar = rightChar;
            Order = order;
        }

        protected string CreateString<T>(T value, Func<T, string> format)
        {
            var t = typeof(T);
            if (_required && ((Nullable.GetUnderlyingType(t) != null && value.Equals(default(T))) || (t == typeof(string) && string.IsNullOrWhiteSpace(value as string))))
                throw new ArgumentException("A value is required!", nameof(value));

            if (!_required && (value == null || value.Equals(default(T))))
                return string.Empty;

            var fv = (
                value == null || value.Equals(default(T)) ?
                string.Empty :
                format(value).Replace(";", string.Empty) ?? string.Empty
            );
            var s = fv.Substring(0, _length > fv.Length ? fv.Length : _length);

            if (_leftChar != null)
                return s.PadLeft(_length, _leftChar.Value);
            if (_rightChar != null)
                return s.PadRight(_length, _rightChar.Value);
            return s;
        }
        public abstract string CreateString(object value);

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TextAttribute : FieldAttribute
    {
        public TextAttribute(int length, [CallerLineNumber] int order = 0) : base(length, true, null, null, order) { }

        //public TextAttribute(int length, [CallerLineNumber] int order = 0) : base(length, true, null, ' ', order) { }

        //public TextAttribute(int length, PaddingModes padding, [CallerLineNumber] int order = 0) : base(length, true, null, padding == PaddingModes.Fill ? ' ' : (char?)null, order) { }

        public override string CreateString(object value) => CreateString(value as string, (v) => v);
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalTextAttribute : FieldAttribute
    {
        public OptionalTextAttribute(int length, [CallerLineNumber] int order = 0) : base(length, false, null, null, order) { }

        //public OptionalTextAttribute(int length, [CallerLineNumber] int order = 0) : base(length, false, null, ' ', order) { }

        //public OptionalTextAttribute(int length, PaddingModes padding, [CallerLineNumber] int order = 0) : base(length, false, null, padding == PaddingModes.Fill ? ' ' : (char?)null, order) { }

        public override string CreateString(object value) => CreateString(value as string, (v) => v);
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NumberAttribute : FieldAttribute
    {
        private readonly NumberModes _mode;

        public NumberAttribute(int length, [CallerLineNumber] int order = 0) : base(length, true, '0', null, order)
        {
            _mode = NumberModes.Integer;
        }

        public NumberAttribute(int length, NumberModes mode, [CallerLineNumber] int order = 0) : base(length, true, '0', null, order)
        {
            _mode = mode;
        }

        public NumberAttribute(int length, NumberModes mode, PaddingModes padding, [CallerLineNumber] int order = 0) : base(length, true, padding == PaddingModes.Fill ? '0' : (char?)null, null, order)
        {
            _mode = mode;
        }

        protected NumberAttribute(int length, bool required, NumberModes mode, PaddingModes padding, int order) : base(length, required, '0', null, order)
        {
            _mode = mode;
        }

        public override string CreateString(object value)
        {
            switch (_mode)
            {
                case NumberModes.StringAsInteger:
                    return CreateString(decimal.TryParse(value as string, out var pv) ? pv : (decimal?)null, (v) => v.Value.ToString(Culture));
                case NumberModes.Decimal:
                    return CreateString(value as decimal?, (v) => v.Value.ToString(Culture));
                case NumberModes.Long:
                    return CreateString(value as Int64?, (v) => v.Value.ToString(Culture));
                case NumberModes.Integer:
                default:
                    return CreateString(value as int?, (v) => v.Value.ToString(Culture));
            }            
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalNumberAttribute : NumberAttribute
    {
        public OptionalNumberAttribute(int length, [CallerLineNumber] int order = 0) : base(length, false, NumberModes.Integer, PaddingModes.Fill, order) {}

        public OptionalNumberAttribute(int length, NumberModes mode, [CallerLineNumber] int order = 0) : base(length, false, mode, PaddingModes.Fill, order) {}
        
        public OptionalNumberAttribute(int length, NumberModes mode, PaddingModes padding, [CallerLineNumber] int order = 0) : base(length, false, mode, padding, order) {}
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateAttribute : FieldAttribute
    {
        public DateAttribute([CallerLineNumber] int order = 0) : base(10, true, null, null, order) {}

        protected DateAttribute(bool required, [CallerLineNumber] int order = 0) : base(10, required, null, null, order) {}

        public override string CreateString(object value)
        {
            return CreateString(value as DateTime?, (v) => v.Value.ToString("dd.MM.yyyy"));
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class OptionalDateAttribute : DateAttribute
    {
        public OptionalDateAttribute([CallerLineNumber] int order = 0) : base(false, order) { }
    }

    public class FormattedNumberAttribute : FieldAttribute
    {
        private readonly int _length;
        private readonly NumberModes _mode;
        private readonly string _format;

        public FormattedNumberAttribute(int length, NumberModes mode, string format, [CallerLineNumber] int order = 0) : this(length, mode, format, false, order) {}

        protected FormattedNumberAttribute(int length, NumberModes mode, string format, bool optional, int order = 0) : base(length, optional, null, null, order)
        {
            _length = length;
            _mode = mode;
            _format = format;
        }

        public override string CreateString(object value)
        {
            string result;
            switch (_mode)
            {
                case NumberModes.StringAsInteger:
                    result = CreateString(decimal.TryParse(value as string, out var pv) ? pv : (decimal?)null, (v) => string.Format(Culture, _format, v.Value));
                    break;
                case NumberModes.Decimal:
                    result = CreateString(value as decimal?, (v) => string.Format(Culture, _format, v.Value));
                    break;
                case NumberModes.Integer:
                default:
                    result = CreateString(value as int?, (v) => string.Format(Culture, _format, v.Value));
                    break;
            }
            
            if (result.Length > _length)
                throw new ArgumentException($"Formatted value is too big! ({result} is longer than the allowed {_length} characters)");
            
            return result;
        }
    }

    public class OptionalFormattedNumberAttribute : FormattedNumberAttribute
    {
        public OptionalFormattedNumberAttribute(int length, NumberModes mode, string format, [CallerLineNumber] int order = 0) : base(length, mode, format, true, order) {}
    }

    public class ConfigEntryAttribute : Attribute
    {
        public string ConfigKey { get; set; }
        
        public ConfigEntryAttribute(string configKey)
        {
            ConfigKey = configKey;
        }
    }

}