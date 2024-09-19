// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using Prometheus.Countries.Dto;
using System.ComponentModel.DataAnnotations;
using SystemTextJsonPatch.Operations;

namespace Prometheus.Patch.Settings;

internal sealed partial class CountryPatchSettings
{
    public const string Section = "patch:countries";

    [Required]
    public required Dictionary<string, Operation<CountryDto>[]> Patches { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CountryPatchSettings>;
}
