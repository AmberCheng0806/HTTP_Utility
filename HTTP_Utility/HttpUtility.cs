using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Utility
{
    internal class HttpUtility : IHttpRequest
    {
        private HttpClient client = new HttpClient();

        public string BaseUrl
        {
            set => client.BaseAddress = new Uri(value = value.EndsWith("/") ? value : value + "/");
        }
        public string Token { get => Token; set => AddHeaders("Authorization", $"Bearer {Token}"); }

        //public async Task<T> GetAsync<T>(string url)
        //{

        //    var response = await client.GetAsync(url);
        //    var responseString = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<T>(responseString);
        //}

        //public async Task<T> PostAsync<T>(string url, object payload)
        //{
        //    var json = JsonConvert.SerializeObject(payload);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await client.PostAsync(url, content);
        //    var responseString = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<T>(responseString);
        //}

        //public async Task<T> PutAsync<T>(string url, object payload)
        //{
        //    var json = JsonConvert.SerializeObject(payload);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await client.PutAsync(url, content);
        //    var responseString = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<T>(responseString);
        //}

        //public async Task<T> DeleteAsync<T>(string url)
        //{
        //    var response = await client.DeleteAsync(url);
        //    var responseString = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<T>(responseString);
        //}

        public void AddHeaders(string key, string value)
        {
            client.DefaultRequestHeaders.Add(key, value);
        }

        public async Task<string> GetAsync(string url)
        {
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public async Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> urlParam = null)
        {
            var responseString = await GetAsync(FormatUrl(url, urlParam));
            return JsonConvert.DeserializeObject<TResult>(responseString);
        }

        private string FormatUrl(string url, Dictionary<string, string> urlParam)
        {
            if (urlParam == null)
            {
                return url;
            }
            //url?key=value&key2=value2
            var urlParams = string.Join("&", urlParam.Select(x => $"{x.Key}={x.Value}"));
            return $"{url}?{urlParams}";
        }

        public async Task<string> PostAsync(string url, object input)
        {
            var json = JsonConvert.SerializeObject(input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public async Task<TResult> PostAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null)
        {
            var responseString = await PostAsync(FormatUrl(url, urlParam), input);
            return JsonConvert.DeserializeObject<TResult>(responseString);
        }

        public async Task<TResult> PostAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null)
        {
            var responseString = await PostAsync(FormatUrl(url, urlParam), input);
            return JsonConvert.DeserializeObject<TResult>(responseString);
        }

        public async Task<string> PatchAsync(string url, object input)
        {
            var json = JsonConvert.SerializeObject(input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var httpMessage = new HttpRequestMessage(new HttpMethod("Patch"), url);
            var response = await client.SendAsync(httpMessage);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
        public async Task<TResult> PatchAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null)
        {
            var responseString = await PatchAsync(FormatUrl(url, urlParam), input);
            return JsonConvert.DeserializeObject<TResult>(responseString);
        }

        public async Task<TResult> PatchAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null)
        {
            var responseString = await PatchAsync(FormatUrl(url, urlParam), input);
            return JsonConvert.DeserializeObject<TResult>(responseString);
        }

        public async Task<string> PutAsync(string url, object input)
        {
            var json = JsonConvert.SerializeObject(input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public async Task<TResult> PutAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null)
        {
            var responseString = await PutAsync(FormatUrl(url, urlParam), input);
            return JsonConvert.DeserializeObject<TResult>(responseString);
        }

        public async Task<string> DeleteAsync(string url, Dictionary<string, string> urlParam = null)
        {
            var response = await client.DeleteAsync(FormatUrl(url, urlParam));
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

    }
}
