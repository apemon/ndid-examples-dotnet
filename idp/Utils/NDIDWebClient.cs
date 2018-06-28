using idp.Exceptions;
using idp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace idp.Utils
{
    public class NDIDWebClient
    {
        public static async Task<NDIDGetCallbackModel> GetCallback(string baseAddress)
        {
            string url = new Uri(new Uri(baseAddress), "idp/callback").ToString();
            string result = await _CallAPI("GET", url);
            NDIDGetCallbackModel response = JsonConvert.DeserializeObject<NDIDGetCallbackModel>(result);
            return response;
        }

        private static async Task<string> _CallAPI(string method, string url, string jsonBody = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = null;
                if(jsonBody != null) content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                if (method == "GET") response = client.GetAsync(url).Result;
                else if (method == "POST") response = client.PostAsync(url, content).Result;
                else
                {
                    APIInvalidException ex = new APIInvalidException("Invalid method");
                    ex.URL = url;
                    throw ex;
                }
                string responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    APIInvalidException ex = new APIInvalidException();
                    ex.StatusCode = response.StatusCode.ToString();
                    ex.URL = url;
                    ex.Content = responseBody;
                    throw ex;
                }
                else return responseBody;
            }
        }
    }
}
