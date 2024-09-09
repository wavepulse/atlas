// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Stores.Countries;
using Fluxor;

namespace Atlas.Web.App;

public sealed partial class Index(IDispatcher dispatcher, IActionSubscriber subscriber) : IDisposable
{
    private string _randomizedCca2 = string.Empty;

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    protected override void OnInitialized()
    {
        base.OnInitialized();

        dispatcher.Dispatch(new CountryActions.Randomize());
        subscriber.SubscribeToAction<CountryActions.RandomizeResult>(this, action =>
        {
            _randomizedCca2 = action.Cca2;
            StateHasChanged();
        });
    }

    private string GetRandomizedFlag() => $"https://flagcdn.com/w640/{_randomizedCca2.ToLowerInvariant()}.webp";
}
