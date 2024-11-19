// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Options;
using Atlas.Web.App.Stores.DevMode;
using Atlas.Web.App.Stores.Games;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Atlas.Web.App.Games.Flags;

public sealed partial class RandomizedFlag(IDispatcher dispatcher, IActionSubscriber subscriber, DevModeOptions devMode) : IDisposable
{
    private const int MaxAttempts = 6;

    private readonly List<GuessedCountryResponse> _guesses = new(MaxAttempts);

    private string? _answer;
    private RandomizedCountryResponse? _country;
    private bool _isGameFinished;

    [SupplyParameterFromQuery]
    public string? Cca2 { get; set; }

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    protected override void OnInitialized()
    {
        subscriber.SubscribeToAction<GameActions.RandomizeResult>(this, action =>
        {
            _country = action.Country;
            StateHasChanged();
        });

        if (devMode.Enabled)
        {
            subscriber.SubscribeToAction<DevModeActions.GetCountryResult>(this, action =>
            {
                _country = action.Country;
                StateHasChanged();
            });
        }

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

        if (devMode.Enabled && !string.IsNullOrEmpty(Cca2))
            dispatcher.Dispatch(new DevModeActions.GetCountry(Cca2));
        else
            dispatcher.Dispatch(new GameActions.Randomize());
    }

    private void Guess(string guessedCca2)
        => dispatcher.Dispatch(new GameActions.Guess(guessedCca2, _country!.Cca2));

    private void Restart()
    {
        _guesses.Clear();
        _isGameFinished = false;

        dispatcher.Dispatch(new GameActions.Randomize());
    }
}
