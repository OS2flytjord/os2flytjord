using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class ModtagerAnlaegBusiness : GenericBusiness<ModtagerAnlaeg>, IModtagerAnlaegBusiness
    {
        private readonly IModtagerAnlaegRepository _repo;
        private readonly IAnmeldelserRepository _anmeldelserRepo;
        private readonly IVognlaesRepository _vognlaesRepository;
        private readonly IGraensevaerdierRepository _graensevaerdierRepository;
        private readonly IJordmodtagerBusiness _jordmodtagerBusiness;
        private readonly IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
        private readonly IStatusAnmeldelseRepository _statusAnmeldelseRepository;
        private readonly IKodelisteBusiness _kodelisteBusiness;


        public ModtagerAnlaegBusiness(
          IModtagerAnlaegRepository repo,
          IAnmeldelserRepository anmeldelserRepo,
          IGraensevaerdierRepository graensevaerdierRepository,
          IJordmodtagerBusiness jordmodtagerBusiness,
          IVognlaesRepository vognlaesRepository,
		  IStatusAnmeldelseBusiness statusAnmeldelseBusiness, IStatusAnmeldelseRepository statusAnmeldelseRepository,
          IKodelisteBusiness kodelisteBusiness,
          IUnitOfWork unitOfWork)
          : base(repo, unitOfWork)
        {
            _repo = repo;
            _anmeldelserRepo = anmeldelserRepo;
            _graensevaerdierRepository = graensevaerdierRepository;
            _jordmodtagerBusiness = jordmodtagerBusiness;
            _vognlaesRepository = vognlaesRepository;
			_statusAnmeldelseBusiness = statusAnmeldelseBusiness;
            _statusAnmeldelseRepository = statusAnmeldelseRepository;
            _kodelisteBusiness = kodelisteBusiness;
        }

        public enum EnumFiltreringmetode
        {
            //I prioteret rækkefølge
            Tidligere = 1,
            Afstand = 2,
            Oprindelseskommune = 3,
            Alle = 4
        }

        public IList<ModtagerAnlaeg> ReadUsersAktiveModtageAnlaeg(Guid userId)
        {
            var anlaegList = new List<ModtagerAnlaeg>();
            var jordModtagerList = _jordmodtagerBusiness.GetJordModtagerListForUser(userId);
            foreach (var jordmodtager in jordModtagerList)
            {
                foreach (var anl in jordmodtager.ModtagerAnlaeg)
                {
                    if (anl.Aktiv)
                    anlaegList.Add(anl);
                }
            }
            return anlaegList;
        }

        public IList<ModtagerAnlaeg> ReadModtagerAnlaegBypassEf()
        {
            return _repo.ExecuteSqlQuery("EXEC [SP_SELECT_ModtageAnlaegList]").ToList();
        }

        public IList<ModtagerAnlaeg> ReadAktiveModtagerAnlaeg()
        {
            return (from k in _repo.Search(x => x.Aktiv)select k).ToList();
        }

	    public IList<ModtagerAnlaeg> ReadAktiveModtagerAnlaeg(bool anvenderJf, Guid jordKlassifikationType, bool affald)
	    {
		    List<ModtagerAnlaeg> list;
            var jordKlastype = _kodelisteBusiness.ReadJordKlassifikationType(jordKlassifikationType);
		    if (jordKlassifikationType != Guid.Empty)
		    {
                list = (from k in _repo.Search(x => x.Aktiv
                            && x.Jordmodtager != null
                            && x.JordKlassifikationType != null
                            && x.JordKlassifikationType.LandsdelTypeId == jordKlastype.LandsdelTypeId
                            && x.JordKlassifikationType.Priotering <= jordKlastype.Priotering
                            && x.AnmelderOprettetMidlertidigtAnlaeg == false)
                            /* 15.8.2017 TOK: Nedenstående linie er erstattet de 3 ovenstående, således modtageranlæg med en lavere klasse/kategori kan vælges */
                            //&& x.JordKlassifikationType.Id == jordKlassifikationType) 
                        select k).ToList();
		    }
		    else
		    {
                list = (from k in _repo.Search(x => x.Aktiv && x.Jordmodtager != null)select k).ToList();
		    }

		    if (anvenderJf)
			    list = list.Where(m => m.AnvenderJF).ToList();

		    if (affald)
			    list = list.Where(m => m.Affald).ToList();

		    return list;
	    }

	    public ModtagerAnlaeg ReadModtagerAnlaeg(Guid id)
        {
            return (from k in _repo.Search(x => x.Id == id) select k).FirstOrDefault();
        }

        public ModtagerAnlaeg ReadModtagerAnlaeg(int nummer)
        {
            return (from k in _repo.Search(x => x.Nummer == nummer) select k).FirstOrDefault();
        }

        public IList<ModtagerAnlaeg> ReadTidligereAktiveAnvendteModtagerAnlaeg(Guid personid)
        {
            var list =
            (from a in _anmeldelserRepo.Search(a =>
                    a.Anmelder != null &&
                    a.Anmelder.Id == personid
                    && a.ModtagerAnlaeg != null
                    && a.ModtagerAnlaeg.Aktiv
                    && a.ModtagerAnlaeg.Jordmodtager != null
                    )
                select a.ModtagerAnlaeg)
                .OrderBy(t => t.Navn).ToList();
      
            list = list.Distinct().ToList();

            return list;
        }

        public IList<ModtagerAnlaeg> ReadTidligereAktiveAnvendteModtagerAnlaeg(Guid personid, bool anvenderJf, Guid jordKlassifikationType, bool affald)
        {
            List<ModtagerAnlaeg> list;
            var jordKlastype = _kodelisteBusiness.ReadJordKlassifikationType(jordKlassifikationType);
  
            if (jordKlassifikationType == Guid.Empty)
            {
                list =
                    (from a in _anmeldelserRepo.Search(a =>
                                                    a.Anmelder != null &&
                                                    a.Anmelder.Id == personid
                                                    && a.ModtagerAnlaeg != null
                                                    && a.ModtagerAnlaeg.Aktiv
                                                    && a.ModtagerAnlaeg.Jordmodtager != null)
                    select a.ModtagerAnlaeg)
                    .OrderBy(t => t.Navn).ToList();

                list = list.Distinct().ToList();
            }
            else
            {
                list =
                    (from a in _anmeldelserRepo.Search(a =>
                        a.Anmelder != null &&
                        a.Anmelder.Id == personid &&
                        a.ModtagerAnlaeg != null &&
                        a.ModtagerAnlaeg.Aktiv &&
                        a.ModtagerAnlaeg.Jordmodtager != null &&
                        a.ModtagerAnlaeg.JordKlassifikationType.LandsdelTypeId == jordKlastype.LandsdelTypeId && 
                        a.ModtagerAnlaeg.JordKlassifikationType.Priotering <= jordKlastype.Priotering && 
                        a.ModtagerAnlaeg.AnmelderOprettetMidlertidigtAnlaeg == false)
                            /* 15.8.2017 TOK: Nedenstående linie er erstattet de 3 ovenstående, således modtageranlæg med en lavere klasse/kategori kan vælges */
                            //&& x.JordKlassifikationType.Id == jordKlassifikationType) 
                        //a.ModtagerAnlaeg.JordKlassifikationType.Id == jordKlassifikationType)
                    select a.ModtagerAnlaeg)
                    .OrderBy(t => t.Navn).ToList();

                list = list.Distinct().ToList();
            }

            if (anvenderJf)
            {
                list = list.Where(m => m.AnvenderJF).ToList();
            }

            if (affald)
            {
                list = list.Where(m => m.Affald).ToList();
            }

            return list;
        }

	    public IList<ModtagerAnlaeg> ReadAktiveNaermesteModtagerAnlaeg(DbGeometry geometry, bool anvenderJf, Guid jordKlassifikationType, bool affald)
	    {
            var jordKlastype = _kodelisteBusiness.ReadJordKlassifikationType(jordKlassifikationType);

		    if (jordKlassifikationType != Guid.Empty)
		    {
                return (from ma in _repo.Search(a => a.Aktiv & a.AnvenderJF == anvenderJf
                                                    && a.Aktiv
                                                    && a.JordKlassifikationType.LandsdelTypeId == jordKlastype.LandsdelTypeId
                                                    && a.JordKlassifikationType.Priotering <= jordKlastype.Priotering
                                                    && a.AnmelderOprettetMidlertidigtAnlaeg == false
                                       /* 15.8.2017 TOK: Nedenstående linie er erstattet de 3 ovenstående, således modtageranlæg med en lavere klasse/kategori kan vælges */
                                                    //&& a.JordKlassifikationType.Id == jordKlassifikationType
                                                    && a.Affald == affald)
                                        .OrderBy(m => m.Geom.Distance(geometry))
                        select ma).ToList();
		    }

            var list = (from ma in _repo.Search(a => a.Aktiv & a.AnvenderJF == anvenderJf)
                                        .OrderBy(m => m.Geom.Distance(geometry))
                        select ma).ToList();

		    return list;
	    }

        public void RemoveGraenseVaerdi(Guid modtageAnlaegGuid, Guid guid)
        {
            if (modtageAnlaegGuid == Guid.Empty) return;
            if (guid == Guid.Empty) return;

            var anl = Read(modtageAnlaegGuid);
            var list = anl.Graensevaerdier;

            var itemToBeRemoved = list.FirstOrDefault(a => a.ForureningskomponenterId == guid);
            _graensevaerdierRepository.Delete(itemToBeRemoved);

            SaveChanges();
        }

        public void UpdateGraenseVaerdi(Guid modtageAnlaegGuid, Guid forureningskomponentId, decimal? max)
        {
            if (modtageAnlaegGuid == Guid.Empty) return;
            if (forureningskomponentId == Guid.Empty) return;

            var anl = Read(modtageAnlaegGuid);
            var itemToBeUpdated = anl.Graensevaerdier.FirstOrDefault(a => a.ForureningskomponenterId == forureningskomponentId);

            if (itemToBeUpdated != null) {
                itemToBeUpdated.Max = (decimal)max;

                _graensevaerdierRepository.Commit();
                SaveChanges();
            }
        }

        public void AddGraenseVaerdi(Graensevaerdier fk)
        {
            if (fk == null) 
		        return;

            _graensevaerdierRepository.Add(fk);
            SaveChanges();
        }

        public void UpdateModtageAnlaegAktiv(Guid modtageAnlaegGuid, Boolean bAnlaegAktivt)
        {
            var anl = _repo.Read(modtageAnlaegGuid);
            anl.Aktiv = bAnlaegAktivt;
            _repo.Commit();
 
            SaveChanges();
        }

        public bool ShouldTakeStikproeve(Guid modtageAnlaegGuid)
        {
            var frekvens = 50;
            var modtageAnlaeg = _repo.Read(modtageAnlaegGuid);
            if (modtageAnlaeg != null && modtageAnlaeg.StikproeveFrekvens != null)
            frekvens = (int)modtageAnlaeg.StikproeveFrekvens;

            var vognLaesList = _vognlaesRepository.Search(x => x.Anmeldelse.ModtagerAnlaegId == modtageAnlaegGuid)
				    .OrderByDescending(a => a.Dato)
				    .Take(frekvens);

            var shouldTakeStikproeve = true;

			    // hvis vi endnu ikke er kommet op på 50, vil vi ikke tage nogen stikprøver
			    if (vognLaesList.Count() < frekvens)
				    return false;			

	        foreach (var vognlaes in vognLaesList)
            {
	            if (vognlaes.Stikproeve == null || vognlaes.Stikproeve.Count <= 0) 
					    continue;
	      
				    shouldTakeStikproeve = false;
	            break;
            }
            return shouldTakeStikproeve;
        }

        public IEnumerable<Anmeldelse> GetAnmeldelserOnModtageAnlaeg(int modtageAnlaegNummer, bool kunUafsluttede = true, DateTime? tidspunkt = null)
		{
            var list = _anmeldelserRepo.Search(a => a.StatusAnmeldelse.Any(s => s.Tid >= tidspunkt || tidspunkt == null) && a.ModtagerAnlaeg.Nummer == modtageAnlaegNummer).ToList();

		    if (kunUafsluttede)
		        return from n in list where !_statusAnmeldelseBusiness.IsAnmeldelseAfsluttet(n) select n;

		    return list;
		}

        public List<Lastbil> GetLastbilList(int modtageAnlaegNummer)
        {
	        var anmeldList = GetAnmeldelserOnModtageAnlaeg(modtageAnlaegNummer);
			var lastbilList = new List<Lastbil>();  

	        foreach (var anmeldelse in anmeldList)
	        {
		        foreach (var vognlaese in anmeldelse.Vognlaes)
		        {
			        if (vognlaese.Lastbil == null) 
						    continue;

			        lastbilList.Add(vognlaese.Lastbil);
		        }
	        }
            return lastbilList;
        }

        public double GetOmregningsfaktor(Guid modtageAnlaegGuid)
        {
            var aarhusHavnModtagerAnlaegGuidListe = new List<Guid>();
			    aarhusHavnModtagerAnlaegGuidListe.Add(ApplicationConstants.ModtageAnlaegGuidAarhusOliehavnRenJord); // Aarhus Oliehavn - ren jord
			    aarhusHavnModtagerAnlaegGuidListe.Add(ApplicationConstants.ModtageAnlaegGuidAarhusOliehavnLetForuJord); // Aarhus Oliehavns, let forurenet jord - opstart 1/2
                aarhusHavnModtagerAnlaegGuidListe.Add(ApplicationConstants.ModtageAnlaegGuidMellemdeponi); // Mellemsdeponi, Aarhus Østhavn
                aarhusHavnModtagerAnlaegGuidListe.Add(ApplicationConstants.ModtageAnlaegGuidAarhusMiljoeHavnLetForuJord); // Aarhus Miljøhavn, let forurenet jord

            var jorddkModtagerAnlaegGuidListe = new List<Guid>();
            jorddkModtagerAnlaegGuidListe.Add(ApplicationConstants.ModtageAnlaegGuidLangengevej143RenJord); // Langengevej 143 Ren jord, Jord.dk
            jorddkModtagerAnlaegGuidListe.Add(ApplicationConstants.ModtageAnlaegGuidLangengevej143LetforurenetJord); // Langengevej 143 Let forurenet jord, Jord.dk


            var m = Read(modtageAnlaegGuid);
            if (m != null)
            {
                //Århus Havn
                if (aarhusHavnModtagerAnlaegGuidListe.Contains(m.Id))
                {
                    switch (m.JordKlassifikationType.Kode)
                    {
                        case (short)EnumJordklassifikation.Kategori1:
                            {
                                return 5.0;
                            }
                        case (short)EnumJordklassifikation.Kategori2:
                            {
                                return 5.0;
                            }
                    }
                }

                //Jord.dk
                if (jorddkModtagerAnlaegGuidListe.Contains(m.Id))
                {
                    // Ton i stedet for aksler, gældende fra 1 februar 2022:
                    if (DateTime.Now >= new DateTime(2022, 02, 01))
                        return 1.0;

                    return 5.3;
                }

                //Andre modtagere
                switch (m.JordKlassifikationType.Kode)
                {
                    case (short)EnumJordklassifikation.Kategori1:
                    {
                        return 4.5;
                    }
                    case (short)EnumJordklassifikation.Kategori2:
                    {
                        return 3.6;
                    }
                    default:
                    {
                        return 4;
                    }
                }
            }
            return 4;
        }
    }
}
