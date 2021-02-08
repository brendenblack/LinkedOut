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

            var linkedoutApiUrl = builder.Configuration.GetSection("LinkedOut").GetValue<string>("Url");
            // https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-5.0
            //builder.Services.AddScoped(sp => {
            //    var client = new HttpClient()
            //    {
            //        BaseAddress = new Uri(linkedoutApiUrl),
            //    };

            //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "");

            //    return client;
            //});

            builder.Services.AddAntDesign();

            //builder.Services
            //    .AddBlazorise(options =>
            //    {
            //        options.ChangeTextOnKeyPress = true;
            //    })
            //    //.AddBulmaProviders()
            //    .AddAntDesignProviders()
            //    .AddFontAwesomeIcons();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
            });


            builder.RootComponents.Add<App>("app");

            var host = builder.Build();

            //host.Services
            //    //.UseBulmaProviders()
            //    .UseAntDesignProviders()
            //    .UseFontAwesomeIcons();

            await host.RunAsync();
        }
    }
}
