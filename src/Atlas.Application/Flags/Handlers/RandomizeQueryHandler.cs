// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Commons;
using Atlas.Application.Flags.Abstractions;
using Atlas.Application.Flags.Mappers;
using Atlas.Domain.Flags;
using Mediator;
using FlagCodeContract = Atlas.Contracts.Flags.FlagCode;

namespace Atlas.Application.Flags.Handlers;

internal sealed class RandomizeQueryHandler(IFlagRepository flagRepository, IRandomizer randomizer) : IQueryHandler<FlagRequests.Randomize, FlagCodeContract>
{
    public async ValueTask<FlagCodeContract> Handle(FlagRequests.Randomize query, CancellationToken cancellationToken)
    {
        IEnumerable<Flag> flags = await flagRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return randomizer.Randomize(flags).Code.AsContract();
    }
}
