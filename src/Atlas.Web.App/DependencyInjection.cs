// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Options;
using Atlas.Web.App.Services;
using Atlas.Web.App.Storages;
using Fluxor;
#if DEBUG
using Fluxor.Blazor.Web.ReduxDevTools;
#endif
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Web.App;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    internal static void AddServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddSingleton(sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>());
        builder.Services.AddLocalization();

        builder.Services.AddTransient<ITimeService, TimeService>();
        builder.Services.AddSingleton<ILocalStorage, LocalStorage>();
    }

    internal static void AddOptions(this WebAssemblyHostBuilder builder)
    {
        builder.Services.Configure<ProjectOptions>(builder.Configuration.GetSection(ProjectOptions.Section))
                        .AddSingleton<IValidateOptions<ProjectOptions>, ProjectOptions.Validator>()
                        .AddSingleton(sp => sp.GetRequiredService<IOptions<ProjectOptions>>().Value);

        builder.Services.Configure<CompanyOptions>(builder.Configuration.GetSection(CompanyOptions.Section))
                        .AddSingleton<IValidateOptions<CompanyOptions>, CompanyOptions.Validator>()
                        .AddSingleton(sp => sp.GetRequiredService<IOptions<CompanyOptions>>().Value);
    }

    internal static void ConfigureLoggings(this WebAssemblyHostBuilder builder)
    {
        if (!builder.HostEnvironment.IsProduction())
            return;

        builder.Logging.ClearProviders();
    }

    internal static void AddFluxor(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddFluxor(options =>
        {
            options.ScanAssemblies(typeof(DependencyInjection).Assembly);
#if DEBUG
            options.UseReduxDevTools(o => o.Name = "Atlas");
#endif
        });
    }
}
