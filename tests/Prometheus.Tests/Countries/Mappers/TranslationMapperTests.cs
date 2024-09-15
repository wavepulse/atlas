// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Languages;
using Prometheus.Countries.Dto;

namespace Prometheus.Countries.Mappers;

public sealed class TranslationMapperTests
{
    private readonly IEnumerable<string> _languages = ["fra"];
    private readonly NameDto _name = new("Canada");

    [Fact]
    public void AsDomainShouldMapDtoToDomain()
    {
        TranslationDto[] dto = [new("fra", "Canada")];

        Translation[] translations = dto.AsDomain(_name, _languages);

        Translation english = translations[0];

        english.Code.Should().Be("eng");
        english.Name.Should().Be("Canada");

        Translation french = translations[1];

        french.Code.Should().Be("fra");
        french.Name.Should().Be("Canada");
    }

    [Fact]
    public void AsDomainShouldExcludeTranslationsWhichAreNotInAcceptedLanguages()
    {
        TranslationDto[] dto = [new("deu", "Kanada")];

        Translation[] translations = dto.AsDomain(_name, _languages);

        translations.Should().ContainSingle();
    }
}
