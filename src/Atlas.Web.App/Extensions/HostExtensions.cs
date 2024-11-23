// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Settings;
using Atlas.Web.App.Storages;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Atlas.Web.App.Extensions;

[ExcludeFromCodeCoverage]
internal static class HostExtensions
{
    public static WebAssemblyHost UseLocalization(this WebAssemblyHost host)
    {
        ILocalStorage localStorage = host.Services.GetRequiredService<ILocalStorage>();
        AppSettings? settings = localStorage.GetItem<AppSettings>(LocalStorageKeys.Settings);

        if (settings is null)
            return host;

        CultureInfo culture = new(settings.General.Language switch
        {
            Language.English => "en",
            Language.French => "fr-CA",
            _ => "en"
        });

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        return host;
    }
}
