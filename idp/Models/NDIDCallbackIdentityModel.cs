using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class NDIDCallbackIdentityModel
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "success")]
        public bool IsSuccess { get; set; }

        [DataMember(Name = "exist")]
        public bool IsExist { get; set; }

        [DataMember(Name = "reference_id")]
        public string ReferenceId { get; set; }

        [DataMember(Name = "request_id")]
        public string RequestId { get; set; }

        [DataMember(Name = "accessor_id")]
        public string AccessorId { get; set; }

        [DataMember(Name = "secret")]
        public string Secret { get; set; }

        [DataMember(Name = "error")]
        public NDIDErrorModel Error { get; set; }
    }
}
