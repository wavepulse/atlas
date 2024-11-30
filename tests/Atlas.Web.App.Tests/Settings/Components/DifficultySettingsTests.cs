// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Web.App.Stores.Settings;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Web.App.Settings.Components;

public sealed class DifficultySettingsTests : Bunit.TestContext
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();
    private readonly AppSettings _settings = new();

    public DifficultySettingsTests()
    {
        Services.AddSingleton(_dispatcher);
        Services.AddLocalization();
    }

    [Theory]
    [InlineData(1, Difficulty.Blur)]
    [InlineData(2, Difficulty.Invert)]
    [InlineData(3, Difficulty.Shift)]
    public void ComponentShouldDispatchChangeFlagDifficultyActionWhenPressingOnAllFlagCheckbox(int child, Difficulty expectedDifficulty)
    {
        IRenderedComponent<DifficultySettings> component = RenderComponent<DifficultySettings>(parameters => parameters.Add(c => c.Settings, _settings));

        IElement checkbox = component.Find($".difficulty-settings:first-of-type .checkbox:nth-child({child}) input");

        checkbox.Change(new ChangeEventArgs() { Value = true });

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeFlagDifficulty>(action => action.Difficulty.All == expectedDifficulty));
    }

    [Fact]
    public void ComponentShouldDispatchChangeFlagDifficultyActionWithNoneWhenUncheckAllFlagCheckbox()
    {
        IRenderedComponent<DifficultySettings> component = RenderComponent<DifficultySettings>(parameters => parameters.Add(c => c.Settings, _settings));

        IElement checkbox = component.Find(".difficulty-settings:first-of-type .checkbox:nth-child(1) input");

        checkbox.Change(new ChangeEventArgs() { Value = false });

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeFlagDifficulty>(action => action.Difficulty.All == Difficulty.None));
    }

    [Fact]
    public void ComponentShouldAddDisabledCssOnRandomizedAndDailyWhenAllFlagCheckboxIsChecked()
    {
        IRenderedComponent<DifficultySettings> component = RenderComponent<DifficultySettings>(parameters => parameters.Add(c => c.Settings, _settings with
        {
            Flag = _settings.Flag with { All = Difficulty.Blur }
        }));

        IRefreshableElementCollection<IElement> elements = component.FindAll(".difficulty-container.disabled");

        elements.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(1, Difficulty.Blur)]
    [InlineData(2, Difficulty.Invert)]
    [InlineData(3, Difficulty.Shift)]
    public void ComponentShouldDispatchChangeFlagDifficultyActionWhenPressingOnRandomizedFlagCheckbox(int child, Difficulty expectedDifficulty)
    {
        IRenderedComponent<DifficultySettings> component = RenderComponent<DifficultySettings>(parameters => parameters.Add(c => c.Settings, _settings));

        IRefreshableElementCollection<IElement> elements = component.FindAll(".difficulty-settings");
        IElement checkbox = elements[1].QuerySelector($".checkbox:nth-child({child}) input")!;

        checkbox.Change(new ChangeEventArgs() { Value = true });

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeFlagDifficulty>(action => action.Difficulty.Randomized == expectedDifficulty));
    }

    [Fact]
    public void ComponentShouldDispatchChangeFlagDifficultyActionWithNoneWhenUncheckRandomizedFlagCheckbox()
    {
        IRenderedComponent<DifficultySettings> component = RenderComponent<DifficultySettings>(parameters => parameters.Add(c => c.Settings, _settings));

        IRefreshableElementCollection<IElement> elements = component.FindAll(".difficulty-settings");
        IElement checkbox = elements[1].QuerySelector(".checkbox:nth-child(1) input")!;

        checkbox.Change(new ChangeEventArgs() { Value = false });

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeFlagDifficulty>(action => action.Difficulty.Randomized == Difficulty.None));
    }

    [Theory]
    [InlineData(1, Difficulty.Blur)]
    [InlineData(2, Difficulty.Invert)]
    [InlineData(3, Difficulty.Shift)]
    public void ComponentShouldDispatchChangeFlagDifficultyActionWhenPressingOnDailyFlagCheckbox(int child, Difficulty expectedDifficulty)
    {
        IRenderedComponent<DifficultySettings> component = RenderComponent<DifficultySettings>(parameters => parameters.Add(c => c.Settings, _settings));

        IRefreshableElementCollection<IElement> elements = component.FindAll(".difficulty-settings");
        IElement checkbox = elements[2].QuerySelector($".checkbox:nth-child({child}) input")!;

        checkbox.Change(new ChangeEventArgs() { Value = true });

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeFlagDifficulty>(action => action.Difficulty.Daily == expectedDifficulty));
    }

    [Fact]
    public void ComponentShouldDispatchChangeFlagDifficultyActionWithNoneWhenUncheckDailyFlagCheckbox()
    {
        IRenderedComponent<DifficultySettings> component = RenderComponent<DifficultySettings>(parameters => parameters.Add(c => c.Settings, _settings));

        IRefreshableElementCollection<IElement> elements = component.FindAll(".difficulty-settings");
        IElement checkbox = elements[2].QuerySelector(".checkbox:nth-child(1) input")!;

        checkbox.Change(new ChangeEventArgs() { Value = false });

        _dispatcher.Received().Dispatch(Arg.Is<SettingsActions.ChangeFlagDifficulty>(action => action.Difficulty.Daily == Difficulty.None));
    }
}
