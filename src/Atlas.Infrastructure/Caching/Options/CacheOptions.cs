// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Caching.Options;

[ExcludeFromCodeCoverage]
internal sealed partial class CacheOptions
{
    public const string Section = "cache";

    [Required]
    public required int ExpirationTimeInMinutes { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CacheOptions>;
}
