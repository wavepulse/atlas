// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Contracts.Countries;
using Atlas.Web.App.Stores.Countries;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Components;

public sealed partial class CountryAutocomplete(IJSInProcessRuntime jsRuntime, IDispatcher dispatcher, IStateSelection<SearchCountryState, SearchCountry[]> countries) : IDisposable
{
    private string _input = string.Empty;
    private DotNetObjectReference<CountryAutocomplete>? _reference;
    private ElementReference _autocomplete;
    private SearchCountry[] _filteredCountries = [];

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

    private void SelectCountry(string cca2)
    {
        (_, string name) = Array.Find(countries.Value, c => c.Cca2.Equals(cca2, StringComparison.OrdinalIgnoreCase))!;

        _input = name;
        _filteredCountries = [];
    }

    private SearchCountry[] GetFilteredCountries()
        => Array.FindAll(countries.Value, c => c.Name.Contains(_input, StringComparison.OrdinalIgnoreCase));

    private void HandleKeyboard(KeyboardEventArgs e)
    {
        if (e.Key is Keyboard.Escape)
        {
            ClearSearch();
            FocusOut();
        }
        else if (e.Key is Keyboard.Enter && _filteredCountries.Length == 1)
        {
            SelectCountry(_filteredCountries[0].Cca2);
            FocusOut();
        }
    }

    private void FocusOut() => jsRuntime.InvokeVoid("focusOut", _autocomplete);

    private static class Keyboard
    {
        internal const string Escape = "Escape";
        internal const string Enter = "Enter";
    }
}
