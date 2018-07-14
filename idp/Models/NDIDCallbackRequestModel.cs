using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class NDIDCallbackRequestModel
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "mode")]
        public int Mode { get; set; }

        [DataMember(Name = "namespace")]
        public string Namespace { get; set; }

        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        [DataMember(Name = "request_id")]
        public string RequestId { get; set; }

        [DataMember(Name = "request_message")]
        public string RequestMsg { get; set; }

        [DataMember(Name = "request_message_hash")]
        public string RequestMsgHash { get; set; }

        [DataMember(Name = "requester_node_id")]
        public string RequesterNodeId { get; set; }

        [DataMember(Name = "min_ial")]
        public decimal MinIAL { get; set; }

        [DataMember(Name = "min_aal")]
        public decimal MinAAL { get; set; }

        [DataMember(Name = "data_request_list")]
        public List<NDIDDataRequestModel> DataRequests { get; set; }

        [DataMember(Name ="error")]
        public NDIDErrorModel Error { get; set; }

        public NDIDCallbackRequestModel()
        {
            DataRequests = new List<NDIDDataRequestModel>();
        }
    }
}
