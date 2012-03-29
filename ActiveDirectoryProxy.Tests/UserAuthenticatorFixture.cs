using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ActiveDirectoryForCloud.Proxy;
using System.DirectoryServices.AccountManagement;

namespace ActiveDirectoryProxy.Tests
{
    public class UserAuthenticatorFixture
    {
        public UserAuthenticatorFixture()
        {
            var identity = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine), IdentityType.SamAccountName, "adtest");
            if (identity == null)
            {
                var principal = new UserPrincipal(new PrincipalContext(ContextType.Machine));
                principal.SamAccountName = "adtest";
                principal.DisplayName = "ad test";
                principal.Save();
                principal.SetPassword("password");
            }
        }

        [Fact]
        public void when_authenticating_user_with_right_password_true_is_returned()
        {
            var authenticator = new UserAuthenticator();
            var result = authenticator.Authenticate("adtest", "password");

            Assert.True(result);
        }

        [Fact]
        public void when_authenticating_user_with_wrong_password_false_is_returned()
        {
            var authenticator = new UserAuthenticator();
            var result = authenticator.Authenticate("adtest", "foo");

            Assert.False(result);
        }
    }
}
