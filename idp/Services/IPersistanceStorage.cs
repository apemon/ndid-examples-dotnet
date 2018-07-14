using idp.Models;
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
        void SaveReference(string referenceId, string type, string value);
        string GetReferecne(string referenceId, string type);
        void RemoveReference(string referenceId);
        long CreateNewUser(NDIDUserModel user);
        NDIDUserModel FindUser(string namespaces, string identifier);
    }
}
