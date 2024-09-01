// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Atlas.Web.App.Settings;

public sealed partial class CompanySettings
{
    public const string Section = "project:company";

    [Required]
    public required string Name { get; set; }

    [Required, Url]
    public required string Url { get; set; }

    [OptionsValidator]
    internal sealed partial class Validator : IValidateOptions<CompanySettings>;
}
