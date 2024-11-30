// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Countries;
using Atlas.Domain.Languages;
using Atlas.Domain.Resources;

namespace Atlas.Application.Countries.Responses.Mappers;

internal static class RandomizedCountryResponseMapper
{
    internal static CountryResponse ToResponse(this Country country)
    {
        string name = country.Translations.First(t => t.Language == Language.English).Name;
        (Uri map, Image flag) = country.Resources;

        return new(country.Cca2, name, new ImageResponse(flag.Uri, flag.MediaType), map);
    }
}
