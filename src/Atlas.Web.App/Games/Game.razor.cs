// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Modals;
using Microsoft.AspNetCore.Components;

namespace Atlas.Web.App.Games;

public sealed partial class Game
{
    private ZoomModal _modal = default!;

    [Parameter]
    public ImageResponse? Image { get; init; }

    [Parameter]
    [EditorRequired]
    public required RenderFragment ChildContent { get; init; }

    [Parameter]
    public string? DifficultyCss { get; init; }

    private void Zoom() => _modal.Show();
}
