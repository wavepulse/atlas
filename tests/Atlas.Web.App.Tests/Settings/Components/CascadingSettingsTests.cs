// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Services;
using Atlas.Web.App.Storages;
using Atlas.Web.App.Stores.Settings;
using Fluxor;
using Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Settings.Components;

public sealed class CascadingSettingsTests : Bunit.TestContext
{
    private readonly ILocalStorage _localStorage = Substitute.For<ILocalStorage>();
    private readonly IActionSubscriber _subscriber = Substitute.For<IActionSubscriber>();
    private readonly NavigationManager _navigation = Substitute.For<NavigationManager>();

    public CascadingSettingsTests()
    {
        Services.AddSingleton(_localStorage);
        Services.AddSingleton((IJSInProcessRuntime)JSInterop.JSRuntime);
        Services.AddSingleton(_subscriber);
        Services.AddSingleton(_navigation);
        Services.AddSingleton(Substitute.For<ISender>());
        Services.AddSingleton(Substitute.For<ITimeService>());

        JSInterop.SetupVoid("changeTheme", _ => true).SetVoidResult();
    }

    [Fact]
    public void CascadingSettingsShouldSubscribeToChangeLanguage()
    {
        IRenderedComponent<CascadingSettings> settings = RenderComponent<CascadingSettings>(parameters => parameters.AddChildContent("<p>test</p>"));

        _subscriber.Received(1).SubscribeToAction(settings.Instance, Arg.Any<Action<SettingsActions.ChangeLanguage>>());
    }

    [Fact]
    public void CascadingSettingsShouldSubscribeToChangeTheme()
    {
        IRenderedComponent<CascadingSettings> settings = RenderComponent<CascadingSettings>(parameters => parameters.AddChildContent("<p>test</p>"));

        _subscriber.Received(1).SubscribeToAction(settings.Instance, Arg.Any<Action<SettingsActions.ChangeTheme>>());
    }

    [Fact]
    public void CascadingSettingsShouldUnsubscribeOnDispose()
    {
        IRenderedComponent<CascadingSettings> settings = RenderComponent<CascadingSettings>(parameters => parameters.AddChildContent("<p>test</p>"));

        settings.Instance.Dispose();

        _subscriber.Received(1).UnsubscribeFromAllActions(settings.Instance);
    }

    [Fact]
    public void OnInitializedShouldSetThemeUsingTheSettingsFromStorage()
    {
        IRenderedComponent<CascadingSettings> settings = RenderComponent<CascadingSettings>(parameters => parameters.AddChildContent("<p>test</p>"));

        _localStorage.GetItem<AppSettings>(LocalStorageKeys.Settings).Returns(new AppSettings() { General = new General() { Theme = Theme.Dark } });

        JSInterop.VerifyInvoke("changeTheme", nameof(Theme.Dark));
    }

    [Fact]
    public void OnInitializedShouldUseDefaultSettingsWhenIsNotInStorage()
    {
        AppSettings? appSettings = null;

        IRenderedComponent<CascadingSettings> settings = RenderComponent<CascadingSettings>(parameters => parameters.AddChildContent("<p>test</p>"));

        _localStorage.GetItem<AppSettings>(LocalStorageKeys.Settings).Returns(appSettings);

        JSInterop.VerifyInvoke("changeTheme", nameof(Theme.System));
    }

    [Fact]
    public async Task ChangeLanguageShouldSavingToStorageAndRefresh()
    {
        Services.AddFluxor(x => x.ScanAssemblies(typeof(SettingsActions).Assembly));

        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();
        IStore store = Services.GetRequiredService<IStore>();

        await store.InitializeAsync();

        IRenderedComponent<CascadingSettings> settings = RenderComponent<CascadingSettings>(parameters => parameters.AddChildContent("<p>test</p>"));

        await settings.InvokeAsync(() => dispatcher.Dispatch(new SettingsActions.ChangeLanguage(Language.English)));

        _localStorage.Received(1).SetItem(LocalStorageKeys.Settings, Arg.Any<AppSettings>());
        _navigation.Received().Refresh();
    }

    [Fact]
    public async Task ChangeThemeShouldSavingToStorageAndInvokeJs()
    {
        Services.AddFluxor(x => x.ScanAssemblies(typeof(SettingsActions).Assembly));

        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();
        IStore store = Services.GetRequiredService<IStore>();

        await store.InitializeAsync();

        IRenderedComponent<CascadingSettings> settings = RenderComponent<CascadingSettings>(parameters => parameters.AddChildContent("<p>test</p>"));

        await settings.InvokeAsync(() => dispatcher.Dispatch(new SettingsActions.ChangeTheme(Theme.Dark)));

        _localStorage.Received(1).SetItem(LocalStorageKeys.Settings, Arg.Any<AppSettings>());
        JSInterop.VerifyInvoke("changeTheme", 2, nameof(Theme.Dark));
    }
}
