// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Web.App.Stores.Changelog;

public static class ChangelogActions
{
    public sealed record GetChangelog;

    public sealed record GetChangelogResult(string Changelog);
}
