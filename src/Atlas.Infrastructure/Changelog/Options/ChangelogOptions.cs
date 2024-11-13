// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Atlas.Infrastructure.Changelog.Options;

[ExcludeFromCodeCoverage]
public sealed partial class ChangelogOptions
{
    public const string Section = "project:changelog";

    [Required, Url]
    public required string Url { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<ChangelogOptions>;
}
