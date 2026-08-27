using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Utility
{
    public class Interceptor
    {
        public Func<HttpRequestMessage, Task> RequestHandlerAsync { get; set; }
        public Action<HttpRequestMessage> RequestHandler { get; set; }
        public async Task AddRequestAsync(HttpRequestMessage request)
        {
            if (RequestHandlerAsync != null) await RequestHandlerAsync.Invoke(request);
        }
        public void AddRequest(HttpRequestMessage request)
        {
            RequestHandler?.Invoke(request);
        }
    }
}
