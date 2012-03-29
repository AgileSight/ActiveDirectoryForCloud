using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ActiveDirectoryForCloud.Proxy
{
    [DataContract(Namespace = Constants.AuthenticationNamespace)]
    public class AuthenticationResponse
    {
        [DataMember(Order=1)]
        public bool Result { get; set; }

        [DataMember(Order=2)]
        public UserAttribute[] Attributes { get; set; }
    }

    [DataContract(Namespace = Constants.AuthenticationNamespace)]
    [Serializable]
    public class UserAttribute
    {
        [DataMember(Order=1)]
        public string Name { get; set; }
        
        [DataMember(Order=2)]
        public string Value { get; set; }
    }

    
}
