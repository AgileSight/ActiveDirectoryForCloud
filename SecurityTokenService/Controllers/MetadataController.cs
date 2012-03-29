using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Protocols.WSFederation.Metadata;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSFederation;
using CloudSecurityTokenService.Infrastructure;
using System.IdentityModel.Tokens;
using System.ServiceModel;

namespace CloudSecurityTokenService.Controllers
{
    public class MetadataController : Controller
    {
        public SigningCredentials SigningCredentials
        {
            get
            {
                return CustomSecurityTokenServiceConfiguration.Current.SigningCredentials;
            }
        }
        
        public ActionResult Index()
        {
            return new MetadataActionResult(GenerateEntities());
        }

        private EntityDescriptor GenerateEntities() 
        { 
            var sts = new SecurityTokenServiceDescriptor(); 
            FillOfferedClaimTypes(sts.ClaimTypesOffered); 
            FillEndpoints(sts); 
            FillSupportedProtocols(sts); 
            FillSigningKey(sts); 
            var entity = new EntityDescriptor(new EntityId(string.Format("https://{0}", Request.Url.GetLeftPart(UriPartial.Authority)))) 
            {
                SigningCredentials = this.SigningCredentials 
            }; 
            entity.RoleDescriptors.Add(sts); 
            return entity; 
        }

        private void FillSigningKey(SecurityTokenServiceDescriptor sts) 
        {
            KeyDescriptor signingKey = new KeyDescriptor(this.SigningCredentials.SigningKeyIdentifier) 
            { 
                Use = KeyType.Signing 
            }; 
            
            sts.Keys.Add(signingKey); 
        } 
        
        private void FillSupportedProtocols(SecurityTokenServiceDescriptor sts) 
        { 
            sts.ProtocolsSupported.Add(new System.Uri(WSFederationConstants.Namespace)); 
        } 
        
        private void FillEndpoints(SecurityTokenServiceDescriptor sts) 
        {
            var endpoint = new EndpointAddress(string.Format("http://{0}", Request.Url.GetLeftPart(UriPartial.Authority)));
            sts.SecurityTokenServiceEndpoints.Add(endpoint);
            sts.PassiveRequestorEndpoints.Add(endpoint);
        } 
        
        private void FillOfferedClaimTypes(ICollection<DisplayClaim> claimTypes) 
        { 
            claimTypes.Add(new DisplayClaim(ClaimTypes.Name, "Name", "")); 
            claimTypes.Add(new DisplayClaim(ClaimTypes.Email, "Email", "")); 
            claimTypes.Add(new DisplayClaim(ClaimTypes.Role, "Role", "")); 
        }
    }
}
