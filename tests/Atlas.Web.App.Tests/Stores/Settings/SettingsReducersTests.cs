// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Settings;

namespace Atlas.Web.App.Stores.Settings;

public sealed class SettingsReducersTests
{
    [Fact]
    public void ChangeThemeShouldChangeTheme()
    {
        const Theme theme = Theme.Dark;

        SettingsState newState = SettingsReducers.ReduceChangeTheme(new SettingsState(), new SettingsActions.ChangeTheme(theme));

        newState.General.Theme.Should().Be(theme);
    }

    [Fact]
    public void ChangeLanguageShouldChangeLanguage()
    {
        const Language language = Language.English;

        SettingsState newState = SettingsReducers.ReduceChangeLanguage(new SettingsState(), new SettingsActions.ChangeLanguage(language));

        newState.General.Language.Should().Be(language);
    }
}
