using Niras.Jordflytning.Core.Models.MatrikelOpslag;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    public interface IDawaOpslagBusiness
    {
        DawaClasses.EjerlavInfo[] EjerlavSoegning(string q);
        DawaClasses.Jordstykke[] JordstykkeOpslag(string q, DawaClasses.EjerlavInfo[] ejerlavListe = null);
        DawaClasses.AdresseMini ReverseGeokodning(string x, string y); // Strenge for at undgå konverteringsballade
    }
}