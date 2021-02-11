using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LinkedOut.BlazorWasm
{
    public abstract class SwaggerClientBase
    {
        public string Jwt { get; private set; }

        public void SetJwt(string token)
        {
            Jwt = token;
        }

        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            var message = new HttpRequestMessage();
            //message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Jwt);
            return Task.FromResult(message);
        }
    }
}
