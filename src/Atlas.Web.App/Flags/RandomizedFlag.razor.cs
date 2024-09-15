// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Stores.Countries;
using Atlas.Web.App.Stores.Games;
using Fluxor;

namespace Atlas.Web.App.Flags;

public sealed partial class RandomizedFlag(IDispatcher dispatcher, IActionSubscriber subscriber) : IDisposable
{
    private string _randomizedCca2 = string.Empty;
    private string? _answer;
    private Uri _flagSvgUri = default!;
    private Uri _mapUri = default!;

    private bool _isGameFinished;

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    protected override void OnInitialized()
    {
        dispatcher.Dispatch(new CountryActions.Randomize());

        subscriber.SubscribeToAction<CountryActions.RandomizeResult>(this, action =>
        {
            (string cca2, string name, Uri flagSvgUri, Uri mapUri) = action.Country;

            _randomizedCca2 = cca2;
            _answer = name;
            _flagSvgUri = flagSvgUri;
            _mapUri = mapUri;

            StateHasChanged();
        });

        subscriber.SubscribeToAction<GameActions.GameOver>(this, _ =>
        {
            _isGameFinished = true;
            StateHasChanged();
        });

        subscriber.SubscribeToAction<GameActions.Restart>(this, _ =>
        {
            _isGameFinished = false;
            StateHasChanged();
        });

        subscriber.SubscribeToAction<CountryActions.GuessResult>(this, action =>
        {
            if (!action.Flag.Success)
                return;

            _answer = null;
            _isGameFinished = true;

            StateHasChanged();
        });
    }

    private void Guess(string guessedCca2)
        => dispatcher.Dispatch(new CountryActions.Guess(guessedCca2, _randomizedCca2));
}
