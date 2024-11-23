// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Stores.Settings;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Web.App.Settings.Components;

public sealed class GeneralSettingsTests : Bunit.TestContext
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();

    private readonly AppSettings _settings = new();

    public GeneralSettingsTests()
    {
        Services.AddSingleton(_dispatcher);
        Services.AddLocalization();
    }

    [Fact]
    public void ChangeThemeShouldDispatchChangeThemeAction()
    {
        IRenderedComponent<GeneralSettings> component = RenderComponent<GeneralSettings>(parameters => parameters.Add(c => c.Settings, _settings));

        IElement select = component.Find(".theme > select");

        select.Change(Theme.Dark);

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeTheme>(action => action.Theme == Theme.Dark));
    }

    [Fact]
    public void ChangeLanguageShouldDispatchChangeLanguageAction()
    {
        IRenderedComponent<GeneralSettings> component = RenderComponent<GeneralSettings>(parameters => parameters.Add(c => c.Settings, _settings));

        IElement select = component.Find(".language > select");

        select.Change(Language.English);

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeLanguage>(action => action.Language == Language.English));
    }
}
