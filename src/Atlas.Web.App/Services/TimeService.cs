// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Atlas.Web.App.Services;

[ExcludeFromCodeCoverage]
internal sealed class TimeService : ITimeService
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}
