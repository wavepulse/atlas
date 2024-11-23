// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Fluxor;

namespace Atlas.Web.App.Stores.Settings;

internal static class SettingsReducers
{
    [ReducerMethod]
    public static SettingsState ReduceChangeTheme(SettingsState state, SettingsActions.ChangeTheme action)
        => state with { General = state.General with { Theme = action.Theme } };

    [ReducerMethod]
    public static SettingsState ReduceChangeLanguage(SettingsState state, SettingsActions.ChangeLanguage action)
        => state with { General = state.General with { Language = action.Language } };
}
