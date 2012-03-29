using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.DirectoryServices.AccountManagement;

namespace ActiveDirectoryForCloud.Proxy
{
    [ServiceBehavior(Namespace = "http://agilesight.com/active_directory/agent")]
    public class ProxyService : IAuthenticationService
    {
        IUserFinder userFinder;
        IUserAuthenticator userAuthenticator;

        public ProxyService()
            : this(new UserFinder(), new UserAuthenticator())
        {
        }

        public ProxyService(IUserFinder userFinder, IUserAuthenticator userAuthenticator)
        {
            this.userFinder = userFinder;
            this.userAuthenticator = userAuthenticator;
        }

        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            if (userAuthenticator.Authenticate(request.Username, request.Password))
            {
                return new AuthenticationResponse
                {
                    Result = true,
                    Attributes = this.userFinder.GetAttributes(request.Username)
                };    
            }

            return new AuthenticationResponse { Result = false };
        }
    }
}
