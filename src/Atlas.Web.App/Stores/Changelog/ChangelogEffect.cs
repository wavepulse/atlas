// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Changelog.Queries;
using Fluxor;
using Mediator;

namespace Atlas.Web.App.Stores.Changelog;

internal sealed class ChangelogEffect(ISender sender)
{
    [EffectMethod(typeof(ChangelogActions.GetChangelog))]
    public async Task GetChangelogAsync(IDispatcher dispatcher)
    {
        string changelog = await sender.Send(new GetChangelog.Query()).ConfigureAwait(false);

        dispatcher.Dispatch(new ChangelogActions.GetChangelogResult(changelog));
    }
}
