// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Fakes;
using FlagContract = Atlas.Contracts.Flags.Flag;

namespace Atlas.Application.Flags.Mappers;

public sealed class FlagMapperTests
{
    [Fact]
    public void AsContractShouldConvertDomainToContract()
    {
        IEnumerable<FlagContract> flags = new[] { FakeFlags.Canada }.AsContract();

        FlagContract flag = flags.Single();

        flag.Code.Cca2.Should().Be(FakeFlags.Canada.Code.Cca2);
        flag.Code.Cca3.Should().Be(FakeFlags.Canada.Code.Cca3);
        flag.Translations.Single().Code.Should().BeEquivalentTo(FakeFlags.Canada.Translations.Single().Code);
    }
}
