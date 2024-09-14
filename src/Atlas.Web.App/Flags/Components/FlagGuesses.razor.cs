// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Contracts.Flags;
using Atlas.Web.App.Stores.Countries;
using Atlas.Web.App.Stores.Games;
using Fluxor;
using System.Globalization;

namespace Atlas.Web.App.Flags.Components;

public sealed partial class FlagGuesses(IActionSubscriber subscriber, IDispatcher dispatcher) : IDisposable
{
    private const int MaxGuesses = 6;

    private readonly List<GuessedFlag> _guesses = new(MaxGuesses);
    private readonly NumberFormatInfo _numberFormat = new()
    {
        NumberGroupSeparator = " "
    };

    private bool _hasWonGame;

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    protected override void OnInitialized()
    {
        subscriber.SubscribeToAction<CountryActions.GuessResult>(this, action =>
        {
            _hasWonGame = action.Flag.Success;
            _guesses.Add(action.Flag);

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

    private static string GetFlag(string cca2) => $"https://flagcdn.com/{cca2.ToLowerInvariant()}.svg";

    private static string HasSuccess(bool success) => success ? "success" : "wrong";

    private static string HasFound(bool success) => success ? "success bi-check" : "bi-arrow-up";

    private static string IsSameContinent(bool isSameContinent) => isSameContinent ? "success bi-check" : "wrong bi-x";

    private string HasWonGame() => _hasWonGame ? "has-won" : string.Empty;
}
