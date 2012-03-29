using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CloudSecurityTokenService.Infrastructure
{
    public interface IFormsAuthentication
    {
        void SetAuthCookie(string name);
        void SignOut();
    }

    public class FormsAuthenticationWrapper : IFormsAuthentication
    {
        public void SetAuthCookie(string name)
        {
            FormsAuthentication.SetAuthCookie(name, false);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}