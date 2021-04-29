using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.DynamicForms.Client.Extensions;
using Blazor.DynamicForms.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;

namespace Blazor.DynamicForms.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddLocalization();

            builder.Services.AddScoped(
                sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
            builder.Services.AddScoped<DynamicControlDataService>();
            builder.Services.AddScoped<DynamicFormGeneratorService>();
            builder.Services.AddMudServices();
            builder.Services.AddMudBlazorResizeListener();
            builder.Services.AddMudBlazorSnackbar(config =>
            {
                config.PositionClass = Defaults.Classes.Position.TopCenter;
                config.PreventDuplicates = false;
                config.NewestOnTop = false;
                config.ShowCloseIcon = false;
                config.VisibleStateDuration = 1000;
                config.HideTransitionDuration = 500;
                config.ShowTransitionDuration = 500;
            });


            var host = builder.Build();
            await host.SetDefaultCulture();
            await host.RunAsync();
        }
    }
}