using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Utility
{
    public interface IHttpRequest
    {
        Task<string> GetAsync(string url);
        Task<TResult> GetAsync<TResult>(string url, Dictionary<string, string> urlParam = null);
        Task<string> PostAsync(string url, object input);
        Task<TResult> PostAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null);
        Task<TResult> PostAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null);

        Task<string> PatchAsync(string url, object input);
        Task<TResult> PatchAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string> urlParam = null);
        Task<TResult> PatchAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null);
        Task<string> PutAsync(string url, object input);
        Task<TResult> PutAsync<TResult>(string url, object input, Dictionary<string, string> urlParam = null);
        Task<string> DeleteAsync(string url, Dictionary<string, string> urlParam = null);

        void AddHeaders(string key, string value);

        String BaseUrl { set; }
        String Token { set; get; }

    }
}
