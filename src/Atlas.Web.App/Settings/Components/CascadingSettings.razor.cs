// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Storages;
using Atlas.Web.App.Stores.Settings;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Settings.Components;

public sealed partial class CascadingSettings(ILocalStorage storage, IJSInProcessRuntime jsRuntime, IActionSubscriber subscriber, NavigationManager navigation) : IDisposable
{
    private AppSettings _settings = new();

    [Parameter]
    [EditorRequired]
    public required RenderFragment ChildContent { get; init; }

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    protected override void OnInitialized()
    {
        subscriber.SubscribeToAction<SettingsActions.ChangeLanguage>(this, action =>
        {
            _settings = _settings with { General = _settings.General with { Language = action.Language } };
            storage.SetItem(LocalStorageKeys.Settings, _settings);

            StateHasChanged();
            navigation.Refresh();
        });

        subscriber.SubscribeToAction<SettingsActions.ChangeTheme>(this, action =>
        {
            _settings = _settings with { General = _settings.General with { Theme = action.Theme } };
            storage.SetItem(LocalStorageKeys.Settings, _settings);

            jsRuntime.InvokeVoid("changeTheme", action.Theme.ToString());
            StateHasChanged();
        });

        subscriber.SubscribeToAction<SettingsActions.ChangeFlagDifficulty>(this, action =>
        {
            _settings = _settings with { Flag = action.Difficulty };

            storage.SetItem(LocalStorageKeys.Settings, _settings);
            StateHasChanged();
        });

        _settings = storage.GetItem<AppSettings>(LocalStorageKeys.Settings) ?? _settings;
        jsRuntime.InvokeVoid("changeTheme", _settings.General.Theme.ToString());
    }
}
