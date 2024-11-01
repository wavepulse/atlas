// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Stores.Games;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Atlas.Web.App.Games.Components;

public sealed partial class GameOver(IDispatcher dispatcher)
{
    [Parameter]
    public string? Country { get; init; }

    [Parameter]
    [EditorRequired]
    public required Uri MapUri { get; init; }

    [Parameter]
    public EventCallback OnRestart { get; init; }

    private Task RestartAsync()
    {
        dispatcher.Dispatch(new GameActions.Restart());
        return OnRestart.InvokeAsync();
    }
}
