using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business
{

  public enum EnumStatusAnmeldelse
  {
    Oprettet = 1,
    Gemt = 2,
    Afsendt =3,
    BetalerAccepteretBetalingen=4,
    BetalerAfviserBetalingen=5,
    UnderbehandlingAfKommunen=6,
    GodkendtAfKommunen=7,
    AfvistAfKommunen=8,
    GodkendtAfJordmodtager=9,
    AfvistAfJordmodtager = 10,
    Afsluttet=11,
    RevideretAfAnmelder =12
  }


  public interface IStatusAnmeldelseBusiness
  {
    StatusAnmeldelse CreateStatus(EnumStatusAnmeldelse type, Person udfoertAfPerson);
    IList<StatusAnmeldelse> Read(Guid anmeldelseId);
    
    /// <summary>
    /// Altefter rolle skal denne funktion returnere statustyper svarede til rettighederne.
    /// Sagsbehandler må fx sætte typerne Underbehandling, Godkedt, Afvis
    /// Jordmodtagerens miljømedarbjederen må sætte typen "Godkendt af jordmodtager"
    /// </summary>
    /// <param name="personId"></param>
    /// <returns></returns>
    IList<StatusAnmeldelseType> GetStatusTypesForKommune();

    IList<StatusAnmeldelseType> GetStatusTypesForJordmodtager();

    IList<StatusAnmeldelseType> GetStatusTypesForBetaler();


    /// <summary>
    /// Giver den sidste status som ikke er gem.
    /// </summary>
    /// <param name="statusAnmeldelses"></param>
    /// <returns></returns>
    StatusAnmeldelseType GetLastStatusForKommune(ICollection<StatusAnmeldelse> statusAnmeldelses);

    /// <summary>
    /// Giver den sidste status for jordmodtager som ikke er gem.
    /// </summary>
    /// <param name="statusAnmeldelses"></param>
    /// <returns></returns>
    StatusAnmeldelseType GetLastStatusForJordmodtager(ICollection<StatusAnmeldelse> statusAnmeldelses);

    /// <summary>
    /// Giver den sidste status for betaler som ikke er gem.
    /// </summary>
    /// <param name="statusAnmeldelses"></param>
    /// <returns></returns>
    StatusAnmeldelseType GetLastStatusForBetaler(ICollection<StatusAnmeldelse> statusAnmeldelses);

  }


}
