// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Settings;

namespace Atlas.Web.App.Stores.Settings;

public static class SettingsActions
{
    public sealed record ChangeTheme(Theme Theme);

    public sealed record ChangeThemeResult;

    public sealed record ChangeLanguage(Language Language);

    public sealed record ChangeLanguageResult;
}
