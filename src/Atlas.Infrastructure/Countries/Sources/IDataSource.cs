// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Infrastructure.Countries.Sources;

internal interface IDataSource<T>
{
    Task<T[]> QueryAllAsync(CancellationToken cancellationToken);
}
