// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Atlas.Web.App.Stores.Countries;

internal static class CountryActions
{
    public sealed record Randomize;

    public sealed record RandomizeResult(RandomizedCountryResponse Country);

    public sealed record Guess(string GuessedCca2, string RandomizedCca2);

    public sealed record GuessResult(GuessedCountryResponse Country);

    public sealed record Lookup;

    public sealed record LookupResult(CountryLookupResponse[] Countries);
}
