// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Web.App;

public sealed class IndexTests : BunitContext
{
    [Fact]
    public void IndexShouldDisplayHelloAtlas()
    {
        RenderedComponent<Index> cut = Render<Index>();

        cut.MarkupMatches("<h1>Hello, Atlas!</h1>");
    }
}
