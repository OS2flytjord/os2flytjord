using System.Web.Security;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class BrugereBusiness : GenericBusiness<BrugerProfil>, IBrugereBusiness
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.BrugereBusiness");

        private readonly ISecurityProvider _securityProvider;
        private readonly IPersonRepository _personRepo;
        private readonly IBrugereRepository _brugerRepo;
        private readonly Iwebpages_RolesRepository _roleRepo;
        private readonly IPersonJordmodtagerRepository _personJordmodtagerRepository;
        private readonly IPersonKommuneRepository _personKommuneRepository;
        private readonly IKommuneRepository _kommuneRepository;
        private readonly IJordmodtagerRepository _jordmodtagerRepository;
        private readonly ISagsbehandlerRepository _sagsbehandlerRepository;
        private readonly IAdviseringBusiness _adviseringBusiness;
        private readonly IPersonBusiness _personBusiness;



        public BrugereBusiness(
          ISecurityProvider securityProvider,
          IPersonRepository personRepo,
          IBrugereRepository brugerRepo,
          Iwebpages_RolesRepository roleRepo,
          IPersonKommuneRepository personKommuneRepository,
          IPersonJordmodtagerRepository personJordmodtagerRepository,
          IKommuneRepository kommuneRepository,
          IJordmodtagerRepository jordmodtagerRepository,
          IUnitOfWork uow,
          ISagsbehandlerRepository sagsbehandlerRepository,
          IAdviseringBusiness adviseringBusiness,
                PersonBusiness personBusiness
          )
            : base(brugerRepo, uow)
        {
            _securityProvider = securityProvider;
            _personRepo = personRepo;
            _brugerRepo = brugerRepo;
            _roleRepo = roleRepo;
            _personJordmodtagerRepository = personJordmodtagerRepository;
            _personKommuneRepository = personKommuneRepository;
            _jordmodtagerRepository = jordmodtagerRepository;
            _kommuneRepository = kommuneRepository;
            _sagsbehandlerRepository = sagsbehandlerRepository;
            _adviseringBusiness = adviseringBusiness;
            _personBusiness = personBusiness;
        }

        /// <summary>
        /// Creates new user profile(Betaler) and saves person data.
        /// </summary>
        /// <param name="userProfile">The user profile to create and save person data for.</param>
        /// <returns>True if user profile and person data can be created; otherwise false.</returns>
        public Boolean CreateBetaler(BrugerProfil userProfile)
        {
            var result = true;
            try
            {
                // Generated random password
                userProfile.PasswordClearText = Membership.GeneratePassword(6, 0);
                // create user and account
                var activationToken = _securityProvider.CreateUserAndAccount(userProfile.BrugerNavn, userProfile.PasswordClearText);

                // check that user is created
                var userId = _securityProvider.GetUserId(userProfile.BrugerNavn);
                if (userId > 0)
                {
                    var user = _brugerRepo.Read(userId);
                    userProfile.Person.BrugerId = user.BrugerId;
                    userProfile.Person.Betaler = new Betaler();
                    _personRepo.Add(userProfile.Person);
                    SaveChanges();

                    //Skal nok sendes en anden type email af sted.
                    if (userProfile.Person.BrugerId > 0 && userProfile.Person.Id != Guid.Empty)
                    {
                        _adviseringBusiness.SendAktivationEmailVedOprettetAfAndenBruger(userProfile, activationToken);
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                result = false;
            }
            return result;
        }


        public IList<PersonKommune> ReadPersonKommune()
        {
            return _personKommuneRepository.Read().ToList();
        }

        public IList<PersonJordmodtager> ReadPersonJordmodtager()
        {
            return _personJordmodtagerRepository.Read().ToList();
        }

        public IList<PersonKommune> ReadKommunePersoner(Guid kommuneId)
        {
            return _personKommuneRepository.Search(x => x.KommuneId == kommuneId).ToList();
        }

        public IList<Person> ReadAktiveSagsbehandlere(Guid kommuneId)
        {
            var brugerIds = (from pk in _personKommuneRepository.Search(x => x.KommuneId == kommuneId) select pk.Person.BrugerId).ToList();

            var bs = (from b in _brugerRepo.Search(v => brugerIds.Contains(v.BrugerId) && v.webpages_Roles.Select(x => x.RoleName).Contains("Sagsbehandler"))
                      select b.BrugerId).ToList();
            var sagsbehandlerPersons = (from p in _personRepo.Search(x => bs.Contains(x.BrugerId) && x.Aktiv) select p).ToList();
            return sagsbehandlerPersons;
        }

        public IList<Person> ReadAktivePladsmaend(Guid jordmodtagerId)
        {
            //return
            //  _brugerRepo.Search(
            //    p =>
            //    p.Person.Aktiv &&
            //    p.Roles.Contains("Pladsmand") &&
            //    p.Person.PersonJordmodtager.FirstOrDefault(pe => pe.JordmodtagerId == jordmodtagerId) != null)
            //             .Select(u => u.Person)
            //             .ToList();

            var brugerIds = (from pk in _personJordmodtagerRepository.Search(x => x.JordmodtagerId == jordmodtagerId) select pk.Person.BrugerId).ToList();
            var bs = (from b in _brugerRepo.Search(v => brugerIds.Contains(v.BrugerId) && v.webpages_Roles.Select(x => x.RoleName).Contains(ApplicationConstants.PladsmandRolle))
                      select b.BrugerId).ToList();
            var pladsmaend = (from p in _personRepo.Search(x => bs.Contains(x.BrugerId) && x.Aktiv) select p).ToList();
            return pladsmaend;
        }

        public IList<Person> ReadAktiveMiljoemedarbejdere(Guid jordmodtagerId)
        {
            //return
            //  _brugerRepo.Search(
            //    p =>
            //    p.Person.Aktiv &&
            //    p.Roles.Contains("Miljømedarbejder") &&
            //    p.Person.PersonJordmodtager.FirstOrDefault(pe => pe.JordmodtagerId == jordmodtagerId) != null)

            //             .Select(u => u.Person)
            //             .ToList();

            var brugerIds = (from pk in _personJordmodtagerRepository.Search(x => x.JordmodtagerId == jordmodtagerId) select pk.Person.BrugerId).ToList();
            var bs = (from b in _brugerRepo.Search(v => brugerIds.Contains(v.BrugerId) && v.webpages_Roles.Select(x => x.RoleName).Contains(ApplicationConstants.MiljoemedarbejderRolle))
                      select b.BrugerId).ToList();
            var miljoemedarbejdere = (from p in _personRepo.Search(x => bs.Contains(x.BrugerId) && x.Aktiv) select p).ToList();
            return miljoemedarbejdere;
        }

        public IList<Person> ReadAktiveProevetagere(Guid? jordmodtagerId)
        {
            if (jordmodtagerId == null)
                return null;

            var brugerIds = (from pk in _personJordmodtagerRepository.Search(x => x.JordmodtagerId == jordmodtagerId)
                             select pk.Person.BrugerId).ToList();

            var bs = (from b in _brugerRepo.Search(v =>
                                              brugerIds.Contains(v.BrugerId)
                                              && v.webpages_Roles.Select(x => x.RoleName).Contains(ApplicationConstants.ProevetagerRolle))
                      select b.BrugerId).ToList();

            var proevetagere = (from p in _personRepo.Search(x => bs.Contains(x.BrugerId) && x.Aktiv)
                                select p).ToList();

            return proevetagere;
        }

        public IList<Person> ReadAktiveLaboratoriepersoner(Guid jordmodtagerId)
        {
            var brugerIds = (from pk in _personJordmodtagerRepository.Search(x => x.JordmodtagerId == jordmodtagerId)
                             select pk.Person.BrugerId).ToList();

            var bs = (from b in _brugerRepo.Search(v => brugerIds.Contains(v.BrugerId) && v.webpages_Roles.Select(x => x.RoleName).Contains(ApplicationConstants.LaboratorieRolle))
                      select b.BrugerId).ToList();

            var laboratoriepersoner = (from p in _personRepo.Search(x => bs.Contains(x.BrugerId) && x.Aktiv)
                                       select p).ToList();
            return laboratoriepersoner;
        }

        public Sagsbehandler ReadSagsbehandler(Guid personId)
        {
            var s = _sagsbehandlerRepository.Read(personId);
            if (s != null)
                return s;

            s = new Sagsbehandler();
            s.Id = personId;
            return s;
        }

        public void DeletePersonKommune(PersonKommune personKommune)
        {
            _personKommuneRepository.Delete(personKommune);
        }

        public void DeletePersonJordmodtager(PersonJordmodtager personJordmodtager)
        {
            _personJordmodtagerRepository.Delete(personJordmodtager);
        }

        public PersonJordmodtager ReadJordmodtagerPerson(Guid jordmodtagerId, Guid personId)
        {
            return (from pj in
                        ReadJordmodtagerPersoner(jordmodtagerId)
                    where pj.Person.Id == personId
                    select pj).FirstOrDefault();
        }

        public PersonKommune ReadKommunePerson(Guid kommuneId, Guid personId)
        {
            return (from pk in ReadKommunePersoner(kommuneId)
                    where pk.Person.Id == personId
                    select pk).FirstOrDefault();
        }

        public IList<Jordmodtager> ReadJordmodtagerForPerson(Guid personId)
        {
            var jps =
              (from jp in _personJordmodtagerRepository.Search(x => x.Person.Id == personId) select jp.Jordmodtager).ToList();
            return jps;
        }

        public Person ReadPerson(Guid personId)
        {
            return _personRepo.Read(personId);
        }

        public IList<PersonJordmodtager> ReadJordmodtagerPersoner(Guid jordmodtagerId)
        {
            return _personJordmodtagerRepository.Search(x => x.JordmodtagerId == jordmodtagerId).ToList();
        }

        public bool AddUserToRole(string userName, string roleName)
        {
            var role = _roleRepo.Search(x => x.RoleName == roleName).FirstOrDefault();

            if (role != null)
            {
                var count = role.BrugerProfil.Count;
                var bruger = _brugerRepo.Read(userName);
                if (!role.BrugerProfil.Contains(bruger))
                {
                    role.BrugerProfil.Add(_brugerRepo.Read(userName));
                    SaveChanges();
                }
                return role.BrugerProfil.Count == (count + 1);
            }
            return false;
        }

        public bool RemoveUserFromRole(string userName, string roleName)
        {
            var role = _roleRepo.Search(x => x.RoleName == roleName).FirstOrDefault();

            if (role != null)
            {
                var count = role.BrugerProfil.Count;
                var bruger = _brugerRepo.Read(userName);
                if (role.BrugerProfil.Contains(bruger))
                {
                    role.BrugerProfil.Remove(bruger);
                    SaveChanges();
                }
                return count == (role.BrugerProfil.Count + 1);
            }
            return false;
        }

        public Boolean Update(BrugerProfil userProfile, IList<String> roles)
        {
            IList<bool> resultList = new List<bool>();
            var currentUserRoles = Roles.GetRolesForUser(userProfile.BrugerNavn);

            foreach (var role in currentUserRoles)
            {
                if (!roles.Contains(role))
                {
                    resultList.Add(RemoveUserFromRole(userProfile.BrugerNavn, role)); //Fjern brugeren fra rollen hvis ikke brugeren har den længere.
                }
                else
                {
                    roles.Remove(role);
                }
            }
            foreach (var role in roles)
            {
                resultList.Add(AddUserToRole(userProfile.BrugerNavn, role)); // Tilføj brugeren til eventuelle nye tildelte roller.
            }
            SaveChanges();
            return !resultList.Contains(false);
        }

        private bool CreateUser(BrugerProfil userProfile, bool silent, String[] roles, String organisationsId = null)
        {
            var result = true;
            try
            {
                // create user and account
                var activationToken = _securityProvider.CreateUserAndAccount(userProfile.BrugerNavn, userProfile.PasswordClearText);

                // check that user is created
                var userId = _securityProvider.GetUserId(userProfile.BrugerNavn);
                if (userId > 0)
                {
                    // assign roles
                    if (roles != null)
                    {
                        foreach (var r in roles)
                        {
                            AddUserToRole(userProfile.BrugerNavn, r);
                        }
                    }

                    var user = _brugerRepo.Read(userId);
                    userProfile.Person.BrugerId = user.BrugerId;
                    _personRepo.Add(userProfile.Person);

                    if (organisationsId != null)
                    {
                        if (_kommuneRepository.Read(new Guid(organisationsId)) != null)
                        {
                            var personKommune = new PersonKommune();
                            personKommune.KommuneId = new Guid(organisationsId);
                            personKommune.Person = userProfile.Person;
                            _personKommuneRepository.Add(personKommune);
                        }

                        if (_jordmodtagerRepository.Read(new Guid(organisationsId)) != null)
                        {
                            var personJordmodtager = new PersonJordmodtager();
                            personJordmodtager.JordmodtagerId = new Guid(organisationsId);
                            personJordmodtager.Person = userProfile.Person;
                            _personJordmodtagerRepository.Add(personJordmodtager);
                        }
                    }
                    SaveChanges();

                    if (userProfile.Person.BrugerId > 0 && userProfile.Person.Id != Guid.Empty)
                    {
                        if (!silent)
                            _adviseringBusiness.SendAktivationEmail(userProfile, activationToken);
                    }
                    else // hvis person ikke blev oprettet så slet brugerprofilen	og/eller personen
                    {
                        if (userProfile.Person.Id != Guid.Empty)
                        {
                            _personRepo.Delete(userProfile.Person);
                            SaveChanges();
                        }

                        if (userProfile.Person.BrugerId > 0)
                        //else if (userProfile.Person.BrugerId > 0)
                        {
                            Delete(Read(userProfile.BrugerNavn));
                        }
                        result = false;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                result = false;
                if (userProfile.Person.Id != Guid.Empty)
                {
                    _personRepo.Delete(userProfile.Person);
                }
                else if (userProfile.BrugerId > 0)
                {
                    Delete(Read(userProfile.BrugerNavn));
                }
            }
            return result;
        }

        /// <summary>
        /// Creates new user profile and saves person data.
        /// </summary>
        /// <param name="userProfile">The user profile to create and save person data for.</param>
        /// <param name="roles">List of roles to assign user.</param>
        /// <param name="organisationsId"></param>
        /// <returns>True if user profile and person data can be created; otherwise false.</returns>
        public bool Create(BrugerProfil userProfile, String[] roles, String organisationsId = null)
        {
            return CreateUser(userProfile, false, roles, organisationsId);
        }

        /// <summary>
        /// Creates new user profile and saves person data, but does not send an activation email.
        /// </summary>
        /// <param name="userProfile">The user profile to create and save person data for.</param>
        /// <param name="roles">List of roles to assign user.</param>
        /// <param name="organisationsId"></param>
        /// <returns>True if user profile and person data can be created; otherwise false.</returns>
        public bool CreateSilent(BrugerProfil userProfile, String[] roles, String organisationsId = null)
        {
            return CreateUser(userProfile, true, roles, organisationsId);
        }

        public BrugerProfil Read(String userName)
        {
            BrugerProfil result = null;
            // Get Id of user
            var userId = _securityProvider.GetUserId(userName);
            if (userId != -1)
            {
                // Read user profile from user Id
                result = _brugerRepo.Read(userId);

                // Read roles from user name
                var userRoles = _securityProvider.GetUserRoles(result.BrugerNavn);
                foreach (var role in userRoles)
                {
                    result.Roles.Add(role);
                }

                // Add person data
                lock (_personBusiness)
                {
                    result.Person = _personBusiness.ReadByBrugerId(userId);
                }
            }
            return result;
        }

        public bool ProfileExists(string newmail)
        {
            return _brugerRepo.Search(p => p.BrugerNavn == newmail).FirstOrDefault() != null ? true : false;
        }

        public IList<BrugerProfil> GetBrugerProfilList()
        {
            IList<BrugerProfil> result = _brugerRepo.Read().ToList();
            foreach (var brugerProfil in result)
            {
                lock (brugerProfil)
                {
                    brugerProfil.Person = _personBusiness.ReadByBrugerId(brugerProfil.BrugerId);
                }
            }
            return result;
        }

        public List<Kommune> GetUsersKommuneOrganisationer(Guid id)
        {
            var retList = new List<Kommune>();
            var list = _personKommuneRepository.Search(x => x.PersonId == id).ToList();

            foreach (var personKommune in list)
            {
                var komm = personKommune.Kommune;
                var addKomm = true;
                foreach (var kommune in retList)
                {
                    if (personKommune.KommuneId == kommune.Id)
                    {
                        addKomm = false;
                    }
                }
                if (addKomm)
                {
                    retList.Add(komm);
                }
            }
            return retList;
        }

        public List<Jordmodtager> GetUsersJordModtOrganisationer(Guid id)
        {
            var retList = new List<Jordmodtager>();
            var list = _personJordmodtagerRepository.Search(x => x.PersonId == id).ToList();

            foreach (var jordmodtager in list)
            {
                var jordMdt = jordmodtager.Jordmodtager;
                var addJordMdt = true;
                foreach (var jordmodtagerRet in retList)
                {
                    if (jordmodtagerRet.Id == jordmodtager.Id)
                    {
                        addJordMdt = false;
                    }
                }
                if (addJordMdt)
                {
                    retList.Add(jordMdt);
                }
            }
            return retList;
        }

        public IList<String> GetUserRole(int userId)
        {
            // Read user profile from user Id
            var brugerProfil = _brugerRepo.Read(userId);
            // Read role from user name
            var userRole = _securityProvider.GetUserRoles(brugerProfil.BrugerNavn);
            return userRole;
        }

        public IList<String> GetUserRoleList(Guid userId)
        {
            // Read user profile from user Id
            var brugerProfil = _brugerRepo.Read(userId);
            // Read role from user name
            var userRole = _securityProvider.GetUserRoles(brugerProfil.BrugerNavn);
            return userRole;
        }

        public bool IsUserRelatedToSevelralKommunerOrJordmodtagere(string email, Guid organisationId)
        {
            //var organisationKommune = ReadAktiveSagsbehandlere(organisationId);

            //bool res = false;
            //var user = Read(email);
            //if (user != null)
            //{
            //  isUserInOrganisation = (user.Person.PersonKommune.Count + user.Person.PersonJordmodtager.Count) > 0;
            //}


            //var isUserInOrganisation = (user.Person.PersonKommune.Count + user.Person.PersonJordmodtager.Count) > 0;
            return false;

        }


        /// <summary>
        /// Henter den påloggede person
        /// </summary>
        /// <returns>Person, NULL hvis ingen fundet</returns>
        public Person GetCurrentLoggedOnPerson()
        {
            Person person = null;
            if (_securityProvider.CurrentUser != null && _securityProvider.CurrentUser.Identity.Name != null)
            {
                var identity = Read(_securityProvider.CurrentUser.Identity.Name);
                if (identity != null)
                    person = identity.Person;
            }
            return person;
        }

        #region *** Private Methods ***

        #endregion *** Private Methods ***

    }
}
