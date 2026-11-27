using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace Niras.Jordflytning.Core.BusinessLogic
{
	/// <summary>
	/// Finder mappen med advis-/mailskabeloner (App_Data\Skabeloner i webprojektet).
	/// </summary>
	internal static class SkabelonMappe
	{
		private const string GammelFastSti = @"C:\projects\jordflytning\trunk\Niras.Jordflytning\App_Data\Skabeloner";

		/// <summary>
		/// 1. I webappen: ~/App_Data/skabeloner.
		/// 2. appSetting "SkabelonBaseDir" (absolut, eller relativt til programmets mappe).
		/// 3. Niras.Jordflytning\App_Data\Skabeloner fundet opad fra programmets mappe (tests, lokal kørsel fra repo'et).
		/// 4. Den tidligere faste sti.
		/// </summary>
		public static string Find()
		{
			if (HttpContext.Current != null)
				return HttpContext.Current.Server.MapPath(@"~\App_Data\skabeloner");

			var baseDir = AppDomain.CurrentDomain.BaseDirectory;

			var konfigureret = ConfigurationManager.AppSettings["SkabelonBaseDir"];
			if (!string.IsNullOrWhiteSpace(konfigureret))
				return Path.GetFullPath(Path.Combine(baseDir, konfigureret));

			for (var dir = new DirectoryInfo(baseDir); dir != null; dir = dir.Parent)
			{
				var kandidat = Path.Combine(dir.FullName, "Niras.Jordflytning", "App_Data", "Skabeloner");
				if (Directory.Exists(kandidat))
					return kandidat;
			}

			return GammelFastSti;
		}
	}
}
