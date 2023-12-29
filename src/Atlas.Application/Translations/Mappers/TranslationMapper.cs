// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Translations;
using NameContract = Atlas.Contracts.Translations.Name;
using TranslationContract = Atlas.Contracts.Translations.Translation;

namespace Atlas.Application.Translations.Mappers;

internal static class TranslationMapper
{
    internal static IEnumerable<TranslationContract> AsContract(this IEnumerable<Translation> translations)
    {
        return translations.Select(AsContract).ToArray();

        static TranslationContract AsContract(Translation translation)
            => new(translation.Code, translation.Name.AsContract());
    }

    private static NameContract AsContract(this Name name) => new(name.Common, name.Official);
}
