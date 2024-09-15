// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Languages;
using SearchCountryResponse = Atlas.Contracts.Countries.SearchCountry;

namespace Atlas.Application.Countries.Queries;

public sealed class GetAllSearchCountriesTests
{
    private readonly ISearchCountryRepository _repository = Substitute.For<ISearchCountryRepository>();

    private readonly GetAllSearchCountries.Query _query = new();
    private readonly GetAllSearchCountries.Handler _handler;

    public GetAllSearchCountriesTests()
    {
        _repository.GetAllAsync(CancellationToken.None).Returns([]);

        _handler = new GetAllSearchCountries.Handler(_repository);
    }

    [Fact]
    public async Task HandleShouldReturnGetFromRepository()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _repository.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Fact]
    public async Task HandleShouldReturnMappedCountries()
    {
        SearchCountry country = new()
        {
            Cca2 = "CA",
            Translations = [new Translation(Language.English, "Canada")]
        };

        _repository.GetAllAsync(CancellationToken.None).Returns([country]);

        SearchCountryResponse[] response = await _handler.Handle(_query, CancellationToken.None);

        SearchCountryResponse countryResponse = response[0];

        countryResponse.Cca2.Should().Be("CA");
        countryResponse.Name.Should().Be("Canada");
    }
}
