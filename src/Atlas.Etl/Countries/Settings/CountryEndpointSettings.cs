// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Atlas.Etl.Countries.Settings;

internal sealed partial class CountryEndpointSettings
{
    public const string Section = "country";

    [Url, Required]
    public required string Endpoint { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CountryEndpointSettings>;
}
