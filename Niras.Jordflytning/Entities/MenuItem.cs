using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niras.Jordflytning.Entities
{
	public class MenuItem
	{
		public List<MenuItem> SubItems { get; set; }
		public string Text { get; set; }
		public string MergeText { get; set; }
		public string Url { get; set; }

		public MenuItem() : this("","")
		{
			
		}
		public MenuItem(string text,string mergeText = "", string url = "" )
		{
			SubItems = new List<MenuItem>();
			Text = text;
			Url = url;
			MergeText = mergeText;
		}

		public bool AddSubItem(MenuItem menuItem)
		{
			int count = SubItems.Count();
			SubItems.Add(menuItem);

			return SubItems.Count() == ++count;
		}
	}
}