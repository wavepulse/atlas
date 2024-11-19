// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;

namespace Atlas.Web.App.Stores.DevMode;

public static class DevModeActions
{
    public sealed record GetCountry(string Cca2);

    public sealed record GetCountryResult(CountryResponse Country);
}
