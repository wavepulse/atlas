// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Languages;
using Atlas.Etl.Countries.Dto;

namespace Atlas.Etl.Countries.Mappers;

internal static class TranslationMapper
{
    internal static Translation[] AsDomain(this IEnumerable<TranslationDto> dto, NameDto name, IEnumerable<string> languages)
    {
        Translation[] translations = dto.Where(t => languages.Contains(t.Code))
                                        .Select(t => new Translation(t.Code, t.Common))
                                        .ToArray();

        return [new Translation("eng", name.Common), .. translations];
    }
}
