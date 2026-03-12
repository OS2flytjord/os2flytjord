using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Extensions
{
    public static class PersonExtensions
    {
        public static string FullName(this Person person)
        {
            if (person == null)
                return string.Empty;
            else if (!string.IsNullOrWhiteSpace(person.Navn) && !string.IsNullOrWhiteSpace(person.Efternavn))
                return $"{person.Navn} {person.Efternavn}";
            else if (!string.IsNullOrWhiteSpace(person.Navn))
                return person.Navn;
            else if (!string.IsNullOrWhiteSpace(person.Efternavn))
                return person.Efternavn;
            return string.Empty;
        }
    }
}