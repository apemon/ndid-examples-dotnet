using idp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Services
{
    public interface INDIDService
    {
        Task<string> CreateNewIdentity(NewIdentityModel newIdentity);
        Task<string> AccessorSign(string key, string text);
        Task<NDIDGetCallbackModel> GetCallback();
        Task SetCallback(NDIDGetCallbackModel model);
        Task HandleCreateIdentityResultCallback(NDIDCallbackRequestModel model);
        Task HandleCreateIdentityRequestCallback(NDIDCallbackRequestModel model);
    }
}
