using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace idp.Models
{
    [DataContract]
    public class NDIDUserModel
    {
        [DataMember(Name = "namespace")]
        public string NameSpace { get; set; }

        [DataMember(Name = "identifier")]
        public string Identifier { get; set; }

        [DataMember(Name = "accessors")]
        public List<NDIDAccessorModel> Accessors { get; set; }

        public NDIDUserModel()
        {
            Accessors = new List<NDIDAccessorModel>();
        }

        public void AddAccessor(string accessorId, NDIDAccessorModel model)
        {
            Accessors.Add(model);
        }

        public string GetSId(string accessorId)
        {
            return NameSpace + "-" + Identifier + "-" + accessorId;
        } 
    }
}
