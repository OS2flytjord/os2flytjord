using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.ViewModels.Backend
{
    public class ModtagerAnlaegAdminModel
    {

        public ModtagerAnlaegAdminModel()
        {
            JordanlaegTypeList = new List<SelectListItem>();
            JordKlassifikationTypeList = new List<SelectListItem>();
            LandsdelTypeList = new List<SelectListItem>();
        }

        private ModtagerAnlaegViewModel _selectedModtagerAnlaeg;

        public JordmodtagerViewModel SelectedJordmodtager { get; set; }

        public List<ModtagerAnlaegViewModel> ModtagerAnlaegList { get; set; }

        public ModtagerAnlaegViewModel SelectedModtagerAnlaeg
        {
            get { return _selectedModtagerAnlaeg; }
            set
            {
                _selectedModtagerAnlaeg = value;
                if (_selectedModtagerAnlaeg != null)
                    SelectedModtagerAnlaegGuid = _selectedModtagerAnlaeg.Id;
            }
        }

        public Guid SelectedModtagerAnlaegGuid { get; set; }

        public IEnumerable<SelectListItem> JordanlaegTypeList { get; set; }

        public IEnumerable<SelectListItem> JordKlassifikationTypeList { get; set; }

        public IEnumerable<SelectListItem> LandsdelTypeList { get; set; }

        public void AddLandsdelType(IList<LandsdelType> landsdelTypeList)
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            if (landsdelTypeList != null)
            {
                foreach (var landsdelType in landsdelTypeList)
                {
                    if (landsdelType.Id == Guid.Empty)
                        continue;

                    var selItem = new SelectListItem();
                    selItem.Text = landsdelType.Navn;
                    selItem.Value = landsdelType.Id.ToString();
                    list.Add(selItem);
                }
            }
            LandsdelTypeList = list;
        }

        public void AddJordanlaegType(IList<JordanlaegType> anvendteJordanlaegTypeList)
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            if (anvendteJordanlaegTypeList != null)
            {
                foreach (var jordanlaegType in anvendteJordanlaegTypeList)
                {
                    if (jordanlaegType.Id == Guid.Empty)
                        continue;

                    var selItem = new SelectListItem();
                    selItem.Text = jordanlaegType.Navn;
                    selItem.Value = jordanlaegType.Id.ToString();
                    list.Add(selItem);
                }
            }
            JordanlaegTypeList = list;
        }

        public void AddKlassifikationType(IList<JordKlassifikationType> klassiTypeList)
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            if (klassiTypeList != null)
            {
                foreach (var klassifikationType in klassiTypeList)
                {
                    if (klassifikationType.Id == Guid.Empty)
                        continue;

                    var selItem = new SelectListItem();
                    selItem.Text = klassifikationType.Navn;
                    selItem.Value = klassifikationType.Id.ToString();
                    list.Add(selItem);
                }
            }
            JordKlassifikationTypeList = list;
        }
    }
}