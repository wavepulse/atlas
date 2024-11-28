// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;

namespace Atlas.Web.App.Components.Inputs;

public sealed class CheckboxTests : Bunit.TestContext
{
    [Fact]
    public void CheckboxShouldOutputTheValueWhenCheck()
    {
        bool isChecked = false;

        IRenderedComponent<Checkbox> checkbox = RenderComponent<Checkbox>(parameters =>
            parameters.Add(p => p.Label, "Test")
                      .Bind(p => p.Value, isChecked, value => isChecked = value));

        IElement input = checkbox.Find("input");

        input.Change(value: true);

        isChecked.Should().BeTrue();
    }
}
