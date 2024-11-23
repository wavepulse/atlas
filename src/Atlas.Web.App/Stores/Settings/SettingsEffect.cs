// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Storages;
using Fluxor;

namespace Atlas.Web.App.Stores.Settings;

internal sealed class SettingsEffect(IState<SettingsState> state, ILocalStorage storage)
{
    [EffectMethod(typeof(SettingsActions.ChangeTheme))]
    public Task HandleChangeThemeAsync(IDispatcher dispatcher)
    {
        storage.SetItem(LocalStorageKeys.Settings, state.Value);

        dispatcher.Dispatch(new SettingsActions.ChangeThemeResult());
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(SettingsActions.ChangeLanguage))]
    public Task HandleChangeLanguageAsync(IDispatcher dispatcher)
    {
        storage.SetItem(LocalStorageKeys.Settings, state.Value);

        dispatcher.Dispatch(new SettingsActions.ChangeLanguageResult());
        return Task.CompletedTask;
    }
}
