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
        public Func<HttpRequestMessage, Task> Func { get; set; }
        public async Task AddRequest(HttpRequestMessage request)
        {
            await Func?.Invoke(request);
        }
    }
}
