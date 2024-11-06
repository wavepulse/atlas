// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Atlas.Application.Countries.Repositories;

public interface ICountryRepository
{
    void Cache(Country country);

    ValueTask<Country[]> GetAllAsync(CancellationToken cancellationToken);

    ValueTask<Country?> GetAsync(Cca2 cca2, CancellationToken cancellationToken);
}
