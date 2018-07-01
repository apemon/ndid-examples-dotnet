using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class AccessorSignRequestModel
    {
        [DataMember(Name = "sid_hash")]
        public string SIdHash { get; set; }

        [DataMember(Name = "sid")]
        public string SId { get; set; }

        [DataMember(Name = "hash_method")]
        public string HashMethod { get; set; }

        [DataMember(Name = "keyType")]
        public string KeyMethod { get; set; }

        [DataMember(Name = "sign_method")]
        public string SignMethod { get; set; }

        [DataMember(Name = "accessor_id")]
        public string AccessorId { get; set; }

        [DataMember(Name = "reference_id")]
        public string ReferenceId { get; set; }
    }
}
