// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Contracts.Countries;
using Atlas.Web.App.Stores.Countries;
using Fluxor;

namespace Atlas.Web.App;

public sealed partial class Index(IDispatcher dispatcher, IActionSubscriber subscriber, IStateSelection<SearchCountryState, SearchCountry[]> countries) : IDisposable
{
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
    }

    private string GetRandomizedFlag() => $"https://flagcdn.com/{_randomizedCca2.ToLowerInvariant()}.svg";

    private void Guess(string guessedCca2)
        => dispatcher.Dispatch(new CountryActions.Guess(guessedCca2, _randomizedCca2));
}
