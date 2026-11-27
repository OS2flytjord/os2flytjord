using System;

namespace Niras.Jordflytning.IntegrationTests.MockData
{
	/// <summary>Id'er på kommuner fra kodelisten (MockData\Kodelister\KommuneMock), som testene bruger direkte.</summary>
	public static class Kommuner
	{
		public static readonly Guid AarhusId = new Guid("A15D5888-BC70-4206-871E-A47A0C05DECC"); // 751
		public static readonly Guid SkanderborgId = new Guid("D1FFE942-1608-4A13-A7F3-80B11AE019E3"); // 746
		public static readonly Guid RoskildeId = new Guid("FF8AEE8D-3512-4E7E-94A4-51B8BEB3C732"); // 265
	}
}
