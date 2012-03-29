using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ActiveDirectoryForCloud.Proxy
{
    [DataContract(Namespace = Constants.AuthenticationNamespace)]
    public class AuthenticationRequest
    {
        [DataMember(Order=1)]
        public string Username { get; set; }

        [DataMember(Order=2)]
        public string Password { get; set; }
    }
}
