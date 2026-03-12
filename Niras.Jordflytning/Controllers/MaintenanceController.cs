using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using System;
using System.Web.Mvc;
using Niras.Jordflytning.Library.Logging;
using System.Configuration;
using System.IO;
using System.Net;

namespace Niras.Jordflytning.Controllers
{

    [AllowAnonymous]
    public class MaintenanceController : Controller
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger(typeof(MaintenanceController).FullName);

        private readonly IAnmeldelserBusiness _anmeldelserBusiness;

        public MaintenanceController(IAnmeldelserBusiness anmeldelserBusiness)
        {
            _anmeldelserBusiness = anmeldelserBusiness;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            // The [appsettings | environment variable] "MaintenanceKey" is required on the server, and as a user provided header

            base.OnAuthorization(filterContext);
            var systemKey = ConfigurationManager.AppSettings["MaintenanceKey"];
            var userKey = filterContext.HttpContext?.Request?.Headers["MaintenanceKey"] ?? null;
            if (string.IsNullOrWhiteSpace(systemKey) || string.IsNullOrWhiteSpace(userKey) || !systemKey.Equals(userKey, StringComparison.OrdinalIgnoreCase))
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public ActionResult AfslutGamleAnmeldelser(int uger)
        {
            try
            {
                _anmeldelserBusiness.AfslutGamleAnmeldelser(uger);
            }
            catch (Exception e)
            {
                Logger.LogException($"Fejl {nameof(AfslutGamleAnmeldelser)}, {nameof(uger)}: " + uger, e);
                throw;
            }
            return new EmptyResult();
        }
                        
        [HttpPost]
        public ActionResult RydGamleZipFiler(int timer)
        {
            try
            {
                var zipArchivePath = ConfigurationManager.AppSettings["ZipArchivePath"];
                var dir = new DirectoryInfo(zipArchivePath);
                var time = DateTime.UtcNow.AddHours(-24);
                if (dir.Exists)
                {
                    foreach (var file in dir.GetFiles())
                    {
                        if (file.LastWriteTimeUtc < time)
                            file.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException($"Fejl {nameof(RydGamleZipFiler)}, {nameof(timer)}: " + timer, e);
                throw;
            }
            return new EmptyResult();
        }

    }
}
