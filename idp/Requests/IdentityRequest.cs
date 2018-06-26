using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Requests
{
    [DataContract]
    public class IdentityRequest
    {
        [DataMember(Name = "namespace")]
        public string NameSpace { get; set; }

        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }
    }
}
