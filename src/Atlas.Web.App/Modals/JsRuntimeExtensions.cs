// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Modals;

internal static class JsRuntimeExtensions
{
    internal static void ShowModal(this IJSInProcessRuntime jsRuntime, ElementReference dialog)
        => jsRuntime.InvokeVoid("showModal", dialog);

    internal static void CloseModal(this IJSInProcessRuntime jsRuntime, ElementReference dialog)
        => jsRuntime.InvokeVoid("closeModal", dialog);
}
