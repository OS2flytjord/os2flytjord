using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public static class Extensions
    {

        private static readonly Dictionary<EnumStatusAnmeldelse, string> _statusDescriptions = new Dictionary<EnumStatusAnmeldelse, string>();

        public static string Navn(this EnumStatusAnmeldelse status)
        {
            if (_statusDescriptions.Count == 0)
            {
                // Find og cache description attributter på status enum
                foreach (EnumStatusAnmeldelse entry in Enum.GetValues(typeof(EnumStatusAnmeldelse)))
                {
                    string text;
                    var fi = entry.GetType().GetField(entry.ToString());
                    if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
                        text = attributes.First().Description;
                    else
                        text = entry.ToString();
                    _statusDescriptions.Add(entry, text);
                }
            }
            return _statusDescriptions[status];
        }


    }
}
