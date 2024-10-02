// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Contracts.Countries;
using Atlas.Web.App.Stores.Countries;
using Atlas.Web.App.Stores.Games;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Atlas.Web.App.Components;

public sealed partial class CountryAutocomplete(IJSInProcessRuntime jsRuntime, IDispatcher dispatcher, IStateSelection<SearchCountryState, SearchCountry[]> countries) : IDisposable
{
    private readonly List<string> _selectedCountries = [];

    private string _input = string.Empty;
    private DotNetObjectReference<CountryAutocomplete>? _reference;
    private ElementReference _autocomplete;
    private SearchCountry[] _filteredCountries = [];

    [Parameter]
    public EventCallback<string> Guess { get; init; }

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

        SubscribeToAction<CountryActions.GuessResult>(action =>
        {
            if (action.Flag.Success)
                return;

            _selectedCountries.Add(action.Flag.Cca2);
        });

        SubscribeToAction<GameActions.Restart>(_ =>
        {
            _selectedCountries.Clear();

            _input = string.Empty;
            _filteredCountries = [];
        });

        countries.Select(c => c.Countries);
        dispatcher.Dispatch(new SearchCountryActions.GetAll());
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

    private void FocusSearch()
    {
        jsRuntime.InvokeVoid("scrollToAutocomplete");
        _filteredCountries = GetFilteredCountries();
    }

    private Task SelectCountryAsync(string cca2)
    {
        _input = string.Empty;
        _filteredCountries = [];

        return Guess.InvokeAsync(cca2);
    }

    private SearchCountry[] GetFilteredCountries()
    {
        string input = RemoveDiacritics(_input);
        SearchCountry[] availableCountries = [.. countries.Value.ExceptBy(_selectedCountries, c => c.Cca2)];

        return Array.FindAll(availableCountries, c => RemoveDiacritics(c.Name).Contains(input, StringComparison.OrdinalIgnoreCase));
    }

    private async Task HandleKeyboardAsync(KeyboardEventArgs e)
    {
        if (e.Key is Keyboard.Escape)
        {
            ClearSearch();
            jsRuntime.InvokeVoid("focusOut", _autocomplete);
        }
        else if (e.Key is Keyboard.Enter && TrySelectCountry(out string? cca2))
        {
            await SelectCountryAsync(cca2);
        }
    }

    private Task GuessAsync()
    {
        if (string.IsNullOrWhiteSpace(_input))
            return Task.CompletedTask;

        if (TrySelectCountry(out string? cca2))
            return SelectCountryAsync(cca2);

        return Task.CompletedTask;
    }

    private bool TrySelectCountry([NotNullWhen(true)] out string? cca2)
    {
        cca2 = null;

        if (HasOneElement())
        {
            cca2 = _filteredCountries[0].Cca2;
            return true;
        }

        if (HaveElements())
        {
            SearchCountry? country = Array.Find(_filteredCountries, c => RemoveDiacritics(_input).Equals(c.Name, StringComparison.OrdinalIgnoreCase));

            cca2 = country?.Cca2;
            return cca2 is not null;
        }

        return false;

        bool HasOneElement() => _filteredCountries.Length == 1;

        bool HaveElements() => _filteredCountries.Length > 0;
    }

    private static string RemoveDiacritics(string value)
    {
        ReadOnlySpan<char> normalized = value.Normalize(NormalizationForm.FormD);

        return string.Create(value.Length, normalized, (buffer, state) =>
        {
            int i = 0;
            foreach (char c in state)
            {
                if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    buffer[i++] = c;
            }
        });
    }

    private static class Keyboard
    {
        internal const string Escape = "Escape";
        internal const string Enter = "Enter";
    }
}
