// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Web.App.Settings;

public sealed record AppSettings
{
    public General General { get; init; } = new General();

    public FlagDifficulty Flag { get; init; } = new FlagDifficulty();
}
