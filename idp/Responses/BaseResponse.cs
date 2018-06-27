using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Responses
{
    [DataContract]
    public class BaseResponse
    {
        [DataMember(Name = "Error")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "ErrorDesc")]
        public string ErrorDescription { get; set; }
    }
}
