using Niras.Jordflytning.Core.Models.datafordeler;
using Niras.Jordflytning.Core.Models.MatrikelOpslag;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    public interface IDawaOpslagBusiness
    {
        DawaClasses.EjerlavInfo[] EjerlavSoegning(string q);
        DawaClasses.Jordstykke[] JordstykkeOpslag(string q, DawaClasses.EjerlavInfo[] ejerlavListe = null);
        MatrikelGeometri GetMatrikel(string ejerlavkode, string matrikelnummer);
        //DawaClasses.AdresseMini ReverseGeokodning(string x, string y); // Strenge for at undgå konverteringsballade
    }
}