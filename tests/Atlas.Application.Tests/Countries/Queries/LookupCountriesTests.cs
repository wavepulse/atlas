// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Repositories;
using Atlas.Application.Countries.Responses;
using Atlas.Domain.Countries;
using Atlas.Domain.Languages;

namespace Atlas.Application.Countries.Queries;

public sealed class LookupCountriesTests
{
    private readonly ICountryLookupRepository _repository = Substitute.For<ICountryLookupRepository>();

    private readonly LookupCountries.Query _query = new();
    private readonly LookupCountries.Handler _handler;

    public LookupCountriesTests()
    {
        _repository.LookupAsync(CancellationToken.None).Returns([]);

        _handler = new LookupCountries.Handler(_repository);
    }

    [Fact]
    public async Task HandleShouldGetDataFromRepository()
    {
        await _handler.Handle(_query, CancellationToken.None);

        await _repository.Received(1).LookupAsync(CancellationToken.None);
    }

    [Fact]
    public async Task HandleShouldMapTheDataToResponse()
    {
        CountryLookup country = new()
        {
            Cca2 = new Cca2("CA"),
            Translations = [new Translation(Language.English, "Canada")]
        };

        _repository.LookupAsync(CancellationToken.None).Returns([country]);

        CountryLookupResponse[] response = await _handler.Handle(_query, CancellationToken.None);

        CountryLookupResponse countryResponse = response[0];

        countryResponse.Cca2.Should().Be("CA");
        countryResponse.Name.Should().Be("Canada");
    }
}
