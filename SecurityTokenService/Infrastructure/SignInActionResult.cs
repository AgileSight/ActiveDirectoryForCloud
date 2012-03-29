using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Web;

namespace CloudSecurityTokenService.Infrastructure
{
    public class SignInActionResult : ActionResult
    {
        SignInResponseMessage message;

        public SignInActionResult(SignInResponseMessage message)
        {
            this.message = message;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            this.message.Write(context.HttpContext.Response.Output);
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.End();
        }
    }
}