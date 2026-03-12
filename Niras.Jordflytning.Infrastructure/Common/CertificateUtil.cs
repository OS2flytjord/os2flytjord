using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using System.Configuration;
using System;

namespace Niras.Jordflytning.Infrastructure.Common
{
    public class CertificateUtil
    {
        /// <summary>
        /// Private Utility method to get a certificate from a given store
        /// </summary>
        public static X509Certificate2 GetX509Certificate2(StoreLocation storeLocation, StoreName storeName, X509FindType findType, string findValue)
        {
            X509Store store = null;
            try
            {
                store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly);

                var certs = store.Certificates.Find(findType, findValue, false);

                if (certs.Count < 1)
                    throw new ApplicationException(string.Format("Certificate {0} not found", findValue));

                if (certs.Count > 1)
                    throw new ApplicationException(String.Format("Find value {0} resulted in more than one certificate", findValue));

                return certs[0];
            }
            finally
            {
                if (store != null) store.Close();
            }
        }

        public static CustomBinding CreateWsFederationBindingWithoutSecureConversation(WSFederationHttpBinding inputBinding)
        {
            if (inputBinding == null) throw new ArgumentNullException("inputBinding");

            // This CustomBinding starts out identical to the specified WSFederationHttpBinding.
            var outputBinding = new CustomBinding(inputBinding.CreateBindingElements());

            // Find the SecurityBindingElement for message security.
            var security = outputBinding.Elements.Find<SecurityBindingElement>();

            // If the security mode is message, then the secure session settings are the protection token parameters.
            if (inputBinding.Security.Mode == WSFederationHttpSecurityMode.Message)
            {
                var symmetricSecurity = (SymmetricSecurityBindingElement)security;

                var secureConversation = (SecureConversationSecurityTokenParameters)symmetricSecurity.ProtectionTokenParameters;

                var securityIndex = outputBinding.Elements.IndexOf(security);
                var newElement = (SymmetricSecurityBindingElement)secureConversation.BootstrapSecurityBindingElement;

                // Replace the secure session SecurityBindingElement with the bootstrap SecurityBindingElement.
                outputBinding.Elements[securityIndex] = newElement;
            }

            // Return modified binding.
            return outputBinding;
        }

        public static X509Certificate2 LoadX509CertificateFromConfigFile(string configKey)
        {
            if (configKey == null) throw new ArgumentNullException("configKey");

            var certBase64 = ConfigurationManager.AppSettings.Get(configKey);

            if (string.IsNullOrEmpty(certBase64))
                throw new ArgumentException("Config key: " + configKey + " is empty");

            var x509 = new X509Certificate2(Convert.FromBase64String(certBase64));

            return x509;
        }

    }
}
