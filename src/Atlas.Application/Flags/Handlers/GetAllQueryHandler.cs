// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Flags.Abstractions;
using Atlas.Application.Flags.Mappers;
using Atlas.Domain.Flags;
using Mediator;
using FlagContract = Atlas.Contracts.Flags.Flag;

namespace Atlas.Application.Flags.Handlers;

internal sealed class GetAllQueryHandler(IFlagRepository flagRepository) : IQueryHandler<FlagRequests.GetAll, IEnumerable<FlagContract>>
{
    public async ValueTask<IEnumerable<FlagContract>> Handle(FlagRequests.GetAll query, CancellationToken cancellationToken)
    {
        IEnumerable<Flag> flags = await flagRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

        return flags.AsContract();
    }
}
