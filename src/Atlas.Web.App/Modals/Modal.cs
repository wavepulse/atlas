// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.JSInterop;

namespace Atlas.Web.App.Modals;

internal sealed class Modal(IJSInProcessRuntime jsRuntime) : IModal
{
    public void Show() => jsRuntime.InvokeVoid("showModal");

    public void Close() => jsRuntime.InvokeVoid("closeModal");
}
