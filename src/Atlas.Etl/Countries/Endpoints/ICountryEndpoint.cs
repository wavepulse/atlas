// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Etl.Countries.Dto;

namespace Atlas.Etl.Countries.Endpoints;

internal interface ICountryEndpoint
{
    Task<CountryDto[]> GetAllAsync(CancellationToken cancellationToken);
}
