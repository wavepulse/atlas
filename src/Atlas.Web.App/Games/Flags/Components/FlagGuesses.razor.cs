// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Responses;
using Atlas.Web.App.Components;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Atlas.Web.App.Games.Flags.Components;

public sealed partial class FlagGuesses
{
    private readonly NumberFormatInfo _numberFormat = new()
    {
        NumberGroupSeparator = " "
    };

    [Parameter]
    [EditorRequired]
    public required IReadOnlyCollection<GuessedCountryResponse> Guesses { get; init; }

    [Parameter]
    [EditorRequired]
    public required int MaxAttempts { get; init; }

    private static string HasSuccess(bool success) => success ? "success" : "wrong";

    private static string HasFound(bool success) => success ? $"success {Icons.Check}" : Icons.ArrowUp;

    private static string IsSameContinent(bool isSameContinent) => isSameContinent ? $"success {Icons.Check}" : $"wrong {Icons.Times}";

    private string HasWonGame() => Guesses.Any(c => c.Success) ? "has-won" : string.Empty;
}
