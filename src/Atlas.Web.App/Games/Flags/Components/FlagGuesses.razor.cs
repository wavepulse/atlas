// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Components;
using Atlas.Web.App.Stores.Games;
using Fluxor;
using System.Globalization;

namespace Atlas.Web.App.Games.Flags.Components;

public sealed partial class FlagGuesses(IActionSubscriber subscriber, IDispatcher dispatcher) : IDisposable
{
    private const int MaxGuesses = 6;

    private readonly List<GuessedCountryResponse> _guesses = new(MaxGuesses);
    private readonly NumberFormatInfo _numberFormat = new()
    {
        NumberGroupSeparator = " "
    };

    private bool _hasWonGame;

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    protected override void OnInitialized()
    {
        subscriber.SubscribeToAction<GameActions.GuessResult>(this, action =>
        {
            _hasWonGame = action.Country.Success;
            _guesses.Add(action.Country);

            if (_guesses.Count == MaxGuesses && !_hasWonGame)
                dispatcher.Dispatch(new GameActions.GameOver());

            StateHasChanged();
        });

        subscriber.SubscribeToAction<GameActions.Restart>(this, _ =>
        {
            _guesses.Clear();
            _hasWonGame = false;

            StateHasChanged();
        });
    }

    private static string HasSuccess(bool success) => success ? "success" : "wrong";

    private static string HasFound(bool success) => success ? $"success {Icons.Check}" : Icons.ArrowUp;

    private static string IsSameContinent(bool isSameContinent) => isSameContinent ? $"success {Icons.Check}" : $"wrong {Icons.Times}";

    private string HasWonGame() => _hasWonGame ? "has-won" : string.Empty;
}
