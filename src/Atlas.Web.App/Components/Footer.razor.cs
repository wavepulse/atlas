// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Services;
using Atlas.Web.App.Settings.Modals;
using Microsoft.AspNetCore.Components;

namespace Atlas.Web.App.Components;

public sealed partial class Footer(ITimeService timeService)
{
    [CascadingParameter]
    public required SettingsModal Modal { get; init; }

    protected override bool ShouldRender() => false;

    private string GetCopyrightYears()
    {
        const int startYear = 2024;

        int currentYear = timeService.Today.Year;

        if (currentYear == startYear)
            return startYear.ToString();

        return $"{startYear} - {currentYear}";
    }
}
