using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Services
{
    public interface IPersistanceStorageService
    {
        void SaveAccessorSign(string key, string sid);
        string GetAccessorSign(string id);
    }
}
