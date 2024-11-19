// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Atlas.Web.App.Options;

[ExcludeFromCodeCoverage]
public sealed class DevModeOptions
{
    public const string Section = "devMode";

    public bool Enabled { get; set; }
}
