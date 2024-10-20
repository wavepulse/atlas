// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Stores.Countries;
using Atlas.Web.App.Stores.Games;
using Fluxor;

namespace Atlas.Web.App.Flags;

public sealed partial class RandomizedFlag(IDispatcher dispatcher, IActionSubscriber subscriber) : IDisposable
{
    private string _randomizedCca2 = string.Empty;
    private string? _answer;
    private ImageResponse _flag = default!;
    private Uri _mapUri = default!;

    private bool _isGameFinished;

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    protected override void OnInitialized()
    {
        subscriber.SubscribeToAction<CountryActions.RandomizeResult>(this, action =>
        {
            (string cca2, string name, ImageResponse flag, Uri mapUri) = action.Country;

            _randomizedCca2 = cca2;
            _answer = name;
            _flag = flag;
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
            if (!action.Country.Success)
                return;

            _answer = null;
            _isGameFinished = true;

            StateHasChanged();
        });

        dispatcher.Dispatch(new CountryActions.Randomize());
    }

    private void Guess(string guessedCca2)
        => dispatcher.Dispatch(new CountryActions.Guess(guessedCca2, _randomizedCca2));
}
