// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Atlas.Application.Countries;

public interface ICountryRepository
{
    Task<string[]> GetAllCodesAsync(CancellationToken cancellationToken);

    Task<Country?> GetByCodeAsync(string cca2, CancellationToken cancellationToken);
}
