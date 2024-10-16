// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Infrastructure.Countries.Sources;

namespace Atlas.Infrastructure.Countries.Repositories;

public sealed class CountryLookupRepositoryTests
{
    private readonly IDataSource<CountryLookup> _dataSource = Substitute.For<IDataSource<CountryLookup>>();

    private readonly CountryLookupRepository _repository;

    public CountryLookupRepositoryTests()
    {
        _repository = new CountryLookupRepository(_dataSource);
    }

    [Fact]
    public async Task LookupAsyncShouldGetAllCountryLookups()
    {
        await _repository.LookupAsync(CancellationToken.None);

        await _dataSource.Received(1).QueryAllAsync(Arg.Any<CancellationToken>());
    }
}
