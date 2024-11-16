// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using AngleSharp.Dom;
using Atlas.Application.Countries.Responses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Net.Mime;

namespace Atlas.Web.App.Modals;

public sealed class ZoomModalTests : TestContext
{
    private readonly ImageResponse _image = new(new Uri("https://image.svg"), MediaTypeNames.Image.Svg);

    public ZoomModalTests()
    {
        Services.AddSingleton((IJSInProcessRuntime)JSInterop.JSRuntime);
        Services.AddLocalization();

        JSInterop.SetupVoid("showModal", _ => true).SetVoidResult();
        JSInterop.SetupVoid("closeModal", _ => true).SetVoidResult();
    }

    [Fact]
    public void ShowShouldInvokeJavascript()
    {
        IRenderedComponent<ZoomModal> modal = RenderComponent<ZoomModal>(parameters =>
            parameters.Add(z => z.Image, _image));

        modal.Instance.Show();

        JSInterop.VerifyInvoke("showModal");
    }

    [Fact]
    public void ModalShouldCloseTheModal()
    {
        IRenderedComponent<ZoomModal> modal = RenderComponent<ZoomModal>(parameters =>
            parameters.Add(z => z.Image, _image));

        IElement dialog = modal.Find("dialog");

        dialog.Click();

        JSInterop.VerifyInvoke("closeModal");
    }
}
