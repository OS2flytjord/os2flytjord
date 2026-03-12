using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class StatusAnmeldelseBusiness : GenericBusiness<StatusAnmeldelse>, IStatusAnmeldelseBusiness
    {
        private readonly IStatusAnmeldelseTypeRepository _statusAnmeldelseTypeRepo;
        private readonly IStatusAnmeldelseRepository _statusAnmeldelseRepo;
        private readonly IStatusBetalerBusiness _statusBetalerBusiness;
        private readonly IBetalerBusiness _betalerBusiness;

        public StatusAnmeldelseBusiness(
                IStatusAnmeldelseTypeRepository statusAnmeldelseTypeRepo,
                IStatusAnmeldelseRepository statusAnmeldelseRepo,
                IStatusBetalerBusiness statusBetalerBusiness,
          IBetalerBusiness betalerBusiness,
          IUnitOfWork uow) : base(statusAnmeldelseRepo, uow)
        {
            _statusAnmeldelseTypeRepo = statusAnmeldelseTypeRepo;
            _statusAnmeldelseRepo = statusAnmeldelseRepo;
            _statusBetalerBusiness = statusBetalerBusiness;
            _betalerBusiness = betalerBusiness;
        }

        public StatusAnmeldelse CreateStatus(EnumStatusAnmeldelse type, Person udfoertAfPerson)
        {
            var statusType = _statusAnmeldelseTypeRepo
                .Search(x => x.Kode == (short)type)
                .FirstOrDefault();
            
            var statusAnmeldelse = new StatusAnmeldelse();
            statusAnmeldelse.StatusAnmeldelseType = statusType;
            statusAnmeldelse.Tid = DateTime.Now;

            if (udfoertAfPerson != null)
                statusAnmeldelse.Person = udfoertAfPerson;

            return statusAnmeldelse;
        }

        public IList<StatusAnmeldelse> Read(Guid anmeldelseId)
        {
            return _statusAnmeldelseRepo
                .Search(x => x.Anmeldelse != null && x.Anmeldelse.Id == anmeldelseId)
                .OrderByDescending(x => x.Tid)
                .ToList();
        }

        public IList<StatusAnmeldelseType> GetStatusTypesForKommune()
        {
            var statusTypes = GetStatusTypeKodesForKommune();
            return _statusAnmeldelseTypeRepo
                .Search(x => statusTypes.Contains(x.Kode))
                .OrderBy(f => f.Kode)
                .ToList();
        }

        public IList<StatusAnmeldelseType> GetStatusTypesForJordmodtager()
        {
            var statusTypes = GetStatusTypeKodesForJordmodtager();
            return _statusAnmeldelseTypeRepo
                .Search(x => statusTypes.Contains(x.Kode))
                .OrderBy(f => f.Kode)
                .ToList();
        }

        public IList<StatusAnmeldelseType> GetStatusTypesForBetaler()
        {
            var statusTypes = GetStatusTypeKodesForBetaler();
            return _statusAnmeldelseTypeRepo
                .Search(x => statusTypes.Contains(x.Kode))
                .OrderBy(f => f.Kode)
                .ToList();
        }

        public IList<short> GetStatusTypeKodesForKommune()
        {
            return new List<short> {
                Convert.ToInt16(EnumStatusAnmeldelse.UnderbehandlingAfKommunen),
                Convert.ToInt16(EnumStatusAnmeldelse.GodkendtAfKommunen),
                Convert.ToInt16(EnumStatusAnmeldelse.AfvistAfKommunen)
            };
        }

        public IList<short> GetStatusTypeKodesForJordmodtager()
        {
            return new List<short> {
                Convert.ToInt16(EnumStatusAnmeldelse.JordmodtagerAfviserJorden),
                Convert.ToInt16(EnumStatusAnmeldelse.JordmodtagerAcceptererJorden)
            };
        }

        public IList<short> GetStatusTypeKodesForBetaler()
        {
            return new List<short> {
                Convert.ToInt16(EnumStatusAnmeldelse.BetalerAfviserBetalingen),
                Convert.ToInt16(EnumStatusAnmeldelse.BetalerAccepteretBetalingen)
            };
        }

        public IList<short> GetStatusTypeKodesForAnmelder()
        {
            return new List<short>
            {
                Convert.ToInt16(EnumStatusAnmeldelse.Oprettet),
                Convert.ToInt16(EnumStatusAnmeldelse.RevideretAfAnmelder),
                Convert.ToInt16(EnumStatusAnmeldelse.Afsendt)
            };
        }

        public IList<StatusAnmeldelseType> GetStatusTypesForAnmelder()
        {
            var statusTypes = GetStatusTypeKodesForAnmelder();
            return _statusAnmeldelseTypeRepo
                .Search(x => statusTypes.Contains(x.Kode))
                .OrderBy(f => f.Kode)
                .ToList();
        }

        public IDictionary<EnumStatusAnmeldelse, DateTime?> GetStatusListeMedTid(AnmeldelseEffektivStatus status)
        {
            var items = new Dictionary<EnumStatusAnmeldelse, DateTime?>
            {
                { EnumStatusAnmeldelse.Ukendt, null },
                { EnumStatusAnmeldelse.Oprettet, status.Oprettet },
                { EnumStatusAnmeldelse.Afsendt, status.Afsendt },
                { EnumStatusAnmeldelse.BetalerAccepteretBetalingen, status.BetalingAccepteret },
                { EnumStatusAnmeldelse.BetalerAfviserBetalingen, status.BetalingAfvist },
                { EnumStatusAnmeldelse.UnderbehandlingAfKommunen, status.UnderBehandling },
                { EnumStatusAnmeldelse.GodkendtAfKommunen, status.Godkendt },
                { EnumStatusAnmeldelse.AfvistAfKommunen, status.Afvist },
                { EnumStatusAnmeldelse.JordmodtagerAcceptererJorden, status.JordAccepteret },
                { EnumStatusAnmeldelse.JordmodtagerAfviserJorden, status.JordAfvist },
                { EnumStatusAnmeldelse.Afsluttet, status.Afsluttet },
                { EnumStatusAnmeldelse.RevideretAfAnmelder, status.Revideret },
                { EnumStatusAnmeldelse.AnmeldelseAktiv, status.Aktiv }
            };
            return items;
        }

        public EnumStatusAnmeldelse GetLastStatus(AnmeldelseEffektivStatus status)
        {
            var items = GetStatusListeMedTid(status);

            if (items.Any(x => x.Value != null))
                return items
                    .Where(x => x.Value != null)
                    .OrderByDescending(x => x.Value)
                    .First()
                    .Key;

            return EnumStatusAnmeldelse.Ukendt;
        }
        public bool GetLastStatus(AnmeldelseEffektivStatus status, IEnumerable<EnumStatusAnmeldelse> filter, out EnumStatusAnmeldelse statusType, out DateTime tid)
        {
            var items = GetStatusListeMedTid(status);

            if (items.Any(x => filter.Contains(x.Key) && x.Value != null))
            {
                var entry = items
                    .Where(x => filter.Contains(x.Key) && x.Value != null)
                    .OrderByDescending(x => x.Value)
                    .First();
                
                statusType = entry.Key;
                tid = entry.Value.Value;

                return true;
            }

            statusType = EnumStatusAnmeldelse.Ukendt;
            tid = DateTime.MinValue;
            
            return false;
        }

        public bool GetLastStatus(AnmeldelseEffektivStatus status, out EnumStatusAnmeldelse statusType, out DateTime tid)
        {
            var items = GetStatusListeMedTid(status);

            if (items.Any(x => x.Value != null))
            {
                var entry = items
                    .Where(x => x.Value != null)
                    .OrderByDescending(x => x.Value)
                    .First();

                statusType = entry.Key;
                tid = entry.Value.Value;

                return true;
            }

            statusType = EnumStatusAnmeldelse.Ukendt;
            tid = DateTime.MinValue;

            return false;
        }

        public string GetOprettetDato(AnmeldelseEffektivStatus status)
        {
            if (status.Oprettet.HasValue)
                return status.Oprettet.Value.ToShortDateString();

            return "";
        }

        public string GetAktivDato(AnmeldelseEffektivStatus status)
        {
            if (status.Aktiv.HasValue)
                return status.Aktiv.Value.ToShortDateString();

            return "";
        }

        public string GetAfvistDato(AnmeldelseEffektivStatus status)
        {
            if (status.Afvist.HasValue)
                return status.Afvist.Value.ToShortDateString();

            return "";
        }

        public EnumStatusAnmeldelse GetLastStatusInklStatusGemt(AnmeldelseEffektivStatus status)
        {
            var items = new dynamic[]
            {
                new { Key = EnumStatusAnmeldelse.Ukendt, Value = DateTime.MinValue.AddDays(1) },

                new { Key = EnumStatusAnmeldelse.Oprettet, Value = status.Oprettet ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.Gemt, Value = status.Gemt ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.Afsendt, Value = status.Afsendt ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.BetalerAccepteretBetalingen, Value = status.BetalingAccepteret ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.BetalerAfviserBetalingen, Value = status.BetalingAfvist ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.UnderbehandlingAfKommunen, Value = status.UnderBehandling ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.GodkendtAfKommunen, Value = status.Godkendt ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.AfvistAfKommunen, Value = status.Afvist ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.JordmodtagerAcceptererJorden, Value = status.JordAccepteret ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.JordmodtagerAfviserJorden, Value = status.JordAfvist ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.Afsluttet, Value = status.Afsluttet ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.RevideretAfAnmelder, Value = status.Revideret ?? DateTime.MinValue },
                new { Key = EnumStatusAnmeldelse.AnmeldelseAktiv, Value = status.Aktiv ?? DateTime.MinValue }
            };

            return items.OrderByDescending(x => x.Value).First().Key;
        }

        public EnumStatusAnmeldelse GetLastStatusTypeForKommune(AnmeldelseEffektivStatus status)
        {
            GetLastStatusTypeForKommune(status, out var statusType, out _);
            return statusType;
        }

        public bool GetLastStatusTypeForKommune(AnmeldelseEffektivStatus status, out EnumStatusAnmeldelse statusType, out DateTime tid)
        {
            return GetLastStatus(
                status,
                new EnumStatusAnmeldelse[]
                {
                    EnumStatusAnmeldelse.AfvistAfKommunen,
                    EnumStatusAnmeldelse.GodkendtAfKommunen,
                    EnumStatusAnmeldelse.UnderbehandlingAfKommunen
                }, 
                out statusType, 
                out tid
            );
        }

        public EnumStatusAnmeldelse GetLastStatusTypeForJordmodtager(AnmeldelseEffektivStatus status)
        {
            GetLastStatusTypeForJordmodtager(status, out var statusType, out _);
            return statusType;
        }

        public bool GetLastStatusTypeForJordmodtager(AnmeldelseEffektivStatus status, out EnumStatusAnmeldelse statusType, out DateTime tid)
        {
            return GetLastStatus(
                status,
                new EnumStatusAnmeldelse[]
                {
                    EnumStatusAnmeldelse.JordmodtagerAfviserJorden,
                    EnumStatusAnmeldelse.JordmodtagerAcceptererJorden
                },
                out statusType,
                out tid
            );
        }

        public EnumStatusAnmeldelse GetLastStatusTypeForBetaler(AnmeldelseEffektivStatus status)
        {
            GetLastStatusTypeForBetaler(status, out var statusType, out _);
            return statusType;
        }

        public bool GetLastStatusTypeForBetaler(AnmeldelseEffektivStatus status, out EnumStatusAnmeldelse statusType, out DateTime tid)
        {
            return GetLastStatus(
                status,
                new EnumStatusAnmeldelse[]
                {
                    EnumStatusAnmeldelse.BetalerAccepteretBetalingen,
                    EnumStatusAnmeldelse.BetalerAfviserBetalingen
                },
                out statusType,
                out tid
            );
        }


        public EnumStatusAnmeldelse GetLastStatusTypeForAnmelder(AnmeldelseEffektivStatus status)
        {
            GetLastStatusTypeForAnmelder(status, out var statusType, out _);
            return statusType;
        }

        public bool GetLastStatusTypeForAnmelder(AnmeldelseEffektivStatus status, out EnumStatusAnmeldelse statusType, out DateTime tid)
        {
            return GetLastStatus(
                status,
                new EnumStatusAnmeldelse[]
                {
                    EnumStatusAnmeldelse.RevideretAfAnmelder,
                    EnumStatusAnmeldelse.Afsendt,
                    EnumStatusAnmeldelse.Oprettet
                },
                out statusType,
                out tid
            );
        }

        public bool IsAnmeldelseAktiv(Anmeldelse anmeldelse)
        {
            return anmeldelse.AnmeldelseEffektivStatus.Aktiv.HasValue;
        }
        
        public bool IsAnmeldelseAktiv(AnmeldelseEffektivStatus status)
        {
            return status.Aktiv.HasValue;
        }

        public bool IsAnmeldelseIndsendtMenIkkeAktiv(AnmeldelseEffektivStatus status)
        {
            return status.Afsendt.HasValue && !status.Aktiv.HasValue && !status.Afsluttet.HasValue;
        }

        public bool IsAnmeldelseIkkeIndsendt(AnmeldelseEffektivStatus status)
        {
            return !status.Aktiv.HasValue && !status.Afsendt.HasValue && !status.Afsluttet.HasValue;
        }

        public bool IsAnmeldelseAfsluttet(Anmeldelse anmeldelse)
        {
            return anmeldelse.AnmeldelseEffektivStatus.Afsluttet.HasValue;
        }

        public bool IsAnmeldelseAfsluttet(AnmeldelseEffektivStatus status)
        {
            return status.Afsluttet.HasValue;
        }

        public bool IsAnmeldelseReadyToBeAktiv(Anmeldelse anmeldelse)
        {
            var readyToBeReleased = false;
            var usesJordFlyt = BrugerModtageAnlaegJordFlyt(anmeldelse);

            var hasBetalerAccepted = HasBetalerAccepteret(anmeldelse);
            var hasKommuneGodkendt = HasKommuneGodkendt(anmeldelse);
            var hasModtageAnlaegGodkendt = HasModtageAnlaegGodkendt(anmeldelse);
            var isBetalerGodkendt = IsBetalerGodkendt(anmeldelse);

            if (usesJordFlyt)
            {
                if (isBetalerGodkendt && hasBetalerAccepted && hasKommuneGodkendt && hasModtageAnlaegGodkendt)
                    readyToBeReleased = true;
            }
            else
            {
                if (hasKommuneGodkendt)
                    readyToBeReleased = true;
            }

            return readyToBeReleased;
        }

        public bool IsBetalerGodkendt(Anmeldelse anmeldelse)
        {
            var isGodkendt = false;
            if (anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.Jordmodtager != null && anmeldelse.BetalerId != null)
            {
                var jordmodtagerId = anmeldelse.ModtagerAnlaeg.Jordmodtager.Id;
                var betalerId = (Guid)anmeldelse.BetalerId;
                isGodkendt = _statusBetalerBusiness.IsBetalerGodkendtForJordmodtager(jordmodtagerId, betalerId);
            }
            return isGodkendt;
        }

        /// <summary>
        /// Hvis den seneste status er "accepterer", 
        /// så returner true ellers returner false
        /// </summary>
        public bool HasBetalerAccepteret(Anmeldelse anmeldelse)
        {
            // Betaler skal ikke godkende anmeldelsen, hvis ikke modtageranlægget anvender flytjord.
            if (anmeldelse != null && anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.AnvenderJF == false)
                return true;

            // Ellers tjekkes betaling
            return anmeldelse.AnmeldelseEffektivStatus.BetalingAccepteret.HasValue;
        }

        public bool MaaAnmeldelseRevideres(Anmeldelse anmeldelse)
        {
            if (anmeldelse != null)
            {
                var status = anmeldelse.AnmeldelseEffektivStatus;
                
                //Hvis anmeldelsen er afsluttet, aktiv eller godkendt
                if (status.Afsluttet.HasValue || status.Aktiv.HasValue || status.Godkendt.HasValue)
                    return true;
            }
            return false;
        }


        public bool HasKommuneGodkendt(Anmeldelse anmeldelse)
        {
            return anmeldelse.AnmeldelseEffektivStatus.Godkendt.HasValue;
        }

        public bool HasModtageAnlaegGodkendt(Anmeldelse anmeldelse)
        {
            return anmeldelse.AnmeldelseEffektivStatus.JordAccepteret.HasValue;
        }

        public bool IsBetalerSpaerret(Anmeldelse anmeldelse)
        {
            var isSpaerret = true;
            if (anmeldelse.ModtagerAnlaegId != null && anmeldelse.BetalerId != null)
            {
                if (anmeldelse.ModtagerAnlaeg.JordmodtagerId != null)
                {
                    var modtagerAnlaegId = (Guid)anmeldelse.ModtagerAnlaeg.JordmodtagerId;
                    var betalerId = (Guid)anmeldelse.BetalerId;
                    isSpaerret = _statusBetalerBusiness.IsBetalerSpaerretForJordmodtager(modtagerAnlaegId, betalerId);
                }
            }
            return isSpaerret;
        }

        public string GetFormattedStatusListForGrids(Anmeldelse anmeldelse)
        {
            var status = anmeldelse.AnmeldelseEffektivStatus;

            if (status.Afsluttet.HasValue)
                return "Afsluttet";

            if (status.Aktiv.HasValue)
                return "Aktiv";

            if (!status.Afsendt.HasValue)
                return "Gemt";

            var usesFlytjord = UsesFlytJord(anmeldelse);

            var statKommune = GetLastStatusTypeForKommune(anmeldelse.AnmeldelseEffektivStatus);

            var strStatKommune = "Kræver behandling";
            var strStatBetaler = "Kræver behandling";
            var strStatJordmodtager = "Kræver behandling";
            var strStatBogholder = "Kræver behandling";

            if (usesFlytjord)
            {
                var statBetaler = GetLastStatusTypeForBetaler(anmeldelse.AnmeldelseEffektivStatus);
                var statJordmodtager = GetLastStatusTypeForJordmodtager(anmeldelse.AnmeldelseEffektivStatus);
                if (statBetaler != EnumStatusAnmeldelse.Ukendt)
                    strStatBetaler = statBetaler.Navn();
                else
                {
                    if (_betalerBusiness.CheckAutoBetalerAccepterBetaling(anmeldelse))
                        strStatBetaler = "Kræver ingen behandling"; //Hvis betaler har bemyndiget anmelder eller hvis betaler er kernekunde hos Jordmodtager
                }

                if (statJordmodtager != null)
                    strStatJordmodtager = statJordmodtager.Navn();

                var godkendtStatus = _statusBetalerBusiness.GetGodkendtStatusForBogholder(anmeldelse);
                switch (godkendtStatus)
                {
                    case null:
                        strStatBogholder = "Kræver behandling";
                        break;

                    case true:
                        strStatBogholder = "Godkendt";
                        break;

                    case false:
                        strStatBogholder = "Afvist";
                        break;
                }
            }

            if (statKommune != null)    
                strStatKommune = statKommune.Navn();

            string returnlist;

            if (usesFlytjord)
            {
                returnlist =
                    "Betaler: '" + strStatBetaler + "'<br/>" +
                    "Kommune: '" + strStatKommune + "'<br/>" +
                    "Jordmodtager: '" + strStatJordmodtager + "'<br/>" +
                    "Bogholder: '" + strStatBogholder + "'";
            }
            else
            {
                returnlist =
                  "Kommune: '" + strStatKommune + "'";
            }
            return returnlist;
        }

        public bool UsesFlytJord(Anmeldelse anmeldelse)
        {
            var usesFlytJord = false;
            if (anmeldelse != null && anmeldelse.ModtagerAnlaeg != null)
            {
                usesFlytJord = anmeldelse.ModtagerAnlaeg.AnvenderJF;
            }
            return usesFlytJord;
        }

        public DateTime? GetTimeForOprettet(AnmeldelseEffektivStatus status)
        {
            return status.Oprettet;
        }

        public DateTime? GetTimeForIndsend(AnmeldelseEffektivStatus status)
        {
            return status.Afsendt;
        }

        private static bool BrugerModtageAnlaegJordFlyt(Anmeldelse anmeldelse)
        {
            var usesJordFlyt = false;
            if (anmeldelse != null && anmeldelse.ModtagerAnlaeg != null)
            {
                if (anmeldelse.ModtagerAnlaeg.AnvenderJF)
                {
                    usesJordFlyt = true;
                }
            }
            return usesJordFlyt;
        }

    }
}
