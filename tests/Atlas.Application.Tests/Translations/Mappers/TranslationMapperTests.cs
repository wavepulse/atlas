// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Translations;
using TranslationContract = Atlas.Contracts.Translations.Translation;

namespace Atlas.Application.Translations.Mappers;

public sealed class TranslationMapperTests
{
    [Fact]
    public void AsContractShouldConvertDomainToContract()
    {
        Translation translation = new("fra", new Name("Common", "Official"));

        IEnumerable<TranslationContract> translations = new[] { translation }.AsContract();

        TranslationContract convertedTranslation = translations.Single();

        convertedTranslation.Code.Should().Be("fra");
        convertedTranslation.Name.Common.Should().Be(translation.Name.Common);
        convertedTranslation.Name.Official.Should().Be(translation.Name.Official);
    }
}
