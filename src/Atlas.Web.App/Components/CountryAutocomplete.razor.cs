// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Contracts.Countries;
using Atlas.Web.App.Stores.Countries;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Globalization;
using System.Text;

namespace Atlas.Web.App.Components;

public sealed partial class CountryAutocomplete(IJSInProcessRuntime jsRuntime, IDispatcher dispatcher, IStateSelection<SearchCountryState, SearchCountry[]> countries) : IDisposable
{
    private bool _hasWonGame;
    private bool _hasLoseGame;
    private string _input = string.Empty;
    private DotNetObjectReference<CountryAutocomplete>? _reference;
    private ElementReference _autocomplete;
    private SearchCountry[] _filteredCountries = [];

    [Parameter]
    public EventCallback<string> Guess { get; init; }

    public bool HasWonGame => _hasWonGame || _hasLoseGame;

    [JSInvokable]
    public void ClearSearch()
    {
        _filteredCountries = [];
        StateHasChanged();
    }

    public void Dispose() => _reference?.Dispose();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        countries.Select(c => c.Countries);
        dispatcher.Dispatch(new SearchCountryActions.GetAll());

        SubscribeToAction<CountryActions.GuessResult>(action => _hasWonGame = action.Flag.Success);
        SubscribeToAction<CountryActions.LoseGame>(_ =>
        {
            _hasLoseGame = true;
            _input = string.Empty;
            _filteredCountries = [];

            StateHasChanged();
        });

        SubscribeToAction<CountryActions.Reset>(_ =>
        {
            _hasWonGame = false;
            _hasLoseGame = false;
        });
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            _reference = DotNetObjectReference.Create(this);
            jsRuntime.InvokeVoid("clearSearch", _reference);
        }
    }

    private void SearchCountry(ChangeEventArgs e)
    {
        _input = e.Value?.ToString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(_input))
            _filteredCountries = [];

        _filteredCountries = GetFilteredCountries();
    }

    private void FocusSearch() => _filteredCountries = GetFilteredCountries();

    private Task SelectCountryAsync(string cca2)
    {
        _input = string.Empty;
        _filteredCountries = [];

        return Guess.InvokeAsync(cca2);
    }

    private SearchCountry[] GetFilteredCountries()
    {
        string input = RemoveDiacritics(_input);

        return Array.FindAll(countries.Value, c => RemoveDiacritics(c.Name).Contains(input, StringComparison.OrdinalIgnoreCase));

        static string RemoveDiacritics(string value)
        {
            ReadOnlySpan<char> normalized = value.Normalize(NormalizationForm.FormD);
            Span<char> builder = stackalloc char[normalized.Length];
            int i = 0;

            foreach (char c in normalized)
            {
                UnicodeCategory unicode = CharUnicodeInfo.GetUnicodeCategory(c);

                if (unicode != UnicodeCategory.NonSpacingMark)
                    builder[i++] = c;
            }

            return builder[..i].ToString();
        }
    }

    private async Task HandleKeyboardAsync(KeyboardEventArgs e)
    {
        if (e.Key is Keyboard.Escape)
        {
            ClearSearch();
            FocusOut();
        }
        else if (e.Key is Keyboard.Enter && _filteredCountries.Length == 1)
        {
            await SelectCountryAsync(_filteredCountries[0].Cca2);
        }
    }

    private void FocusOut() => jsRuntime.InvokeVoid("focusOut", _autocomplete);

    private Task GuessAsync()
    {
        if (string.IsNullOrWhiteSpace(_input))
            return Task.CompletedTask;

        if (_filteredCountries.Length == 1)
            return SelectCountryAsync(_filteredCountries[0].Cca2);

        SearchCountry[] filteredCountries = GetFilteredCountries();

        if (filteredCountries.Length == 1)
            return SelectCountryAsync(filteredCountries[0].Cca2);

        return Task.CompletedTask;
    }

    private static class Keyboard
    {
        internal const string Escape = "Escape";
        internal const string Enter = "Enter";
    }
}
