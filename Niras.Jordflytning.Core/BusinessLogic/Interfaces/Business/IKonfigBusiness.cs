using System;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
  public enum EnumKonfigKey
  {
    InfoOpretAnmeldelseStedKommuneSpecifik=1,
    AkutJordflytningEmail=2,
    InfoKommuneWwwVedrJordflyt = 3

  }

  public interface IKonfigBusiness
  {
    

    string ReadKommuneKonfig(string key,Guid kommuneId);
  }
}
