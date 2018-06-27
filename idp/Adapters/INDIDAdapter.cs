using idp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Adapters
{
    interface INDIDAdapter
    {
        Task<string> CreateNewIdentity(NewIdentityModel newIdentity);
        Task<string> AccessorSign(string key, string text);
    }
}
