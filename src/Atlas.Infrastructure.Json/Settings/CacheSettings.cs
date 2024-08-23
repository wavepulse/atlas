// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Atlas.Infrastructure.Json.Settings;

internal sealed partial class CacheSettings
{
    public const string Section = "cache";

    [Required]
    public required int ExpirationTimeInMinutes { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CacheSettings>;
}
