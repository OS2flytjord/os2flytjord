using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.ViewModels.Backend
{
  public class JordmodtagerListModel
	{
	  public JordmodtagerViewModel SelectedJordmodtager { get; set; }		

		public IList<JordmodtagerViewModel> JordModtagerList { get; set; }
	}
}