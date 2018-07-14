using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using idp.Models;
using idp.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace idp.Services
{
    public class NDIDService : INDIDService
    {
        private IDPKIService _dpki;
        private IConfigurationService _config;
        private IPersistanceStorageService _db;
        private ILogger _logger;
        private string _apiServerAddress;

        public NDIDService(IDPKIService dpki, 
            IConfigurationService config, 
            IPersistanceStorageService db,
            ILogger<NDIDService> logger)
        {
            _dpki = dpki;
            _config = config;
            _db = db;
            _logger = logger;
            _apiServerAddress = _config.GetAPIServerAddress();
        }

        public async Task<NDIDGetCallbackModel> GetCallback()
        {
            return await NDIDWebClient.GetCallback(_apiServerAddress);
        }

        public async Task SetCallback(NDIDGetCallbackModel model)
        {
            await NDIDWebClient.SetCallback(_apiServerAddress, model);
        }

        public async Task<string> AccessorSign(string key, string text)
        {
            _logger.LogInformation(text);
            string sid = _db.GetAccessorSign(key);
            _db.RemoveAccessorSign(key);
            return await _dpki.Sign(sid, text);
        }

        public async Task CreateNewIdentity(NewIdentityModel iden)
        {
            // 1. generate new keypair
            NewIdentityModel newIdentity = new NewIdentityModel();
            newIdentity.NameSpace = iden.NameSpace;
            newIdentity.Identifier = iden.Identifier;
            string sid = newIdentity.NameSpace + "-" + newIdentity.Identifier;
            await _dpki.GenNewKey(sid);
            // 2. read public key
            string pubKey = await _dpki.GetPubKey(sid);
            // 3. construct new identity api request
            newIdentity.AccessorType = "RSA";
            newIdentity.AccessorPubKey = pubKey;
            newIdentity.ReferenceId = Guid.NewGuid().ToString();
            newIdentity.CallbackUrl = new Uri(new Uri(_config.GetCallbackPath()), "api/callback/identity").ToString();
            newIdentity.IAL = 2.3m;
            _db.SaveAccessorSign(newIdentity.ReferenceId, sid);
            _db.SaveReference(newIdentity.ReferenceId, "sid", sid);
            // 4. check response from api reqeust
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri(_apiServerAddress +  "/v2/identity");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string jsonContent = JsonConvert.SerializeObject(newIdentity);
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;
                if (!result.IsSuccessStatusCode) throw new ApplicationException();
                string resultJson = await result.Content.ReadAsStringAsync();
                NDIDCallbackIdentityModel model = JsonConvert.DeserializeObject<NDIDCallbackIdentityModel>(resultJson);
                _db.SaveReference(newIdentity.ReferenceId, "accessor_id", model.AccessorId);
                _db.SaveReference(newIdentity.ReferenceId, "request_id", model.RequestId);
            }
        }

        /// <summary>
        /// this method will handle identity request response from ndid api. It will do as follows
        /// - check for existing user
        /// - get accessor_id and store it
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void HandleCreateIdentityRequestCallback(NDIDCallbackIdentityModel model)
        {
            if (model.IsSuccess)
            {
                // do nothing
            } else
            {
                _db.RemoveReference(model.ReferenceId);
            }
        }

        public async Task HandleCreateIdentityResultCallbackAsync(NDIDCallbackIdentityModel model)
        {
            if (model.IsSuccess)
            {
                string sid = _db.GetReferecne(model.ReferenceId, "sid");
                string[] parts = sid.Split('-');
                NDIDUserModel user = new NDIDUserModel();
                user.NameSpace = parts[0];
                user.Identifier = parts[1];
                string accessor_id = _db.GetReferecne(model.ReferenceId, "accessor_id");
                NDIDAccessorModel accessor = new NDIDAccessorModel();
                accessor.AccessorId = accessor_id;
                accessor.Secret = model.Secret;
                // update key
                string newKeyName = sid + "_" + "0";
                // not use base64 file name because windows cannot support filename with "/" charactor
                _dpki.UpdateKey(sid, newKeyName);
                string pubKey = await _dpki.GetPubKey(newKeyName);
                accessor.AccessorPubKey = pubKey;
                user.Accessors.Add(accessor);
                // save new user
                _db.CreateNewUser(user);
                // remove all referenceId
                _db.RemoveReference(model.ReferenceId);
            }
            else throw new ApplicationException();
        }

        void INDIDService.HandleCreateIdentityResultCallback(NDIDCallbackIdentityModel model)
        {
            throw new NotImplementedException();
        }

        public Task HandleCreateIdentityRequestCallbackAsync(NDIDCallbackIdentityModel model)
        {
            throw new NotImplementedException();
        }

        public void HandleIncomingRequestCallback(NDIDCallbackRequestModel model)
        {
            // check that user exist
            NDIDUserModel user = _db.FindUser(model.Namespace, model.Identifier);
            if (user == null) throw new ApplicationException();
            _db.SaveUserRequest(model.Namespace, model.Identifier, model.RequestId, model);
        }

        public Task HandleIncomingRequestCallbackAsync(NDIDCallbackRequestModel model)
        {
            throw new NotImplementedException();
        }

        public List<NDIDCallbackRequestModel> ListUserRequest(string namespaces, string identifier)
        {
            return _db.ListUserRequest(namespaces, identifier);
        }

        public async Task CreateIDPResponse(string namespaces, string identifier, string requestId, string status)
        {
            // get user from parameter
            NDIDUserModel user = _db.FindUser(namespaces, identifier);
            if (user == null) throw new ApplicationException();
            // get request
            NDIDCallbackRequestModel request = _db.GetUserRequest(namespaces, identifier, requestId);
            if (request == null) throw new ApplicationException();
            // get key and sign message
            // always use first accessor keu for simplicity
            string keyName = namespaces + "-" + identifier + "-" + "0";
            string signature = await _dpki.Sign(keyName, request.RequestMsgHash);
            // construct idp response model
            NDIDIDPResponseModel model = new NDIDIDPResponseModel();
            model.ReferenceId = Guid.NewGuid().ToString();
            model.RequestId = request.RequestId;
            model.CallbackUrl = new Uri(new Uri(_config.GetCallbackPath()), "api/callback/response").ToString();
            model.NameSpace = user.NameSpace;
            model.Identifier = user.Identifier;
            model.AccessorId = user.Accessors[0].AccessorId;
            model.Secret = user.Accessors[0].Secret;
            model.Signature = signature;
            model.Status = status;
            model.IAL = 2.3m;
            model.AAL = 3.0m;
            // call ndid api
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri(_apiServerAddress + "/v2/idp/response");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string jsonContent = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;
                if (!result.IsSuccessStatusCode) throw new ApplicationException();
                string resultJson = await result.Content.ReadAsStringAsync();
                _db.RemoveUserRequest(user.NameSpace, user.Identifier, request.RequestId);
            }
        }
    }
}
