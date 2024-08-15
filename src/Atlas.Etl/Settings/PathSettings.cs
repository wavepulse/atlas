// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Atlas.Etl.Settings;

internal sealed partial class PathSettings
{
    [Required]
    public required string Root { get; set; }

    [Required]
    public required string Output { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<PathSettings>;
}
