// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Stores.Countries;
using Atlas.Web.App.Stores.Games;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Atlas.Web.App.Games.Components;

public sealed partial class CountryLookupInput(IDispatcher dispatcher, IStateSelection<CountryState, CountryLookupResponse[]> countries, IJSInProcessRuntime jsRuntime)
{
    private string _input = string.Empty;
    private CountryLookupResponse[] _filteredCountries = [];
    private DotNetObjectReference<CountryLookupInput>? _reference;

    [Parameter]
    [EditorRequired]
    public EventCallback<string> Guess { get; init; }

    [Parameter]
    public IEnumerable<string> SelectedCountries { get; init; } = [];

    [JSInvokable]
    public void Clear()
    {
        _filteredCountries = [];
        StateHasChanged();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        SubscribeToAction<GameActions.Restart>(_ =>
        {
            _input = string.Empty;
            _filteredCountries = [];
        });

        countries.Select(c => c.Countries);

        if (countries.Value.Length == 0)
            dispatcher.Dispatch(new CountryActions.Lookup());
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _reference = DotNetObjectReference.Create(this);
            jsRuntime.InvokeVoid("addClearEvent", _reference);
        }
    }

    protected override ValueTask DisposeAsyncCore(bool disposing)
    {
        if (_reference is not null)
        {
            jsRuntime.InvokeVoid("removeClearEvent");

            _reference.Dispose();
            _reference = null;
        }

        return base.DisposeAsyncCore(disposing);
    }

    private void Lookup(ChangeEventArgs e)
    {
        _input = e.Value?.ToString() ?? string.Empty;

        _filteredCountries = LookupCountries();
    }

    private void Focus()
    {
        jsRuntime.InvokeVoid("scrollToLookup");
        _filteredCountries = LookupCountries();
    }

    private Task HandleKeyboardAsync(KeyboardEventArgs e)
    {
        if (e.Key is Keyboard.Escape)
            _filteredCountries = [];

        if (e.Key is Keyboard.Enter && TrySelectCountry(out string? cca2))
            return SelectCountryAsync(cca2);

        return Task.CompletedTask;
    }

    private Task GuessAsync()
    {
        if (string.IsNullOrWhiteSpace(_input))
            return Task.CompletedTask;

        if (TrySelectCountry(out string? cca2))
            return SelectCountryAsync(cca2);

        return Task.CompletedTask;
    }

    private Task SelectCountryAsync(string cca2)
    {
        _input = string.Empty;
        _filteredCountries = [];

        return Guess.InvokeAsync(cca2);
    }

    private bool TrySelectCountry([NotNullWhen(true)] out string? cca2)
    {
        cca2 = null;

        if (HasOneCountry())
        {
            cca2 = _filteredCountries[0].Cca2;
            return true;
        }

        if (HasMultipleCountries())
        {
            string input = RemoveDiacritics(_input.Trim());

            CountryLookupResponse? country = Array.Find(_filteredCountries, c => Lookup(c.Name, input))!;

            cca2 = country?.Cca2;
            return cca2 is not null;
        }

        return false;

        bool HasOneCountry() => _filteredCountries.Length == 1;

        bool HasMultipleCountries() => _filteredCountries.Length > 1;

        static bool Lookup(string name, string input)
        {
            string normalized = RemoveDiacritics(name);

            if (normalized.Equals(input, StringComparison.OrdinalIgnoreCase))
                return true;

            ReadOnlySpan<char> initials = GetInitials(normalized);
            return initials.Equals(input, StringComparison.OrdinalIgnoreCase);
        }
    }

    private CountryLookupResponse[] LookupCountries()
    {
        string input = RemoveDiacritics(_input.Trim());
        CountryLookupResponse[] availableCountries = [.. countries.Value.ExceptBy(SelectedCountries, c => c.Cca2)];

        return Array.FindAll(availableCountries, c => Lookup(c.Name, input));

        static bool Lookup(string name, string input)
        {
            string normalized = RemoveDiacritics(name);

            if (normalized.Contains(input, StringComparison.OrdinalIgnoreCase))
                return true;

            ReadOnlySpan<char> initials = GetInitials(normalized);
            return initials.Contains(input, StringComparison.OrdinalIgnoreCase);
        }
    }

    private static ReadOnlySpan<char> GetInitials(ReadOnlySpan<char> value)
    {
        Span<char> initials = stackalloc char[7];

        int i = 0;
        foreach (Range range in value.Split(' '))
        {
            ReadOnlySpan<char> word = value[range];
            initials[i++] = word[0];
        }

        return initials[..i].ToString();
    }

    private static string RemoveDiacritics(string value)
    {
        ReadOnlySpan<char> normalized = value.Normalize(NormalizationForm.FormD);

        return string.Create(value.Length, normalized, (buffer, content) =>
        {
            int i = 0;
            foreach (char c in content)
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
