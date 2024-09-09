// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Web.App.Stores.Countries;

public static class CountryActions
{
    public sealed record Randomize;

    public sealed record RandomizeResult(string Cca2);

    //public sealed record Guess(string GuessedCca2, string RandomizedCca2);

    //public sealed record GuessResult(GuessedCountry Country);
}
