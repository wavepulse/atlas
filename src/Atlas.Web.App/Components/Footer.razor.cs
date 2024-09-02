// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Web.App.Components;

public sealed partial class Footer
{
    protected override bool ShouldRender() => false;

    private static string GetCopyrightYears()
    {
        const int startYear = 2024;

        int currentYear = DateTimeOffset.UtcNow.Year;

        if (currentYear == startYear)
            return startYear.ToString();

        return $"{startYear} - {currentYear}";
    }
}
