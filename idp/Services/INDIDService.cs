using idp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Services
{
    public interface INDIDService
    {
        Task CreateNewIdentity(NewIdentityModel newIdentity);
        Task<string> AccessorSign(string key, string text);
        Task<NDIDGetCallbackModel> GetCallback();
        Task SetCallback(NDIDGetCallbackModel model);
        void HandleCreateIdentityResultCallback(NDIDCallbackRequestModel model);
        void HandleCreateIdentityRequestCallback(NDIDCallbackRequestModel model);
        Task HandleCreateIdentityResultCallbackAsync(NDIDCallbackRequestModel model);
        Task HandleCreateIdentityRequestCallbackAsync(NDIDCallbackRequestModel model);
    }
}
