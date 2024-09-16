// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Contracts.Countries;
using Atlas.Web.App.Stores.Countries;
using Fluxor;

namespace Atlas.Web.App;

public sealed partial class Index(IDispatcher dispatcher, IActionSubscriber subscriber, IStateSelection<SearchCountryState, SearchCountry[]> countries) : IDisposable
{
    private bool _hasLoseGame;
    private string _randomizedName = string.Empty;
    private string _randomizedCca2 = string.Empty;

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        countries.Select(c => c.Countries);

        dispatcher.Dispatch(new CountryActions.Randomize());
        subscriber.SubscribeToAction<CountryActions.RandomizeResult>(this, action =>
        {
            _randomizedCca2 = action.Cca2;
            StateHasChanged();
        });

        subscriber.SubscribeToAction<CountryActions.LoseGame>(this, _ =>
        {
            _hasLoseGame = true;

            _randomizedName = countries.Value.First(c => string.Equals(c.Cca2, _randomizedCca2, StringComparison.OrdinalIgnoreCase)).Name;
            StateHasChanged();
        });
    }

    private string GetRandomizedFlag() => $"https://flagcdn.com/w640/{_randomizedCca2.ToLowerInvariant()}.webp";

    private void Guess(string guessedCca2)
        => dispatcher.Dispatch(new CountryActions.Guess(guessedCca2, _randomizedCca2));

    private void RestartGame()
    {
        _hasLoseGame = false;
        _randomizedName = string.Empty;

        dispatcher.Dispatch(new CountryActions.Randomize());
        dispatcher.Dispatch(new CountryActions.Reset());
    }
}
