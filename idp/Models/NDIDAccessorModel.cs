using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class NDIDAccessorModel
    {
        [DataMember(Name = "accessor_id")]
        public string AccessorId { get; set; }

        [DataMember(Name = "accessor_public_key")]
        public string AccessorPubKey { get; set; }

        [DataMember(Name = "secret")]
        public string Secret { get; set; }
    }
}
