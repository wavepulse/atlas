// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Atlas.Application.Countries;

public interface ICountryRepository
{
    Task<Country[]> GetAllAsync(CancellationToken cancellationToken);

    Task<Country?> GetByCodeAsync(Cca2 cca2, CancellationToken cancellationToken);
}
