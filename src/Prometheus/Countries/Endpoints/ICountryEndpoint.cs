// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Prometheus.Countries.Dto;

namespace Prometheus.Countries.Endpoints;

internal interface ICountryEndpoint
{
    Task<CountryDto[]> GetAllAsync(CancellationToken cancellationToken);
}
