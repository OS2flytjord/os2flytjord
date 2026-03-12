using DocumentFormat.OpenXml.Office2010.Excel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ninject.Activation;
using Niras.Jordflytning.Areas.Backend.ViewModels;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IO.Fakturering;
using Niras.Jordflytning.IO.Fakturering.KMDOpus;
using Niras.Jordflytning.ViewModels;
using Niras.Jordflytning.ViewModels.Backend;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Niras.Jordflytning.Areas.Backend.Controllers
{
    public class FaktureringController : Controller
    {
        private readonly IAnmeldelserBusiness _anmeldelserBusiness;
        private readonly ISecurityProvider _securityProvider;
        private readonly IKommuneBusiness _kommuneBusiness;
        private readonly IBrugereBusiness _brugereBusiness;
        private readonly IKonfigBusiness _konfigBusiness;
        
        public FaktureringController(IAnmeldelserBusiness anmeldelserBusiness, ISecurityProvider securityProvider, IKommuneBusiness kommuneBusiness, IBrugereBusiness brugereBusiness, IKonfigBusiness konfigBusiness)
        {
            _anmeldelserBusiness = anmeldelserBusiness;
            _securityProvider = securityProvider;
            _kommuneBusiness = kommuneBusiness;
            _brugereBusiness = brugereBusiness;
            _konfigBusiness = konfigBusiness;
        }

        //http://localhost:52752/Backend/Fakturering/ExportKMDOpus

        public ActionResult ListFaktureringUdtraek([DataSourceRequest] DataSourceRequest request)
        {
            var list = new List<FaktureringUdtraekViewModel>();
            try
            {
                if (request.PageSize == 0)
                    request.PageSize = 10;

                var person = GetPerson();
                var kommuneId = GetKommuneId(person.Id);
                var udtraek = _anmeldelserBusiness.GetGebyrFaktureringUdtraekListe(kommuneId, (request.Page -1) * request.PageSize, request.PageSize);
                list.AddRange(udtraek.Select(FaktureringUdtraekViewModel.Create));
            }
            catch
            {
            }
            DataSourceResult result = list.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateFaktureringUdtraek(List<Guid> ids, string navn, DateTime skaeringsdato)
        {
            try
            {
                var person = GetPerson();
                var sagsbehandler = GetSagsbehandler(person.Id);
                var kommuneId = GetKommuneId(person.Id);
                var configProvider = new IO.Fakturering.ConfigProvider(_konfigBusiness, kommuneId);
                var exporter = new IO.Fakturering.KMDOpus.Exporter(kommuneId, configProvider);

                var anmeldelser = _anmeldelserBusiness
                    .GetKommunesFakturerbareAnmeldelser(kommuneId)
                    .Where(x => ids.Contains(x.Id))
                    .ToArray();
                var gebyrer = anmeldelser.Select(x => x.Gebyr);
                var anmeldeseIdListe = anmeldelser.Select(x => x.Id).ToArray();
                var udtraek = _anmeldelserBusiness.OpretAnmeldelseGebyrFaktureringUdtraek(anmeldeseIdListe, DateTime.Now, sagsbehandler.Id, !string.IsNullOrWhiteSpace(navn) ? navn : DateTime.Now.ToString("yyyyMMddhhmmss"), skaeringsdato);
                exporter.Export(udtraek, kommuneId);

                return Json(FaktureringUdtraekViewModel.Create(udtraek));
            }
            catch { }
            return Json(new { });
        }

        [HttpPost]
        public ActionResult EditFaktureringUdtraek(Guid id, string navn, bool overskriv)
        {
            try
            {
                var person = GetPerson();
                var sagsbehandler = GetSagsbehandler(person.Id);
                var kommuneId = GetKommuneId(person.Id);
                var configProvider = new IO.Fakturering.ConfigProvider(_konfigBusiness, kommuneId);
                var exporter = new IO.Fakturering.KMDOpus.Exporter(kommuneId, configProvider);
                var udtraek = _anmeldelserBusiness.RedigerAnmeldelseGebyrFaktureringUdtraek(id, !string.IsNullOrWhiteSpace(navn) ? navn : DateTime.Now.ToString("yyyyMMddhhmmss"));
                if (overskriv)
                    exporter.Export(udtraek, kommuneId);
                return Json(FaktureringUdtraekViewModel.Create(udtraek));
            }
            catch { }
            return Json(new { });
        }


        [HttpGet]
        public ActionResult DownloadFaktureringUdtraekKMDOpus(Guid id)
        {
            try
            {
                var person = GetPerson();
                var kommuneId = GetKommuneId(person.Id);
                var configProvider = new IO.Fakturering.ConfigProvider(_konfigBusiness, kommuneId);
                var exporter = new IO.Fakturering.KMDOpus.Exporter(kommuneId, configProvider);

                var file = exporter.GetFile(id);
                //if (!file.Exists)
                //{
                //    var udtraek = _anmeldelserBusiness.GetGebyrFaktureringUdtraek(id);
                //    file = exporter.Export(udtraek);
                //}
                byte[] fileBytes = System.IO.File.ReadAllBytes(file.FullName);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
            }
            catch { }
            return new HttpNotFoundResult();
        }

        [HttpPost]
        public ActionResult MarkAsDelivered(Guid id)
        {
            var success = false;
            try
            {
                var person = GetPerson();
                var kommuneId = GetKommuneId(person.Id);
                var sagsbehandler = _brugereBusiness.ReadSagsbehandler(person.Id);
                var tidspunkt = DateTime.Now;
                _anmeldelserBusiness.OverfoerAnmeldelseGebyrFaktureringUdtraek(id, tidspunkt, sagsbehandler.Id);

                success = true;
            }
            catch { }
            return Json(new { Success = success });
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var success = false;
            try
            {
                var person = GetPerson();
                var kommuneId = GetKommuneId(person.Id);
                var configProvider = new IO.Fakturering.ConfigProvider(_konfigBusiness, kommuneId);
                var exporter = new IO.Fakturering.KMDOpus.Exporter(kommuneId, configProvider);
                var file = exporter.GetFile(id);
                if (file.Exists)
                    file.Delete();
                _anmeldelserBusiness.SletAnmeldelseGebyrFaktureringUdtraek(id);
                success = true;
            }
            catch { }
            return Json(new { Success = success });
        }

        private Person GetPerson()
        {
            var b = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            var person = b.Person;
            return person;
        }

        private Sagsbehandler GetSagsbehandler(Guid userId)
        {
            var sagsbehandler = _brugereBusiness.ReadSagsbehandler(userId);
            return sagsbehandler;
        }

        private Guid GetKommuneId(Guid userId)
        {
            var id = _kommuneBusiness.GetKommuneIdByUserId(userId);
            return id;
        }

    }
}
