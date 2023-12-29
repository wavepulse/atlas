// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Contracts.Translations;

namespace Atlas.Contracts.Flags;

public sealed record Flag(FlagCode Code, IEnumerable<Translation> Translations);
