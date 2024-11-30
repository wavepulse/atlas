// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Repositories;
using Atlas.Infrastructure.Changelog.Options;
using Atlas.Infrastructure.Changelog.Repositories;
using Atlas.Infrastructure.Changelog.Sources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Changelog;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    internal static void AddChangelog(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ChangelogOptions>(configuration.GetSection(ChangelogOptions.Section))
                .AddSingleton<IValidateOptions<ChangelogOptions>, ChangelogOptions.Validator>()
                .AddSingleton(sp => sp.GetRequiredService<IOptions<ChangelogOptions>>().Value);

        services.AddHttpClient<IChangelogClient, ChangelogClient>();

        services.AddTransient<IChangelogRepository, ChangelogRepository>();
    }
}
