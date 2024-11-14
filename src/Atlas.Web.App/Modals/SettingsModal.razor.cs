// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Stores.Changelog;
using Fluxor;
using Markdig;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Atlas.Web.App.Modals;

public sealed partial class SettingsModal(IJSInProcessRuntime jsRuntime, IDispatcher dispatcher, IActionSubscriber subscriber) : IDisposable
{
    private MarkupString _changelog;

    [Parameter]
    public RenderFragment? ChildContent { get; init; }

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    public void Show()
    {
        jsRuntime.InvokeVoid("showModal");
        jsRuntime.InvokeVoid("scrollContentToTop");
    }

    protected override void OnInitialized()
    {
        subscriber.SubscribeToAction<ChangelogActions.GetChangelogResult>(this, action =>
        {
            _changelog = AsHtml(action.Changelog);
            StateHasChanged();
        });

        dispatcher.Dispatch(new ChangelogActions.GetChangelog());

        static MarkupString AsHtml(string changelog)
        {
            MarkdownDocument document = Markdown.Parse(changelog);

            HtmlAttributes linkAttributes = new();
            linkAttributes.AddClass("link");
            linkAttributes.AddProperty("target", "_blank");

            HtmlAttributes headingAttributes = new();
            headingAttributes.AddClass("version");

            HtmlAttributes listAttributes = new();
            listAttributes.AddClass("section");

            foreach (MarkdownObject descendant in document.Descendants())
            {
                if (descendant is HeadingBlock { Level: 2 })
                    descendant.SetAttributes(headingAttributes);

                if (descendant is LinkInline)
                    descendant.SetAttributes(linkAttributes);

                if (descendant is ListBlock)
                    descendant.SetAttributes(listAttributes);
            }

            return new MarkupString(document.ToHtml());
        }
    }

    private void Close() => jsRuntime.InvokeVoid("closeModal");
}
