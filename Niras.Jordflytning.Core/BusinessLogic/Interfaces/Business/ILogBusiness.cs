using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{

  public interface ILogBusiness
  {
    Log ChangesOnAnmeldelse(Anmeldelse foer, Anmeldelse efter);
    IList<Delta> ChangesOnOprindelsessted(Oprindelsessted foer, Oprindelsessted efter);
    IList<Delta> ChangesOnJord(Jord foer, Jord efter);
    IList<Delta> ChangesOnModtagerAnlaeg(ModtagerAnlaeg foer, ModtagerAnlaeg efter);
    IList<Delta> ChangesOnTransportoer(Transportoer foer, Transportoer efter);
    IList<Delta> ChangesOnBetaler(Betaler foer, Betaler efter);
    IList<Delta> ChangesOnKommunikation(Anmeldelse foer, Anmeldelse efter);
    Log ReadLog(Guid id);
  }
}
