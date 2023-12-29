// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Domain.Flags;

namespace Atlas.Application.Flags.Abstractions;

public interface IFlagRepository
{
    Task<IEnumerable<Flag>> GetAllAsync(CancellationToken cancellationToken);
}
