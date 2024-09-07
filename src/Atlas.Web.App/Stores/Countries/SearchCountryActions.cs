// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Contracts.Countries;

namespace Atlas.Web.App.Stores.Countries;

internal static class SearchCountryActions
{
    public sealed record GetAll;

    public sealed record GetAllResult(SearchCountry[] Countries);
}
