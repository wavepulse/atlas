// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Prometheus.Options;

internal sealed partial class PathOptions
{
    [Required]
    public required string Root { get; set; }

    [Required]
    public required string Output { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<PathOptions>;
}
