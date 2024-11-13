// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Markdig;
using Microsoft.AspNetCore.Components;

namespace Atlas.Web.App.Modals;

public sealed partial class SettingsModal
{
    private TabItem _selectedTab = TabItem.Changelog;

    private static MarkupString Changelog()
    {
        string test = Markdown.ToHtml("## 0.3.0 &#8212; 2024-11-09\r\n\r\n### Added\r\n\r\n- Added new game mode: Daily flag ([#35])\r\n- Added a game list on the index page ([#90])\r\n- Added Bunit tests for testing components ([#55])\r\n- Lookup input can accept initials of a country name ([#74])\r\n\r\n### Changed\r\n\r\n- Improve the links to be more intuitive ([#72])\r\n- Improve the algorithm to remove diacritics from the country names when guessing ([#91])\r\n- Improve the performance to get all countries for the lookup input ([#34])\r\n- Improve the performance to get a specific country when guessing ([#34])\r\n- Improve the unit tests ([#21])\r\n- Improve the source code structure to implement new games easier ([#24])\r\n\r\n### Fixed\r\n\r\n- Fixed Random flag game where the flag wasn't displayed when navigating back to the game ([#77])\r\n- Fixed Lookup input to keep the focus when pressing the `Escape` key ([#93])\r\n\r\n<!-- 0.3.0 -->\r\n[#21]: https://github.com/wavepulse/atlas/issues/21\r\n[#24]: https://github.com/wavepulse/atlas/issues/24\r\n[#34]: https://github.com/wavepulse/atlas/issues/34\r\n[#35]: https://github.com/wavepulse/atlas/issues/35\r\n[#55]: https://github.com/wavepulse/atlas/issues/55\r\n[#72]: https://github.com/wavepulse/atlas/issues/72\r\n[#74]: https://github.com/wavepulse/atlas/issues/74\r\n[#77]: https://github.com/wavepulse/atlas/issues/77\r\n[#90]: https://github.com/wavepulse/atlas/issues/90\r\n[#91]: https://github.com/wavepulse/atlas/issues/91\r\n[#93]: https://github.com/wavepulse/atlas/issues/93\r\n");

        return new MarkupString(test);
    }

    private void SelectTab(TabItem tabItem) => _selectedTab = tabItem;

    private string IsActive(TabItem tabItem) => _selectedTab == tabItem ? "active" : string.Empty;

    private string GetTabCss() => _selectedTab switch
    {
        TabItem.Changelog => "changelog",
        _ => string.Empty
    };

    private static (string Name, TabItem TabItem)[] GetTabs()
    {
        return Enum.GetValues<TabItem>()
            .Select(tabItem => (Enum.GetName(tabItem)!, tabItem))
            .ToArray();
    }

    private enum TabItem
    {
        Changelog = 0
    }
}
