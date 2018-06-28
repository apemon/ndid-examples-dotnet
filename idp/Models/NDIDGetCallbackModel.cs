using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class NDIDGetCallbackModel
    {
        [DataMember(Name = "incoming_request_url")]
        public string RequestUrl { get; set; }

        [DataMember(Name = "identity_result_url")]
        public string IdentityUrl { get; set; }

        [DataMember(Name = "accessor_sign_url")]
        public string AccessorUrl { get; set; }
    }
}
