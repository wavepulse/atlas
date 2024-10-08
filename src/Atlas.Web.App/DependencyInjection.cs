// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Options;
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

    internal static void AddOptions(this WebAssemblyHostBuilder builder)
    {
        _ = builder.Services.Configure<ProjectOptions>(builder.Configuration.GetSection(ProjectOptions.Section))
                            .AddSingleton<IValidateOptions<ProjectOptions>, ProjectOptions.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<ProjectOptions>>().Value);

        _ = builder.Services.Configure<CompanyOptions>(builder.Configuration.GetSection(CompanyOptions.Section))
                            .AddSingleton<IValidateOptions<CompanyOptions>, CompanyOptions.Validator>()
                            .AddSingleton(sp => sp.GetRequiredService<IOptions<CompanyOptions>>().Value);
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
