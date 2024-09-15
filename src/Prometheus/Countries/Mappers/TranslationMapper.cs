// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Languages;
using Prometheus.Countries.Dto;

namespace Prometheus.Countries.Mappers;

internal static class TranslationMapper
{
    internal static Translation[] AsDomain(this IEnumerable<TranslationDto> dto, NameDto name, IEnumerable<string> languages)
    {
        Translation[] translations = dto.Where(t => languages.Contains(t.Code))
                                        .Select(t => new Translation(GetLanguage(t.Code), t.Common))
                                        .ToArray();

        return [new Translation(Language.English, name.Common), .. translations];

        static Language GetLanguage(string code) => code switch
        {
            "eng" => Language.English,
            "fra" => Language.French,
            _ => throw new NotSupportedException($"Language code '{code}' is not supported.")
        };
    }
}
