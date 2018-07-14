using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class NDIDIDPResponseModel
    {

        [DataMember(Name = "reference_id")]
        public string ReferenceId { get; set; }

        [DataMember(Name = "request_id")]
        public string RequestId { get; set; }

        [DataMember(Name = "namespace")]
        public string NameSpace { get; set; }

        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        [DataMember(Name = "callback_url")]
        public string CallbackUrl { get; set; }

        [DataMember(Name = "accessor_id")]
        public string AccessorId { get; set; }

        [DataMember(Name = "ial")]
        public decimal IAL { get; set; }

        [DataMember(Name = "aal")]
        public decimal AAL { get; set; }

        [DataMember(Name = "secret")]
        public string Secret { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "signature")]
        public string Signature { get; set; }
    }
}
