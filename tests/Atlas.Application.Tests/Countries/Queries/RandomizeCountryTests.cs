// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Services;

namespace Atlas.Application.Countries.Queries;

public sealed class RandomizeCountryTests
{
    private readonly string[] _codes = ["CA"];

    private readonly IRandomizer _randomizer = new Randomizer();
    private readonly ICountryRepository _countryRepository = Substitute.For<ICountryRepository>();

    private readonly RandomizeCountry.Query _query = new();
    private readonly RandomizeCountry.Handler _handler;

    public RandomizeCountryTests()
    {
        _countryRepository.GetAllCodesAsync(CancellationToken.None).Returns(_codes);

        _handler = new RandomizeCountry.Handler(_randomizer, _countryRepository);
    }

    [Fact]
    public async Task HandleShouldGetAllCodes()
    {
        _ = await _handler.Handle(_query, CancellationToken.None);

        await _countryRepository.Received(1).GetAllCodesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task HandleShouldReturnTheRandomizedCountry()
    {
        string code = await _handler.Handle(_query, CancellationToken.None);

        code.Should().Be(_codes[0]);
    }
}
