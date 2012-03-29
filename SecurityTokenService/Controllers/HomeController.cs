using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Protocols.WSFederation;
using System.Net;
using System.Globalization;
using CloudSecurityTokenService.Infrastructure;
using Microsoft.IdentityModel.Web;
using System.Web.Security;

namespace CloudSecurityTokenService.Controllers
{
    public class HomeController : Controller
    {
        IFormsAuthentication formsAuthentication;

        public HomeController()
            : this(new FormsAuthenticationWrapper())
        {
        }

        public HomeController(IFormsAuthentication formsAuthentication)
        {
            this.formsAuthentication = formsAuthentication;
        }

        [Authorize]
        public ActionResult Index()
        {
            var action = Request.QueryString[WSFederationConstants.Parameters.Action];

            if ( action == WSFederationConstants.Actions.SignIn )
            {
                // Process signin request.
                var requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri( Request.Url );
                if ( User != null && User.Identity != null && User.Identity.IsAuthenticated )
                {
                    var sts = new CustomSecurityTokenService(CustomSecurityTokenServiceConfiguration.Current);
                    var responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest( requestMessage, User, sts );

                    return new SignInActionResult(responseMessage);
                }
                else
                {
                    return new HttpUnauthorizedResult();
                }
            }
            else if (action == WSFederationConstants.Actions.SignOut)
            {
                // Process signout request.
                var requestMessage = (SignOutRequestMessage)WSFederationMessage.CreateFromUri(Request.Url);

                this.formsAuthentication.SignOut();

                return Redirect(requestMessage.Reply);
            }
            else
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest,
                    String.Format(CultureInfo.InvariantCulture,
                                   "The action '{0}' (Request.QueryString['{1}']) is unexpected. Expected actions are: '{2}' or '{3}'.",
                                   String.IsNullOrEmpty(action) ? "<EMPTY>" : action,
                                   WSFederationConstants.Parameters.Action,
                                   WSFederationConstants.Actions.SignIn,
                                   WSFederationConstants.Actions.SignOut));
            }
        }
    }
}
