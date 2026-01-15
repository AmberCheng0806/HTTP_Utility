using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTTP_Utility
{
    internal class BaseHttpHandler : DelegatingHandler
    {
        private Token Token = new Token();
        public BaseHttpHandler(HttpClientHandler clientHandler)
        {
            InnerHandler = clientHandler;
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await Token.GetAccessToken();
            request.Headers.Add("Authorization", $"Bearer {token}");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
