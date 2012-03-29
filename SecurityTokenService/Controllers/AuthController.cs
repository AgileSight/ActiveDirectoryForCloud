using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudSecurityTokenService.Models;
using System.Web.Security;
using System.ServiceModel;
using ActiveDirectoryForCloud.Proxy;
using CloudSecurityTokenService.Infrastructure;

namespace CloudSecurityTokenService.Controllers
{
    public class AuthController : Controller
    {
        IAuthenticationClient authenticationClient;
        IFormsAuthentication formsAuthentication;

        public AuthController()
            : this(new AuthenticationClient(), new FormsAuthenticationWrapper())
        {
        }

        public AuthController(IAuthenticationClient authenticationClient, IFormsAuthentication formsAuthentication)
        {
            this.authenticationClient = authenticationClient;
            this.formsAuthentication = formsAuthentication;
        }

        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            return View("Login", new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var response = this.authenticationClient.Authenticate(new AuthenticationRequest
                {
                    Username = model.Username,
                    Password = model.Password
                });

                if (response != null && response.Result)
                {
                    this.formsAuthentication.SetAuthCookie(model.Username);

                    this.Session["Attributes"] = response.Attributes;

                    return Redirect(model.ReturnUrl);
                }
            }

            return View("Login", model);
        }
    }
}
