// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Stores.Games;
using Fluxor;

namespace Atlas.Web.App.Games.Flags;

public sealed partial class DailyFlag(IDispatcher dispatcher, IActionSubscriber subscriber) : IDisposable
{
    private readonly List<string> _guessedCountries = [];

    private string? _answer;
    private RandomizedCountryResponse? _country;
    private bool _isGameFinished;

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    protected override void OnInitialized()
    {
        subscriber.SubscribeToAction<GameActions.GetDailyResult>(this, action =>
        {
            _country = action.Country;
            StateHasChanged();
        });

        subscriber.SubscribeToAction<GameActions.GameOver>(this, _ =>
        {
            _isGameFinished = true;
            _answer = _country!.Name;

            StateHasChanged();
        });

        subscriber.SubscribeToAction<GameActions.GuessResult>(this, action =>
        {
            if (!action.Country.Success)
            {
                _guessedCountries.Add(action.Country.Cca2);

                StateHasChanged();
                return;
            }

            _answer = null;
            _isGameFinished = true;

            StateHasChanged();
        });

        dispatcher.Dispatch(new GameActions.GetDaily());
    }

    private void Guess(string guessedCca2)
        => dispatcher.Dispatch(new GameActions.Guess(guessedCca2, _country!.Cca2));
}
