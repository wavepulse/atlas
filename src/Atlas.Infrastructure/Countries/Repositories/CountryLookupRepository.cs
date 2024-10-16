// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Domain.Countries;
using Atlas.Infrastructure.Countries.Sources;

namespace Atlas.Infrastructure.Countries.Repositories;

internal sealed class CountryLookupRepository(IDataSource<CountryLookup> dataSource) : ICountryLookupRepository
{
    public Task<CountryLookup[]> LookupAsync(CancellationToken cancellationToken)
        => dataSource.QueryAllAsync(cancellationToken);
}
