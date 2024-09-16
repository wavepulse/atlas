// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Atlas.Etl.Countries.Settings;

internal sealed partial class CountryFilterSettings
{
    public const string Section = "country:filters";

    [Required]
    public required IEnumerable<string> Languages { get; set; }

    public IEnumerable<string> ExcludedCountries { get; set; } = [];

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CountryFilterSettings>;
}
