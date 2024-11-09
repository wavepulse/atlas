// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Atlas.Web.App.Stores.Countries;

internal static class CountryActions
{
    public sealed record Lookup;

    public sealed record LookupResult(CountryLookupResponse[] Countries);
}
