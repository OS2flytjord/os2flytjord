using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niras.Jordflytning.Entities
{
	public class Menu
	{
		public List<MenuItem> MenuItems { get; set; }
		public bool AddMenuItem(MenuItem menuItem)
		{
			int count = MenuItems.Count();
			MenuItems.Add(menuItem);

			return MenuItems.Count() == ++count;
		}

		public Menu()
		{
			MenuItems = new List<MenuItem>();
		}

		public void MergeMenues(Menu menuToMerge)
		{
			foreach (MenuItem menuItem in menuToMerge.MenuItems)
			{
				if (this.MenuItems.FirstOrDefault(m => m.Text == menuItem.Text) == null) // hvis menu punktet ikke findes i forvejen
				{
					this.AddMenuItem(menuItem);
				}
				else // menu punktet findes allerede, kig på undermenuerne
				{
					foreach (MenuItem subMenuItem in menuItem.SubItems)
					{
						//subMenuItem.Text = subMenuItem.MergeText;
						if (this.MenuItems[this.MenuItems.IndexOf(this.MenuItems.FirstOrDefault(m => m.Text == menuItem.Text))].SubItems.FirstOrDefault(mi => mi.Text == subMenuItem.Text) == null)
						{
							subMenuItem.Text = subMenuItem.MergeText;
							this.MenuItems[this.MenuItems.IndexOf(this.MenuItems.FirstOrDefault(m => m.Text == menuItem.Text))].SubItems.Add(subMenuItem);
						}
						else
						{
							MenuItems[this.MenuItems.IndexOf(this.MenuItems.FirstOrDefault(m => m.Text == menuItem.Text))].SubItems.ForEach(x => x.Text = x.MergeText);
							subMenuItem.Text = subMenuItem.MergeText;
							this.MenuItems[this.MenuItems.IndexOf(this.MenuItems.FirstOrDefault(m => m.Text == menuItem.Text))].SubItems.Add(subMenuItem);
							//Tilføj, men giv et alternativt navn så der ikke bliver to med navnet forside
						}
					}
				}
			}
		}
	}
}