// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Application;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediator();
        services.AddScoped<IRandomizer, Randomizer>();
        services.AddScoped<IDateHash, DateHash>();
    }
}
