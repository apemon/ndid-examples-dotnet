using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using idp.Models;
using idp.Utils;
using Newtonsoft.Json;

namespace idp.Services
{
    public class NDIDService : INDIDService
    {
        private IDPKIService _dpki;
        private IConfigurationService _config;
        private string _apiServerAddress;

        public NDIDService(IDPKIService dpki, IConfigurationService config)
        {
            _dpki = dpki;
            _config = config;
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
            return await _dpki.Sign(key, text);
        }

        public async Task<string> CreateNewIdentity(NewIdentityModel iden)
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
            newIdentity.IAL = 2.3m;
            // 4. check response from api reqeust
            using(HttpClient client = new HttpClient())
            {
                Uri url = new Uri(_apiServerAddress +  "/identity");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string jsonContent = JsonConvert.SerializeObject(newIdentity);
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var result = client.PostAsync(url, content).Result;
                if (!result.IsSuccessStatusCode) throw new Exception();
            }
            // the api server will callback for assertsor to sign the transaction
            throw new NotImplementedException();
        }
    }
}
