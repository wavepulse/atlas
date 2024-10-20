// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Services;

namespace Atlas.Web.App.Components;

public sealed partial class Footer(ITimeService timeService)
{
    protected override bool ShouldRender() => false;

    private string GetCopyrightYears()
    {
        const int startYear = 2024;

        int currentYear = timeService.UtcNow.Year;

        if (currentYear == startYear)
            return startYear.ToString();

        return $"{startYear} - {currentYear}";
    }
}
