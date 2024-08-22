// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;

namespace Atlas.Application.Countries;

public interface ISearchCountryRepository
{
    Task<SearchCountry[]> GetAllAsync(CancellationToken cancellationToken);
}
