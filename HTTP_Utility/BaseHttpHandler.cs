using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTTP_Utility
{
    internal class BaseHttpHandler : DelegatingHandler
    {
        //private Token Token = new Token();
        private Interceptor Interceptor;

        public BaseHttpHandler(bool IsProxy, Interceptor interceptor)
        {
            InnerHandler = IsProxy ? new HttpClientHandler()
            {
                UseProxy = true,
                Proxy = new WebProxy("http://127.0.0.1:8888"),

            } : new HttpClientHandler();
            Interceptor = interceptor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var token = await Token.GetAccessToken();
            //request.Headers.Add("Authorization", $"Bearer {token}");
            if (Interceptor != null)
                await Interceptor.AddRequest(request);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
