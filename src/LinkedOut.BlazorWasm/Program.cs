using Blazorise;
using Blazorise.AntDesign;
using Blazorise.Bulma;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LinkedOut.BlazorWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
           
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var linkedoutApiUrl = builder.Configuration.GetSection("LinkedOut").GetValue<string>("RootUrl");
            var linkedoutApiPath = builder.Configuration.GetSection("LinkedOut").GetValue<string>("ApiPath");

            // https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-5.0
            // https://code-maze.com/using-access-token-with-blazor-webassembly-httpclient/
            builder.Services.AddHttpClient<JobSearchClient>("jobSearchClient", client => client.BaseAddress = new Uri($"{linkedoutApiUrl}/{linkedoutApiPath}"))
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>();
                    handler.ConfigureHandler(
                        authorizedUrls: new[] { linkedoutApiUrl },
                        scopes: new[] { "linkedoutapi" }
                    );
                    return handler;
                });
            builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("jobSearchClient"));
            
            builder.Services.AddHttpClient<JobApplicationClient>("jobApplicationClient", client => client.BaseAddress = new Uri($"{linkedoutApiUrl}/{linkedoutApiPath}"))
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>();
                    handler.ConfigureHandler(
                        authorizedUrls: new[] { linkedoutApiUrl },
                        scopes: new[] { "linkedoutapi" }
                    );
                    return handler;
                });
            builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("jobApplicationClient"));

            builder.Services.AddAntDesign();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
            });


            builder.RootComponents.Add<App>("app");

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}
