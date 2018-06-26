using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Requests
{
    [DataContract]
    public class AuthenticationRequest
    {
        [DataMember(Name = "requestId")]
        public string RequestId { get; set; }
    }
}
