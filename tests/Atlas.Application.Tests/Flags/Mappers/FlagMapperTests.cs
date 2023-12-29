// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Flags;
using Atlas.Domain.Geography;
using Atlas.Domain.Translations;
using FlagContract = Atlas.Contracts.Flags.Flag;

namespace Atlas.Application.Flags.Mappers;

public sealed class FlagMapperTests
{
    private readonly Flag _flag = new()
    {
        Code = new FlagCode("ca", "can"),
        Translations = [new Translation("fra", new Name("Canada", "Canada"))],
        Continent = Continent.America,
        Area = new Area(9984670),
        Coordinate = new Coordinate(60, -95)
    };

    [Fact]
    public void AsContractShouldConvertDomainToContract()
    {
        IEnumerable<FlagContract> flags = new[] { _flag }.AsContract();

        FlagContract flag = flags.Single();

        flag.Code.Cca2.Should().Be(_flag.Code.Cca2);
        flag.Code.Cca3.Should().Be(_flag.Code.Cca3);
        flag.Translations.Single().Code.Should().BeEquivalentTo(_flag.Translations.Single().Code);
    }
}
