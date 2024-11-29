// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Stores.Settings;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace Atlas.Web.App.Settings.Components;

public sealed partial class DifficultySettings(IDispatcher dispatcher)
{
    [CascadingParameter]
    public required AppSettings Settings { get; set; }

    private bool IsAllFlagEnabled => Settings.Flag.All != Difficulty.None;

    private string IsAllFlagChecked => IsAllFlagEnabled ? "disabled" : string.Empty;

    private bool IsAllFlagDifficultyEnabled(Difficulty difficulty) => Settings.Flag.All == difficulty;

    private bool IsDailyFlagDifficultyEnabled(Difficulty difficulty) => Settings.Flag.Daily == difficulty;

    private bool IsRandomizedFlagDifficultyEnabled(Difficulty difficulty) => Settings.Flag.Randomized == difficulty;

    private void OnAllFlagDifficultyChange(bool isChecked, Difficulty difficulty)
    {
        dispatcher.Dispatch(new SettingsActions.ChangeFlagDifficulty(Settings.Flag with
        {
            All = isChecked ? difficulty : Difficulty.None,
            Daily = Difficulty.None,
            Randomized = Difficulty.None
        }));
    }

    private void OnDailyFlagDifficultyChange(bool isChecked, Difficulty difficulty)
    {
        dispatcher.Dispatch(new SettingsActions.ChangeFlagDifficulty(Settings.Flag with
        {
            Daily = isChecked ? difficulty : Difficulty.None,
        }));
    }

    private void OnRandomizedFlagDifficultyChange(bool isChecked, Difficulty difficulty)
    {
        dispatcher.Dispatch(new SettingsActions.ChangeFlagDifficulty(Settings.Flag with
        {
            Randomized = isChecked ? difficulty : Difficulty.None
        }));
    }
}
