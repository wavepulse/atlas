# Atlas changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.4.1 &#8212; 2024-11-29

### Fixed

- Fixed dependabot paths for nuget packages ([#143])

<!-- 0.4.1 -->
[#143]: https://github.com/wavepulse/atlas/issues/143

## 0.4.0 &#8212; 2024-11-29

### Added

- Added a settings modal ([#56])
- Can see changelog by clicking on the version or settings icon to open the modal (changelog section) ([#56])
- Can manage theme and language (french and english) in the settings modal (general section) ([#39], [#110])
- Can manage difficulty in the settings modal (difficulty section) ([#111])
- Added dev mode for testing country. Only in stage site ([#75])
- Added a zoom feature when clicking on the image ([#109])
- Added a button to return to the index page via games ([#126])

### Changed

- Improve pipelines to deploy the application ([#13], [#122])
- Improve the performance to lookup countries by initials ([#103])
- Update Atlas and Prometheus to .NET 9.0 ([#105])
- Update tests project using XUnit 3 ([#124])

<!-- 0.4.0 -->
[#13]: https://github.com/wavepulse/atlas/issues/13
[#39]: https://github.com/wavepulse/atlas/issues/39
[#56]: https://github.com/wavepulse/atlas/issues/56
[#75]: https://github.com/wavepulse/atlas/issues/75
[#103]: https://github.com/wavepulse/atlas/issues/103
[#105]: https://github.com/wavepulse/atlas/issues/105
[#109]: https://github.com/wavepulse/atlas/issues/109
[#110]: https://github.com/wavepulse/atlas/issues/110
[#111]: https://github.com/wavepulse/atlas/issues/111
[#122]: https://github.com/wavepulse/atlas/issues/122
[#124]: https://github.com/wavepulse/atlas/issues/124
[#126]: https://github.com/wavepulse/atlas/issues/126

## 0.3.0 &#8212; 2024-11-09

### Added

- Added new game mode: Daily flag ([#35])
- Added a game list on the index page ([#90])
- Added Bunit tests for testing components ([#55])
- Lookup input can accept initials of a country name ([#74])

### Changed

- Improve the links to be more intuitive ([#72])
- Improve the algorithm to remove diacritics from the country names when guessing ([#91])
- Improve the performance to get all countries for the lookup input ([#34])
- Improve the performance to get a specific country when guessing ([#34])
- Improve the unit tests ([#21])
- Improve the source code structure to implement new games easier ([#24])

### Fixed

- Fixed Random flag game where the flag wasn't displayed when navigating back to the game ([#77])
- Fixed Lookup input to keep the focus when pressing the `Escape` key ([#93])

<!-- 0.3.0 -->
[#21]: https://github.com/wavepulse/atlas/issues/21
[#24]: https://github.com/wavepulse/atlas/issues/24
[#34]: https://github.com/wavepulse/atlas/issues/34
[#35]: https://github.com/wavepulse/atlas/issues/35
[#55]: https://github.com/wavepulse/atlas/issues/55
[#72]: https://github.com/wavepulse/atlas/issues/72
[#74]: https://github.com/wavepulse/atlas/issues/74
[#77]: https://github.com/wavepulse/atlas/issues/77
[#90]: https://github.com/wavepulse/atlas/issues/90
[#91]: https://github.com/wavepulse/atlas/issues/91
[#93]: https://github.com/wavepulse/atlas/issues/93

## 0.2.1 &#8212; 2024-09-16

### Fixed

- Hide logs in production ([#44])
- Pressing `Enter` key will select the exact country name in any order from the list ([#70])
- External links will open in a new tab ([#71])

<!-- 0.2.1 -->
[#70]: https://github.com/wavepulse/atlas/issues/70
[#71]: https://github.com/wavepulse/atlas/issues/71

## 0.2.0 &#8212; 2024-09-16

### Added

- Mentions used projects in the `README.md` file ([#50])
- Scroll to the autocomplete input when having the focus for mobile devices ([#32])
- Extract flag image uri and map uri from API to use in the game ([#51])
- Distinct environment between preview and production ([#58])
- Added 404 page ([#59])

### Changed

- Filter guessed countries from the autocomplete list ([#38])
- Hide the autocomplete and display answer or congralute you when the game is over ([#37], [#46])
- Change javascript to typescript ([#25])
- Simplify language code for translations ([#20])
- Improve icons usage ([#36])

### Fixed

- Improve randomized flag image quality ([#49])
- Pressing `Enter` key on an exact country name will select the first country in the list corresponding to the name ([#48])
- Hide logs in production ([#44])

<!-- 0.2.0 -->
[#20]: https://github.com/wavepulse/atlas/issues/20
[#25]: https://github.com/wavepulse/atlas/issues/25
[#32]: https://github.com/wavepulse/atlas/issues/32
[#36]: https://github.com/wavepulse/atlas/issues/36
[#37]: https://github.com/wavepulse/atlas/issues/37
[#38]: https://github.com/wavepulse/atlas/issues/38
[#44]: https://github.com/wavepulse/atlas/issues/44
[#46]: https://github.com/wavepulse/atlas/issues/46
[#48]: https://github.com/wavepulse/atlas/issues/48
[#49]: https://github.com/wavepulse/atlas/issues/49
[#50]: https://github.com/wavepulse/atlas/issues/50
[#51]: https://github.com/wavepulse/atlas/issues/51
[#58]: https://github.com/wavepulse/atlas/issues/58
[#59]: https://github.com/wavepulse/atlas/issues/59

## 0.1.0 &#8212; 2024-09-11

### Added

- Initial release
- Add randomized flag game
