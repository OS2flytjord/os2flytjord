
namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
  public interface IMailBusiness
  {
    void SendEmail(string emailSubject, string emailContent, string emailTo);
  }
}
