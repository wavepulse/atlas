// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Fluxor;

namespace Atlas.Web.App.Stores.Countries;

[FeatureState(Name = "Countries")]
public sealed record CountryState
{
    public CountryLookupResponse[] Countries { get; init; } = [];
}
