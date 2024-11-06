// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Application.Countries.Responses;

public sealed record RandomizedCountryResponse(string Cca2, string Name, ImageResponse Flag, Uri Map);