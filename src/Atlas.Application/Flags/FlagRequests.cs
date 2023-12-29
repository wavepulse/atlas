// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Contracts.Flags;
using Mediator;

namespace Atlas.Application.Flags;

public static class FlagRequests
{
    public sealed class GetAll : IQuery<IEnumerable<Flag>>;
}
