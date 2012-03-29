using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ActiveDirectoryForCloud.Proxy
{
    [ServiceContract(Namespace = Constants.AuthenticationNamespace)]
    public interface IAuthenticationService
    {
        [OperationContract]
        AuthenticationResponse Authenticate(AuthenticationRequest request);
    }
}
