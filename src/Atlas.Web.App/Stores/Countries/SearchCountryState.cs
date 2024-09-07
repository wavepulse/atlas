// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Contracts.Countries;
using Fluxor;

namespace Atlas.Web.App.Stores.Countries;

[FeatureState(Name = "SearchCountry")]
public sealed record SearchCountryState
{
    public SearchCountry[] Countries { get; init; } = [];
}
