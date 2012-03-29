using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;

namespace RelyingParty.Controllers
{
    public class HomeController : Controller
    {
        [ValidateInput(false)]
        public ActionResult Index()
        {
            var principal = (ClaimsPrincipal) User;

            return View("Index", (object)principal.Identity.Name);
        }

    }
}
