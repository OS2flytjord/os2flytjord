using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
  public enum EnumAlarmType
  {
    MaengdeKoertJord = 1,
    AnmeldelseAktiv = 2
  }

  public interface IAlarmBusiness
  {
    IList<Alarm> CreateAlarmMaengdeKoertJord(Anmeldelse anmeldelse);

    void SendKoertJordAlarmer(Anmeldelse anmeldelse);

    void DeleteAlarm(Alarm alarm);
  }
}
