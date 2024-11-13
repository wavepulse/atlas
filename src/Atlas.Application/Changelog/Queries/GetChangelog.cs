// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Repositories;
using Mediator;

namespace Atlas.Application.Changelog.Queries;

public static class GetChangelog
{
    public sealed record Query : IQuery<string>;

    internal sealed class Handler(IChangelogRepository repository) : IQueryHandler<Query, string>
    {
        public ValueTask<string> Handle(Query query, CancellationToken cancellationToken)
            => repository.GetAsync(cancellationToken);
    }
}
