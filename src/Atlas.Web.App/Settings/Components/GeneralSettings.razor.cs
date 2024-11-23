// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Stores.Settings;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Settings.Components;

public sealed partial class GeneralSettings(IDispatcher dispatcher, IStateSelection<SettingsState, General> settings, IJSInProcessRuntime jsRuntime, NavigationManager navigation)
{
    protected override void OnInitialized()
    {
        base.OnInitialized();

        settings.Select(state => state.General);

        SubscribeToAction<SettingsActions.ChangeThemeResult>(_ => jsRuntime.InvokeVoid("changeTheme", settings.Value.Theme.ToString()));
        SubscribeToAction<SettingsActions.ChangeLanguageResult>(_ => navigation.Refresh());
    }

    private static (Theme Theme, string Name)[] GetThemes()
    {
        return Enum.GetValues<Theme>()
                   .Select(theme => (theme, theme.ToString()))
                   .ToArray();
    }

    private static (Language Language, string Name)[] GetLanguages()
    {
        return Enum.GetValues<Language>()
                   .Select(language => (language, language.ToString()))
                   .ToArray();
    }

    private void ChangeTheme(Theme theme)
        => dispatcher.Dispatch(new SettingsActions.ChangeTheme(theme));

    private void ChangeLanguage(Language language)
        => dispatcher.Dispatch(new SettingsActions.ChangeLanguage(language));
}
