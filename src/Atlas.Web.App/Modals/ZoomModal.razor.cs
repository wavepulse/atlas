// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Modals;

public sealed partial class ZoomModal(IJSInProcessRuntime jsRuntime)
{
    private ElementReference _dialog;

    [Parameter]
    [EditorRequired]
    public required ImageResponse Image { get; init; }

    public void Show() => jsRuntime.ShowModal(_dialog);

    private void Close() => jsRuntime.CloseModal(_dialog);
}
