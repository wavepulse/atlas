// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Services;
using Mediator;

namespace Atlas.Application.Countries.Queries;

public static class RandomizeCountry
{
    public sealed record Query : IQuery<string>;

    internal sealed class Handler(IRandomizer randomizer, ICountryRepository repository) : IQueryHandler<Query, string>
    {
        public async ValueTask<string> Handle(Query query, CancellationToken cancellationToken)
        {
            string[] codes = await repository.GetAllCodesAsync(cancellationToken).ConfigureAwait(false);

            return randomizer.Randomize<string>(codes);
        }
    }
}
