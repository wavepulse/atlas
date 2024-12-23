// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Atlas.Web.App.Stores.Games;

public static class GameActions
{
    public sealed record GetDaily;

    public sealed record GetDailyResult(CountryResponse Country, GuessedCountryResponse[] Guesses);

    public sealed record Guess(string GuessedCca2, string Cca2);

    public sealed record GuessResult(GuessedCountryResponse Country);

    public sealed record Randomize;

    public sealed record RandomizeResult(CountryResponse Country);

    public sealed record Restart;
}
