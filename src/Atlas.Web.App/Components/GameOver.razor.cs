// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Stores.Countries;
using Atlas.Web.App.Stores.Games;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Atlas.Web.App.Components;

public sealed partial class GameOver(IDispatcher dispatcher)
{
    [Parameter]
    [EditorRequired]
    public required string? Answer { get; init; }

    private void RestartGame()
    {
        dispatcher.Dispatch(new GameActions.Restart());
        dispatcher.Dispatch(new CountryActions.Randomize());
    }
}
