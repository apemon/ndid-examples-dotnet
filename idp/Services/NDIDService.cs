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
            //_db.SaveUser(newUser);
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
                if (!result.IsSuccessStatusCode) throw new Exception();
            }
        }

        /// <summary>
        /// this method will handle identity request response from ndid api. It will do as follows
        /// - check for existing user
        /// - get accessor_id and store it
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void HandleCreateIdentityRequestCallback(NDIDCallbackRequestModel model)
        {
            if (model.IsSuccess)
            {
                _db.SaveReference(model.ReferenceId, "request_id", model.RequestId);
                _db.SaveReference(model.ReferenceId, "accessor_id", model.AccessorId);
            } else
            {
                _db.RemoveReference(model.ReferenceId);
            }
        }

        public async Task HandleCreateIdentityResultCallbackAsync(NDIDCallbackRequestModel model)
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

        void INDIDService.HandleCreateIdentityResultCallback(NDIDCallbackRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Task HandleCreateIdentityRequestCallbackAsync(NDIDCallbackRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
