// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Components;

public sealed partial class CountryAutocomplete(IJSInProcessRuntime jsRuntime) : IDisposable
{
    private readonly (string Code, string Name)[] _countries =
    [
        ("CA", "Canada"),
        ("US", "United States"),
        ("MX", "Mexico"),
        ("GB", "United Kingdom"),
        ("DE", "Germany"),
        ("FR", "France"),
        ("IT", "Italy"),
        ("ES", "Spain"),
        ("PT", "Portugal"),
        ("NL", "Netherlands"),
        ("BE", "Belgium"),
        ("LU", "Luxembourg"),
        ("IE", "Ireland"),
        ("DK", "Denmark"),
        ("SE", "Sweden"),
        ("FI", "Finland"),
        ("NO", "Norway"),
        ("IS", "Iceland"),
        ("CH", "Switzerland"),
        ("AT", "Austria"),
        ("CZ", "Czech Republic"),
        ("SK", "Slovakia"),
        ("HU", "Hungary"),
        ("SI", "Slovenia"),
        ("HR", "Croatia"),
        ("BA", "Bosnia and Herzegovina"),
        ("RS", "Serbia"),
        ("ME", "Montenegro"),
        ("AL", "Albania"),
        ("MK", "North Macedonia"),
        ("BG", "Bulgaria"),
        ("RO", "Romania"),
        ("GR", "Greece"),
        ("TR", "Turkey"),
        ("RU", "Russia"),
        ("UA", "Ukraine"),
        ("BY", "Belarus"),
        ("PL", "Poland"),
        ("LT", "Lithuania"),
        ("LV", "Latvia"),
        ("EE", "Estonia"),
        ("MD", "Moldova"),
        ("AM", "Armenia"),
        ("AZ", "Azerbaijan"),
        ("GE", "Georgia"),
        ("KZ", "Kazakhstan"),
        ("UZ", "Uzbekistan"),
        ("TM", "Turkmenistan"),
        ("TJ", "Tajikistan"),
        ("KG", "Kyrgyzstan"),
        ("AF", "Afghanistan"),
        ("PK", "Pakistan"),
        ("IN", "India"),
        ("NP", "Nepal"),
        ("BD", "Bangladesh"),
        ("LK", "Sri Lanka"),
        ("MM", "Myanmar"),
        ("TH", "Thailand"),
        ("KH", "Cambodia"),
        ("LA", "Laos"),
        ("VN", "Vietnam")
    ];

    private string _input = string.Empty;
    private DotNetObjectReference<CountryAutocomplete>? _reference;
    private (string Code, string Name)[] _filteredCountries = [];

    [JSInvokable]
    public void ClearSearch()
    {
        _filteredCountries = [];
        StateHasChanged();
    }

    public void Dispose() => _reference?.Dispose();

    protected override void OnAfterRender(bool firstRender)
    {
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
        (_, string name) = Array.Find(_countries, c => c.Code.Equals(cca2, StringComparison.OrdinalIgnoreCase));
        _input = name;

        _filteredCountries = [];
    }

    private (string Code, string Name)[] GetFilteredCountries()
        => Array.FindAll(_countries, c => c.Name.Contains(_input, StringComparison.OrdinalIgnoreCase));
}
