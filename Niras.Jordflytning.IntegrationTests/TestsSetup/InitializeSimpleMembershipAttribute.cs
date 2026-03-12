using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using WebMatrix.WebData;

namespace Niras.Jordflytning.IntegrationTests.TestsSetup
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class InitializeSimpleMembershipAttribute : Attribute
	{
		private static SimpleMembershipInitializer _initializer;
		private static object _initializerLock = new object();
		private static bool _isInitialized;

		public InitializeSimpleMembershipAttribute()
		{
			// Ensure ASP.NET Simple Membership is initialized only once per app start
			LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
		}

		private class SimpleMembershipInitializer
		{
			public SimpleMembershipInitializer()
			{
				try
				{
					//WebSecurity.InitializeDatabaseConnection("DefaultConnection", "BrugerProfil", "BrugerId", "BrugerNavn", autoCreateTables: true);
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
				}
			}
		}
	}
}
