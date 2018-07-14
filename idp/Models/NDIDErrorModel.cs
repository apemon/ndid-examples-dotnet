using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class NDIDErrorModel
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "stack")]
        public string Stack { get; set; }
    }
}
