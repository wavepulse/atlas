// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Microsoft.AspNetCore.Components;

namespace Atlas.Web.App.Games;

public sealed partial class Game
{
    [Parameter]
    public ImageResponse? Image { get; init; }

    [Parameter]
    [EditorRequired]
    public required RenderFragment ChildContent { get; init; }
}
