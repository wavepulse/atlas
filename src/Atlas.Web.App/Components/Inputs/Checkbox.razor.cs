// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;

namespace Atlas.Web.App.Components.Inputs;

public sealed partial class Checkbox
{
    [Parameter]
    [EditorRequired]
    public required string Label { get; set; }

    [Parameter]
    public bool Value { get; set; }

    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    private Task OnValueChangeAsync(bool isChecked) => ValueChanged.InvokeAsync(isChecked);
}
