using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServiceWatchDog.BackgroundProcesses
{
    public class HttpHelper
    {
        public static async Task<T> GetAsync<T>(string uri)
        {
            string responseJsonString = null;

            using (var httpClient = new HttpClient())
            {
                using (var r = httpClient.GetAsync(new Uri(uri)).Result)
                {
                    responseJsonString = await r.Content.ReadAsStringAsync().ConfigureAwait(false);
                    r.EnsureSuccessStatusCode();
                    T result = JsonConvert.DeserializeObject<T>(responseJsonString);

                    return result;
                }
            }
        }

        public static async Task<HttpResponseMessage> GetFullResponseAsync(string uri)
        {


            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(new Uri(uri)))
                {

                    return response;
                }
            }
        }

        public static async Task<HttpWebResponse> GetWebRequest(string uri)
        {
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create(uri);
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            return response;
        }

        public static async Task<T> PutAsync<T>(string uri, T obj)
        {
            string responseJsonString = null;

            using (var httpClient = new HttpClient())
            {
                var content = JsonConvert.SerializeObject(obj);

                using (var r = httpClient.PostAsync(new Uri(uri), new StringContent(content, Encoding.UTF8, "text/json")).Result)
                {
                    responseJsonString = await r.Content.ReadAsStringAsync().ConfigureAwait(false);
                    r.EnsureSuccessStatusCode();
                    T result = JsonConvert.DeserializeObject<T>(responseJsonString);

                    return result;
                }
            }
        }
    }
}
