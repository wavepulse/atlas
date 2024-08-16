// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Etl;

internal interface IMigration
{
    Task MigrateAsync(string path, CancellationToken cancellationToken);
}
