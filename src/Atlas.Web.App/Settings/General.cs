// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Web.App.Settings;

public sealed record General
{
    public Theme Theme { get; init; }

    public Language Language { get; init; }
}
