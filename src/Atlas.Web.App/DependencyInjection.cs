// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Settings;
using Fluxor;
#if DEBUG
using Fluxor.Blazor.Web.ReduxDevTools;
#endif
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Atlas.Web.App;

internal static class DependencyInjection
{
    internal static void AddJsRuntime(this WebAssemblyHostBuilder builder)
        => builder.Services.AddSingleton(sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>());

    internal static void AddSettings(this WebAssemblyHostBuilder builder)
    {
        _ = builder.Services.Configure<ProjectSettings>(builder.Configuration.GetSection(ProjectSettings.Section))
                            .AddSingleton<IValidateOptions<ProjectSettings>, ProjectSettings.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<ProjectSettings>>().Value);

        _ = builder.Services.Configure<CompanySettings>(builder.Configuration.GetSection(CompanySettings.Section))
                            .AddSingleton<IValidateOptions<CompanySettings>, CompanySettings.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<CompanySettings>>().Value);
    }

    internal static void ConfigureLoggings(this WebAssemblyHostBuilder builder)
    {
        if (!builder.HostEnvironment.IsProduction())
            return;

        _ = builder.Logging.ClearProviders();
    }

    internal static void AddFluxor(this WebAssemblyHostBuilder builder)
    {
        _ = builder.Services.AddFluxor(options =>
        {
            _ = options.ScanAssemblies(typeof(Program).Assembly);
#if DEBUG
            _ = options.UseReduxDevTools(o => o.Name = "Atlas");
#endif
        });
    }
}
