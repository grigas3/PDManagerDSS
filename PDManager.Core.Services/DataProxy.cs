using Newtonsoft.Json;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Common.Results;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PDManager.Core.Services
{
    /// <summary>
    /// Data Proxy
    /// </summary>
    public class DataProxy : IDataProxy
    {

        #region Private declarations
        private const string BaseAddress = "https://pdmanager.3dnetmedical.com";

        private readonly IProxyCredientialsProvider _credientialsProvider;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credientialsProvider"></param>
        public DataProxy(IProxyCredientialsProvider credientialsProvider)
        {


            this._credientialsProvider = credientialsProvider;

        }


        /// <summary>
        /// This method is used to get access token from pdn
        /// </summary>
        /// <returns>Access token</returns>
        public async Task<string> GetAccessToken()
        {
            string loginUrl = BaseAddress+"/oauth/token";
            HttpClient client = null;

            try
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(loginUrl);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                //            client.DefaultRequestHeaders.Add("Content-Encoding", "application/x-www-form-urlencoded");

                string urlParameters = "username=" + _credientialsProvider.GetUserName() + "&password=" + _credientialsProvider.GetPassword() + "&grant_type=password";
                // List data response.
                HttpResponseMessage response = client.PostAsync(loginUrl, new StringContent(urlParameters)).Result;  // Blocking call!
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    //var res = response.Content.ReadAsAsync<LoginResult>().Result;
                    var str = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<LoginResult>(str);

                    return res.access_token;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client?.Dispose();

            }

            return null;
        }

        private string GetUrl(string url, string id)
        {
            StringBuilder str = new StringBuilder();
            str.Append(url);
            str.Append(String.Format("/{0}", id));

            return (str.ToString());
        }

        private string GetUrl(string url, int take, int skip, string filter, string sort, string sortdir, long lastmodified = -1)
        {
            StringBuilder str = new StringBuilder();
            str.Append(url);
            str.Append(String.Format("/find?take={0}&skip={1}&sort={2}&sortdir={3}", take, skip, sort, sortdir));
            //&sort = &sortdir = false & lastmodified = &_ = 1483996288701
            if (!string.IsNullOrEmpty(filter))
                str.Append(String.Format("&filter={0}", (filter)));
            else str.Append(String.Format("&filter="));

            if (lastmodified > 0)
            {
                str.Append(String.Format("&lastmodified={0}", lastmodified));
            }
            else
                str.Append("&lastmodified=");

            return (str.ToString());
        }

    
        /// <summary>
        /// Get Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>        
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <param name="sortdir"></param>
        /// <param name="lastmodified"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Get<T>(string uri, int take, int skip, string filter, string sort, string sortdir = "false", long lastmodified = -1) where T : class
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }


            //First Get Access token
            var    accessToken = await GetAccessToken();


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            // List data response.
            HttpResponseMessage response = await client.GetAsync(GetUrl(uri, take, skip, filter, sort, sortdir));// new StringContent(jsonRequest, Encoding.UTF8, "application/json")).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                try
                {
                    // Parse the response body. Blocking!
                    IEnumerable<T> jsonResponse = JsonConvert.DeserializeObject<IEnumerable<T>>(res);

                    return jsonResponse;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                throw new Exception();
            }
        }

        /// <summary>
        /// Get A single Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="id"></param>
        
        /// <returns></returns>
        public async Task<T> Get<T>(string uri, string id)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            //First Get Access token
            var accessToken = await GetAccessToken();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            // List data response.
            HttpResponseMessage response = await client.GetAsync(GetUrl(uri, id));// new StringContent(jsonRequest, Encoding.UTF8, "application/json")).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                // Parse the response body. Blocking!
                T jsonResponse = JsonConvert.DeserializeObject<T>(res);

                return jsonResponse;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                throw new Exception();
            }
        }



        /// <summary>
        /// Insert into PDManager repository
        /// </summary>
        /// <typeparam name="T">Object Template</typeparam>
        /// <param name="uri">Url</param>
        
        /// <param name="item"> Item</param>
        /// <returns></returns>
        public async Task<bool> Insert<T>(string uri, T item)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseAddress);
            //First Get Access token
            var accessToken = await GetAccessToken();

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            string jsonRequest = JsonConvert.SerializeObject(item);
            // List data response.
            HttpResponseMessage response =await client.PostAsync(uri, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
           

        }
    }
}