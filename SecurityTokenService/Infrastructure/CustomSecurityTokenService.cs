using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Web.Configuration;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using System.Web;
using ActiveDirectoryForCloud.Proxy;

namespace CloudSecurityTokenService.Infrastructure
{
    /// <summary>
    /// A custom SecurityTokenService implementation.
    /// </summary>
    public class CustomSecurityTokenService : SecurityTokenService
    {
        /// <summary>
        /// Creates an instance of CustomSecurityTokenService.
        /// </summary>
        /// <param name="configuration">The SecurityTokenServiceConfiguration.</param>
        public CustomSecurityTokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// This method returns the configuration for the token issuance request. The configuration
        /// is represented by the Scope class. In our case, we are only capable of issuing a token for a
        /// single RP identity represented by the EncryptingCertificateName.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST.</param>
        /// <returns>The scope information to be used for the token issuance.</returns>
        protected override Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request)
        {
            //
            // Note: The signing certificate used by default has a Distinguished name of "CN=STSTestCert",
            // and is located in the Personal certificate store of the Local Computer. Before going into production,
            // ensure that you change this certificate to a valid CA-issued certificate as appropriate.
            //
            Scope scope = new Scope(request.AppliesTo.Uri.OriginalString, SecurityTokenServiceConfiguration.SigningCredentials);

            string encryptingCertificateName = WebConfigurationManager.AppSettings["EncryptingCertificateName"];
            if (!string.IsNullOrEmpty(encryptingCertificateName))
            {
                // Important note on setting the encrypting credentials.
                // In a production deployment, you would need to select a certificate that is specific to the RP that is requesting the token.
                // You can examine the 'request' to obtain information to determine the certificate to use.
                scope.EncryptingCredentials = new X509EncryptingCredentials(CertificateUtil.GetCertificate(StoreName.My, StoreLocation.CurrentUser, encryptingCertificateName));
            }
            else
            {
                // If there is no encryption certificate specified, the STS will not perform encryption.
                // This will succeed for tokens that are created without keys (BearerTokens) or asymmetric keys.  
                scope.TokenEncryptionRequired = false;
            }

            // Set the ReplyTo address for the WS-Federation passive protocol (wreply). This is the address to which responses will be directed. 
            // In this template, we have chosen to set this to the AppliesToAddress.
            scope.ReplyToAddress = scope.AppliesToAddress;

            return scope;
        }


        /// <summary>
        /// This method returns the claims to be issued in the token.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST, can be used to obtain addtional information.</param>
        /// <param name="scope">The scope information corresponding to this request.</param> 
        /// <exception cref="ArgumentNullException">If 'principal' parameter is null.</exception>
        /// <returns>The outgoing claimsIdentity to be included in the issued token.</returns>
        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            if (null == principal)
            {
                throw new ArgumentNullException("principal");
            }

            var outputIdentity = new ClaimsIdentity();

            var attributes = (UserAttribute[])HttpContext.Current.Session["Attributes"];

            foreach (var attribute in attributes)
            {
                if (attribute.Name == "Email")
                {
                    outputIdentity.Claims.Add(new Claim(System.IdentityModel.Claims.ClaimTypes.Email, attribute.Value));
                }
                else if (attribute.Name == "Group")
                {
                    outputIdentity.Claims.Add(new Claim(ClaimTypes.Role, attribute.Value));
                }
            }
            
            outputIdentity.Claims.Add(new Claim(System.IdentityModel.Claims.ClaimTypes.Name, principal.Identity.Name));

            return outputIdentity;
        }
    }
}