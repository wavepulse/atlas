// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Web.App.Settings;

public sealed record FlagDifficulty
{
    public Difficulty All { get; init; }

    public Difficulty Randomized { get; init; }

    public Difficulty Daily { get; init; }
}
