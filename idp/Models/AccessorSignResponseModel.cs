using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class AccessorSignResponseModel
    {
        [DataMember(Name = "signature")]
        public string Signature { get; set; }
    }
}
