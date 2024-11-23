// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Settings;
using Atlas.Web.App.Storages;
using Fluxor;

namespace Atlas.Web.App.Stores.Settings;

public sealed class SettingsEffectTests
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();
    private readonly IState<SettingsState> _state = Substitute.For<IState<SettingsState>>();
    private readonly ILocalStorage _storage = Substitute.For<ILocalStorage>();

    private readonly SettingsState _settings = new()
    {
        General = new General()
    };

    private readonly SettingsEffect _effect;

    public SettingsEffectTests()
    {
        _effect = new SettingsEffect(_state, _storage);
    }

    [Fact]
    public async Task ChangeThemeShouldSaveSettings()
    {
        _state.Value.Returns(_settings);

        await _effect.HandleChangeThemeAsync(_dispatcher);

        _storage.Received().SetItem(LocalStorageKeys.Settings, _settings);
    }

    [Fact]
    public async Task ChangeThemeShouldDispatchChangeThemeResult()
    {
        _state.Value.Returns(_settings);

        await _effect.HandleChangeThemeAsync(_dispatcher);

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeThemeResult>(_ => true));
    }

    [Fact]
    public async Task ChangeLanguageShouldSaveSettings()
    {
        _state.Value.Returns(_settings);

        await _effect.HandleChangeLanguageAsync(_dispatcher);

        _storage.Received().SetItem(LocalStorageKeys.Settings, _settings);
    }

    [Fact]
    public async Task ChangeLanguageShouldDispatchChangeLanguageResult()
    {
        _state.Value.Returns(_settings);

        await _effect.HandleChangeLanguageAsync(_dispatcher);

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeLanguageResult>(_ => true));
    }
}
