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
        void HandleCreateIdentityResultCallback(NDIDCallbackIdentityModel model);
        void HandleCreateIdentityRequestCallback(NDIDCallbackIdentityModel model);
        void HandleIncomingRequestCallback(NDIDCallbackRequestModel model);
        void HandleResponseResultCallback(NDIDCallbackIdentityModel model);
        Task HandleCreateIdentityResultCallbackAsync(NDIDCallbackIdentityModel model);
        Task HandleCreateIdentityRequestCallbackAsync(NDIDCallbackIdentityModel model);
        Task HandleIncomingRequestCallbackAsync(NDIDCallbackRequestModel model);
        Task HandleResponseResultCallbackAsync(NDIDCallbackIdentityModel model);
        List<NDIDCallbackRequestModel> ListUserRequest(string namespaces, string identifier);
        Task CreateIDPResponse(string namespaces, string identifier, string requestId, string status);
        List<NDIDUserModel> ListUser();
    }
}
