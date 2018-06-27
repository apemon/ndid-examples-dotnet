using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class NewIdentityModel
    {
        [DataMember(Name = "namespace")]
        public string NameSpace { get; set; }

        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        [DataMember(Name = "reference_id")]
        public string ReferenceId { get; set; }

        [DataMember(Name = "accessor_type")]
        public string AccessorType { get; set; }

        [DataMember(Name = "accessor_public_key")]
        public string AccessorPubKey { get; set; }

        [DataMember(Name = "ial")]
        public decimal IAL { get; set; }
    }
}
