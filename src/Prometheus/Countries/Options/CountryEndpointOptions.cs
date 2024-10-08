// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Prometheus.Countries.Options;

internal sealed partial class CountryEndpointOptions
{
    public const string Section = "country";

    [Url, Required]
    public required string Endpoint { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CountryEndpointOptions>;
}
