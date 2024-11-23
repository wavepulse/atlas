// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Modals;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Settings.Modals;

public sealed partial class SettingsModal(IJSInProcessRuntime jsRuntime, IActionSubscriber subscriber) : IDisposable
{
    private TabItem _selectedTab = TabItem.General;
    private ElementReference _dialog;

    [Parameter]
    public RenderFragment? ChildContent { get; init; }

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    public void ShowGeneral() => Show(TabItem.General);

    public void ShowChangelog() => Show(TabItem.Changelog);

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
            jsRuntime.InvokeVoid("addCloseOutsideEvent", _dialog);
    }

    private static (TabItem Item, string Name)[] GetTabItems()
    {
        return Enum.GetValues<TabItem>()
                   .Select(t => (t, t.ToString()))
                   .ToArray();
    }

    private void Show(TabItem tab)
    {
        SelectTab(tab);

        jsRuntime.ShowModal(_dialog);
        jsRuntime.InvokeVoid("scrollContentToTop", _dialog);
    }

    private void SelectTab(TabItem tab) => _selectedTab = tab;

    private string IsActive(TabItem tab) => _selectedTab == tab ? "active" : string.Empty;

    private string GetTabCss() => _selectedTab == TabItem.General ? "general" : "changelog";

    private void Close() => jsRuntime.CloseModal(_dialog);

    private enum TabItem
    {
        General = 0,
        Changelog = 1
    }
}
