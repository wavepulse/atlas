// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Settings;
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
}
