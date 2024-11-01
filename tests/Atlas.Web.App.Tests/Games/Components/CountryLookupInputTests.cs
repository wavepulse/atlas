// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Stores.Countries;
using Atlas.Web.App.Stores.Games;
using Fluxor;
using Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Games.Components;

public sealed class CountryLookupInputTests : TestContext
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();
    private readonly IActionSubscriber _subscriber = Substitute.For<IActionSubscriber>();
    private readonly IStateSelection<CountryState, CountryLookupResponse[]> _state = Substitute.For<IStateSelection<CountryState, CountryLookupResponse[]>>();

    public CountryLookupInputTests()
    {
        _state.Value.Returns(
        [
            new CountryLookupResponse("CA", "Canada"),
            new CountryLookupResponse("IT", "Italy"),
            new CountryLookupResponse("AX", "Åland Islands"),
            new CountryLookupResponse("AS", "American Samoa"),
            new CountryLookupResponse("WS", "Samoa"),
            new CountryLookupResponse("VI", "United States Virgin Islands"),
            new CountryLookupResponse("US", "United States"),
        ]);

        Services.AddSingleton(_dispatcher);
        Services.AddSingleton(_subscriber);
        Services.AddSingleton(_state);
        Services.AddSingleton(_ => (IJSInProcessRuntime)JSInterop.JSRuntime);

        JSInterop.SetupVoid("scrollToLookup").SetVoidResult();
        JSInterop.SetupVoid("addClearEvent", m => m.Arguments[0] is DotNetObjectReference<CountryLookupInput>).SetVoidResult();
        JSInterop.SetupVoid("removeClearEvent").SetVoidResult();
    }

    [Fact]
    public void OnInitializedShouldDispatchToGetCountries()
    {
        _state.Value.Returns([]);

        RenderComponent<CountryLookupInput>();

        _dispatcher.Received().Dispatch(Arg.Any<CountryActions.Lookup>());
    }

    [Fact]
    public void OnInitializedShouldNotDispatchToGetCountriesWhenAlreadyLoaded()
    {
        RenderComponent<CountryLookupInput>();

        _dispatcher.DidNotReceive().Dispatch(Arg.Any<CountryActions.Lookup>());
    }

    [Fact]
    public void OnInitializedShouldSelectCountries()
    {
        RenderComponent<CountryLookupInput>();

        _state.Received().Select(Arg.Any<Func<CountryState, CountryLookupResponse[]>>());
    }

    [Theory]
    [InlineData("Canada")]
    [InlineData("CaNaDa")]
    [InlineData("canada")]
    [InlineData(" Canada ")]
    public void InputShouldDisplayFilteredCountriesBasedOnValueWhenTriggeringOnInput(string value)
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = value });

        IRefreshableElementCollection<IElement> items = lookupInput.FindAll("ul.items > li.item");

        items.Should().ContainSingle();
        items[0].TextContent.Should().Be("Canada");
    }

    [Theory]
    [InlineData("Åland")]
    [InlineData("Aland")]
    [InlineData(" Åland ")]
    [InlineData(" Aland ")]
    public void InputShouldDisplayCountriesWithDiacritics(string value)
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = value });

        IRefreshableElementCollection<IElement> items = lookupInput.FindAll("ul.items > li.item");

        items.Should().ContainSingle();
        items[0].TextContent.Should().Be("Åland Islands");
    }

    [Fact]
    public void InputShouldDisplayAllCountriesWhenOnInputGiveEmptyValue()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = null });

        lookupInput.FindAll("ul.items > li.item").Count.Should().Be(7);
    }

    [Fact]
    public void InputShouldDisplayAllCountriesWhenHavingFocus()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");

        input.Focus();

        lookupInput.FindAll("ul.items > li.item").Count.Should().Be(7);
    }

    [Fact]
    public void InputShouldScrollToLookupUsingJavascriptWhenHavingFocus()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");

        input.Focus();

        JSInterop.VerifyInvoke("scrollToLookup");
    }

    [Fact]
    public void InputShouldRemoveTheListWhenPressingEscapeKey()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");

        input.Focus();

        input.KeyDown(new KeyboardEventArgs { Key = "Escape" });

        lookupInput.FindAll("ul.items > li.item").Count.Should().Be(0);
    }

    [Fact]
    public void InputShouldSelectCountryWhenPressingEnterKeyAndContainsSingleCountry()
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "Canada" });
        input.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        guessedCountry.Should().Be("CA");
    }

    [Theory]
    [InlineData("Samoa")]
    [InlineData("SaMoA")]
    [InlineData("samoa")]
    [InlineData("Samoâ")]
    [InlineData(" Samoa ")]
    public void InputShouldSelectCountryWithSameNameWhenPressingEnterKeyAndContainsMultipleCountries(string country)
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = country });
        input.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        guessedCountry.Should().Be("WS");
    }

    [Fact]
    public void InputShouldNotSelectCountryWhenPressingEnterKeyAndContainsNoCountry()
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "Unknown" });
        input.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        guessedCountry.Should().BeEmpty();
    }

    [Fact]
    public void InputShouldNotSelectCountryWhenPressingEnterKeyAndContainsMultipleCountries()
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "an" });
        input.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        guessedCountry.Should().BeEmpty();
    }

    [Fact]
    public void InputShouldNotSelectCountryWhenNotPressingEnterKeyAndContainsCountries()
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "Canada" });
        input.KeyDown(new KeyboardEventArgs { Key = "Backspace" });

        guessedCountry.Should().BeEmpty();
    }

    [Fact]
    public void InputShouldClearInputAndListWhenPressingEnterKey()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "Canada" });
        input.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        input.GetAttribute("value").Should().BeEmpty();
        lookupInput.FindAll("ul.items > li.item").Count.Should().Be(0);
    }

    [Fact]
    public void InputShouldDisplayCountriesBasedOnInitialsWhenTriggeringOnInput()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "US" });

        IRefreshableElementCollection<IElement> items = lookupInput.FindAll("ul.items > li.item");

        items.Should().HaveCount(2);
        items[0].TextContent.Should().Be("United States");
        items[1].TextContent.Should().Be("United States Virgin Islands");
    }

    [Fact]
    public void InputShouldSelectCountryByInitialsWhenPressingEnter()
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "US" });
        input.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        guessedCountry.Should().Be("US");
    }

    [Fact]
    public void InputShouldSelectCountryByInitialsOnlyThereIsMoreThanOneCountry()
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "I" });
        input.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        guessedCountry.Should().BeEmpty();
    }

    [Fact]
    public void InputShouldSelectCountryWhenClickingOnTheItem()
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "Canada" });

        IElement item = lookupInput.Find("ul.items > li.item");
        item.Click();

        guessedCountry.Should().Be("CA");
    }

    [Fact]
    public void InputShouldClearInputAndListWhenClickingOnTheItem()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "Canada" });

        IElement item = lookupInput.Find("ul.items > li.item");
        item.Click();

        input.GetAttribute("value").Should().BeEmpty();
        lookupInput.FindAll("ul.items > li.item").Count.Should().Be(0);
    }

    [Fact]
    public void InputShouldExcludeSelectedCountriesFromTheList()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(l => l.SelectedCountries, ["CA"]));

        IElement input = lookupInput.Find("input");
        input.Input(new ChangeEventArgs { Value = "Canada" });

        lookupInput.FindAll("ul.items > li.item").Count.Should().Be(0);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GuessButtonShouldNotSelectCountryWhenInputIsEmpty(string value)
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");
        input.Input(new ChangeEventArgs { Value = value });

        IElement button = lookupInput.Find("button");

        button.Click();

        guessedCountry.Should().BeEmpty();
    }

    [Fact]
    public void GuessButtonShouldSelectCountryWhenInputIsNotEmpty()
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "Canada" });

        IElement button = lookupInput.Find("button");
        button.Click();

        guessedCountry.Should().Be("CA");
    }

    [Fact]
    public void GuessButtonShouldClearInputAndListWhenSelectingCountry()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");
        input.Input(new ChangeEventArgs { Value = "Canada" });

        IElement button = lookupInput.Find("button");
        button.Click();

        input.GetAttribute("value").Should().BeEmpty();
        lookupInput.FindAll("ul.items > li.item").Count.Should().Be(0);
    }

    [Fact]
    public void GuessButtonShouldNotSelectCountryWhenThereIsMultipleCountries()
    {
        string guessedCountry = string.Empty;

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>(parameters =>
            parameters.Add(c => c.Guess, value => guessedCountry = value));

        IElement input = lookupInput.Find("input");

        input.Input(new ChangeEventArgs { Value = "an" });
        IElement button = lookupInput.Find("button");

        button.Click();
        guessedCountry.Should().BeEmpty();
    }

    [Fact]
    public void InputShouldSubscribeToGameActionsRestart()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        _subscriber.Received(1).SubscribeToAction(lookupInput.Instance, Arg.Any<Action<GameActions.Restart>>());
    }

    [Fact]
    public async Task InputShouldClearInputAndListWhenGameActionsRestart()
    {
        Services.AddFluxor(options => options.ScanAssemblies(typeof(GameActions).Assembly));
        Services.AddSingleton(Substitute.For<ISender>());

        IStore store = Services.GetRequiredService<IStore>();
        IDispatcher dispatcher = Services.GetRequiredService<IDispatcher>();

        await store.InitializeAsync();

        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");
        await input.InputAsync(new ChangeEventArgs { Value = "an" });

        dispatcher.Dispatch(new GameActions.Restart());

        lookupInput.Find("input").GetAttribute("value").Should().BeEmpty();
        lookupInput.FindAll("ul.items > li.item").Count.Should().Be(0);
    }

    [Fact]
    public void InputShouldAddClearEventToClearSearchWhenFirstRendered()
    {
        _ = RenderComponent<CountryLookupInput>();

        JSInterop.VerifyInvoke("addClearEvent");
    }

    [Fact]
    public async Task ClearShouldOnlyClearListOfCountries()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        IElement input = lookupInput.Find("input");
        await input.InputAsync(new ChangeEventArgs { Value = "Canada" });

        await lookupInput.InvokeAsync(lookupInput.Instance.Clear);

        lookupInput.Find("input").GetAttribute("value").Should().Be("Canada");
        lookupInput.FindAll("ul.items > li.item").Count.Should().Be(0);
    }

    [Fact]
    public async Task DisposeShouldRemoveClearEvent()
    {
        IRenderedComponent<CountryLookupInput> lookupInput = RenderComponent<CountryLookupInput>();

        await lookupInput.Instance.DisposeAsync();

        JSInterop.VerifyInvoke("removeClearEvent");
    }
}
