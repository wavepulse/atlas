// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Stores.Games;
using Fluxor;

namespace Atlas.Web.App.Games.Flags;

public sealed partial class DailyFlag(IDispatcher dispatcher, IActionSubscriber subscriber) : IDisposable
{
    private const int MaxAttempts = 6;

    private readonly List<GuessedCountryResponse> _guesses = new(MaxAttempts);

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

        subscriber.SubscribeToAction<GameActions.GuessResult>(this, action =>
        {
            _guesses.Add(action.Country);

            if (action.Country.Success)
            {
                _answer = null;
                _isGameFinished = true;

                StateHasChanged();
                return;
            }

            if (_guesses.Count == MaxAttempts && !action.Country.Success)
            {
                _answer = _country!.Name;
                _isGameFinished = true;
            }

            StateHasChanged();
        });

        dispatcher.Dispatch(new GameActions.GetDaily());
    }

    private void Guess(string guessedCca2)
        => dispatcher.Dispatch(new GameActions.Guess(guessedCca2, _country!.Cca2));
}
