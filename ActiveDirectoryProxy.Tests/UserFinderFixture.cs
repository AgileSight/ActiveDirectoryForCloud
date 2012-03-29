using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;
using Xunit;
using ActiveDirectoryForCloud.Proxy;

namespace ActiveDirectoryProxy.Tests
{
    public class UserFinderFixture
    {
        public UserFinderFixture()
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
        public void when_retrieving_existing_user_include_attributes()
        {
            var finder = new UserFinder(ContextType.Machine);
            var attributes = finder.GetAttributes("adtest");
            
            Assert.True(attributes.Any(a => a.Name == "DisplayName" && a.Value == "ad test"));
        }
    }
}
