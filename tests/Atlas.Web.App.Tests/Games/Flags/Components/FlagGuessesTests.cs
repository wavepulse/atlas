// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;

namespace Atlas.Web.App.Games.Flags.Components;

public sealed class FlagGuessesTests : TestContext
{
    private const int MaxAttempts = 6;

    private readonly GuessedCountryResponse _country = new()
    {
        Cca2 = "GB",
        Name = "United Kingdom",
        IsSameContinent = true,
        Kilometers = 1000,
        Direction = 45,
        Success = true,
        Flag = new ImageResponse(new Uri("https://flag.svg"), MediaTypeNames.Image.Svg)
    };

    public FlagGuessesTests()
    {
        Services.AddLocalization();
    }

    [Fact]
    public void ComponentShouldRenderNonGuessedFlagsBasedOnMaxAttemptsWhenThereIsNoGuesses()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IRefreshableElementCollection<IElement> nonGuessedFlags = component.FindAll("li.guess.empty");

        nonGuessedFlags.Should().HaveCount(MaxAttempts);
    }

    [Fact]
    public void ComponentShouldRenderNonGuessedFlagsWithGoodIndex()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IRefreshableElementCollection<IElement> nonGuessedFlags = component.FindAll("li.guess.empty");

        nonGuessedFlags[0].TextContent.Should().Be("1 / 6");
    }

    [Fact]
    public void ComponentShouldPopulateGuessedFlagsWhenHavingGuesses()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [_country])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IRefreshableElementCollection<IElement> guessedFlags = component.FindAll("li.guess:not(.empty)");
        guessedFlags.Should().ContainSingle();

        IRefreshableElementCollection<IElement> nonGuessedFlags = component.FindAll("li.guess.empty");
        nonGuessedFlags.Should().HaveCount(MaxAttempts - 1);
    }

    [Fact]
    public void GuessedCountryShouldHaveSuccessCssWhenIsGoodCountry()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [_country])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IElement guessedFlag = component.Find("li.guess:first-child");

        guessedFlag.ClassName.Should().Contain("success");
    }

    [Fact]
    public void GuessedCountryShouldHaveWrongCssWhenIsBadCountry()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [_country with { Success = false }])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IElement guessedFlag = component.Find("li.guess:first-child");

        guessedFlag.ClassName.Should().Contain("wrong");
    }

    [Fact]
    public void GuessedCountryShouldHaveCheckCssForArrowWhenIsGoodCountry()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [_country])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IElement arrow = component.Find("li.guess i.arrow");

        arrow.ClassName.Should().Contain($"success {Icons.Check}");
    }

    [Fact]
    public void GuessedCountryShouldHaveDefaultCssForArrowWhenIsBadCountry()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [_country with { Success = false }])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IElement arrow = component.Find("li.guess i.arrow");

        arrow.ClassName.Should().Contain(Icons.ArrowUp);
    }

    [Fact]
    public void GuessedCountryShouldHaveCheckCssForContinentWhenIsSameContinent()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [_country])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IElement sameContinent = component.Find("li.guess i.same-continent");

        sameContinent.ClassName.Should().Contain($"success {Icons.Check}");
    }

    [Fact]
    public void GuessedCountryShouldHaveTimesCssForContinentWhenIsNotSameContinent()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [_country with { IsSameContinent = false }])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IElement sameContinent = component.Find("li.guess i.same-continent");

        sameContinent.ClassName.Should().Contain($"wrong {Icons.Times}");
    }

    [Fact]
    public void NonGuessedCountryShouldHaveNoCssWhenIsNotTheGoodCountry()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [_country with { Success = false }])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IElement nonGuessedFlag = component.Find("li.guess.empty");

        nonGuessedFlag.ClassName.Should().NotContain("has-won");
    }

    [Fact]
    public void NonGuessedCountryShouldHaveHasWonCssWhenIsGoodCountry()
    {
        IRenderedComponent<FlagGuesses> component = RenderComponent<FlagGuesses>(parameters =>
            parameters.Add(f => f.Guesses, [_country])
                      .Add(f => f.MaxAttempts, MaxAttempts));

        IElement nonGuessedFlag = component.Find("li.guess.empty");

        nonGuessedFlag.ClassName.Should().Contain("has-won");
    }
}
