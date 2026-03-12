using System;

namespace Niras.Jordflytning.IO.Fakturering
{
    public interface IConfigProvider
    {
        object GetValue(Type type, string key);
    }
}