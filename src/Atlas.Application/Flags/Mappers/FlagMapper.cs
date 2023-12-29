// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Translations.Mappers;
using Atlas.Domain.Flags;
using FlagCodeContract = Atlas.Contracts.Flags.FlagCode;
using FlagContract = Atlas.Contracts.Flags.Flag;

namespace Atlas.Application.Flags.Mappers;

internal static class FlagMapper
{
    internal static IEnumerable<FlagContract> AsContract(this IEnumerable<Flag> flags)
    {
        return flags.Select(AsContract).ToArray();

        static FlagContract AsContract(Flag flag)
            => new(flag.Code.AsContract(), flag.Translations.AsContract());
    }

    internal static FlagCodeContract AsContract(this FlagCode code) => new(code.Cca2, code.Cca3);
}
