using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class NDIDDataRequestModel
    {
        [DataMember(Name = "service_id")]
        public string ServiceId { get; set; }

        [DataMember(Name = "as_id_list")]
        public List<string> ASIdList { get; set; }

        [DataMember(Name = "min_as")]
        public int MinAS { get; set; }

        // request param ??
    }
}
