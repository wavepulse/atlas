// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Web.App.Stores.Changelog;
using Fluxor;
using Markdig;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;

namespace Atlas.Web.App.Settings.Components;

public sealed partial class Changelog(IDispatcher dispatcher, IActionSubscriber subscriber) : IDisposable
{
    private string _changelog = string.Empty;
    private bool _isLoading;

    public void Dispose() => subscriber.UnsubscribeFromAllActions(this);

    protected override void OnInitialized()
    {
        subscriber.SubscribeToAction<ChangelogActions.GetChangelogResult>(this, action =>
        {
            _changelog = action.Changelog;
            _isLoading = false;

            StateHasChanged();
        });

        _isLoading = true;
        dispatcher.Dispatch(new ChangelogActions.GetChangelog());
    }

    private MarkupString DisplayChangelogContent()
    {
        MarkdownDocument document = Markdown.Parse(_changelog);

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
