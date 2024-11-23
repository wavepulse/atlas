// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Services;
using Atlas.Web.App.Storages;
using Atlas.Web.App.Stores.Settings;
using Fluxor;
using Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Settings.Components;

public sealed class GeneralSettingsTests : Bunit.TestContext
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();
    private readonly IActionSubscriber _subscriber = Substitute.For<IActionSubscriber>();
    private readonly IStateSelection<SettingsState, General> _settings = Substitute.For<IStateSelection<SettingsState, General>>();
    private readonly NavigationManager _navigation = Substitute.For<NavigationManager>();

    public GeneralSettingsTests()
    {
        Services.AddSingleton(_dispatcher);
        Services.AddSingleton(_settings);
        Services.AddSingleton(_subscriber);
        Services.AddSingleton((IJSInProcessRuntime)JSInterop.JSRuntime);
        Services.AddSingleton(_navigation);
        Services.AddSingleton(Substitute.For<ISender>());
        Services.AddSingleton(Substitute.For<ILocalStorage>());
        Services.AddSingleton(Substitute.For<ITimeService>());

        Services.AddLocalization();

        _settings.Value.Returns(new General() { Theme = Theme.Dark });

        JSInterop.SetupVoid("changeTheme", x => x.Arguments[0]!.ToString()!.Equals(nameof(Theme.Dark), StringComparison.OrdinalIgnoreCase)).SetVoidResult();
    }

    [Fact]
    public void OnInitializedShouldSelectGeneralSettings()
    {
        RenderComponent<GeneralSettings>();

        _settings.Received().Select(Arg.Any<Func<SettingsState, General>>());
    }

    [Fact]
    public void OnInitializedShouldSubscribeToChangeThemeResultAction()
    {
        IRenderedComponent<GeneralSettings> component = RenderComponent<GeneralSettings>();

        _subscriber.Received().SubscribeToAction(component.Instance, Arg.Any<Action<SettingsActions.ChangeThemeResult>>());
    }

    [Fact]
    public void OnInitializedShouldSubscribeToChangeLanguageResultAction()
    {
        IRenderedComponent<GeneralSettings> component = RenderComponent<GeneralSettings>();

        _subscriber.Received().SubscribeToAction(component.Instance, Arg.Any<Action<SettingsActions.ChangeLanguageResult>>());
    }

    [Fact]
    public void ChangeThemeShouldDispatchChangeThemeAction()
    {
        IRenderedComponent<GeneralSettings> component = RenderComponent<GeneralSettings>();

        IElement select = component.Find(".theme > select");

        select.Change(Theme.Dark);

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeTheme>(action => action.Theme == Theme.Dark));
    }

    [Fact]
    public void ChangeLanguageShouldDispatchChangeLanguageAction()
    {
        IRenderedComponent<GeneralSettings> component = RenderComponent<GeneralSettings>();

        IElement select = component.Find(".language > select");

        select.Change(Language.English);

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeLanguage>(action => action.Language == Language.English));
    }

    [Fact]
    public async Task ChangeThemeResultShouldInvokeChangeThemeJSInterop()
    {
        Services.AddFluxor(s => s.ScanAssemblies(typeof(SettingsActions).Assembly));

        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();
        IStore store = Services.GetRequiredService<IStore>();

        await store.InitializeAsync();

        IRenderedComponent<GeneralSettings> component = RenderComponent<GeneralSettings>();

        dispatcher.Dispatch(new SettingsActions.ChangeThemeResult());

        JSInterop.VerifyInvoke("changeTheme", _settings.Value.Theme.ToString());
    }

    [Fact]
    public async Task ChangeLanguageResultShouldRefreshThePage()
    {
        Services.AddFluxor(s => s.ScanAssemblies(typeof(SettingsActions).Assembly));

        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();
        IStore store = Services.GetRequiredService<IStore>();

        await store.InitializeAsync();

        IRenderedComponent<GeneralSettings> component = RenderComponent<GeneralSettings>();

        dispatcher.Dispatch(new SettingsActions.ChangeLanguageResult());

        _navigation.Received().Refresh();
    }
}
