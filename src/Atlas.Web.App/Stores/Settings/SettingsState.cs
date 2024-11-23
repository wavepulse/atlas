// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Settings;
using Fluxor;

namespace Atlas.Web.App.Stores.Settings;

[FeatureState(Name = "Settings")]
public sealed record SettingsState
{
    public General General { get; init; } = new General();
}
