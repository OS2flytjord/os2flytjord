using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using System;

namespace Niras.Jordflytning.IO.Fakturering
{
    public class ConfigProvider : IConfigProvider
    {
        private readonly IKonfigBusiness _konfigBusiness;
        private readonly Guid _kommuneId;

        public ConfigProvider(IKonfigBusiness konfigBusiness, Guid kommuneId)
        {
            _konfigBusiness = konfigBusiness;
            _kommuneId = kommuneId;
        }

        public object GetValue(Type type, string key)
        {
            var cfg = _konfigBusiness.ReadKommuneKonfig(key, _kommuneId);
            var isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (!string.IsNullOrWhiteSpace(cfg))
            {
                if (type == typeof(string))
                    return cfg;

                if (isNullable)
                    return Convert.ChangeType(cfg, Nullable.GetUnderlyingType(type));

                return Convert.ChangeType(cfg, type);
            }

            if (type.IsValueType && !isNullable)
                return Activator.CreateInstance(type);
            return null;
        }
    }
}