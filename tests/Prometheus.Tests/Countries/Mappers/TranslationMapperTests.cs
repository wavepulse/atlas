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

        english.Language.Should().Be(Language.English);
        english.Name.Should().Be("Canada");

        Translation french = translations[1];

        french.Language.Should().Be(Language.French);
        french.Name.Should().Be("Canada");
    }

    [Fact]
    public void AsDomainShouldExcludeTranslationsWhichAreNotInAcceptedLanguages()
    {
        TranslationDto[] dto = [new("deu", "Kanada")];

        Translation[] translations = dto.AsDomain(_name, _languages);

        translations.Should().ContainSingle();
    }

    [Fact]
    public void AsDomainShouldThrowNotSupportedExceptionForUnsupportedLanguage()
    {
        TranslationDto[] dto = [new("deu", "Kanada")];
        Action action = () => dto.AsDomain(_name, ["deu"]);
        action.Should().Throw<NotSupportedException>().WithMessage("Language code 'deu' is not supported.");
    }
}
