// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Prometheus.Countries.Options;

internal sealed partial class CountryFilterOptions
{
    public const string Section = "country:filters";

    [Required]
    public required IEnumerable<string> Languages { get; set; }

    public IEnumerable<string> ExcludedCountries { get; set; } = [];

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CountryFilterOptions>;
}
