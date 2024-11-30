// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Countries.Queries;
using Atlas.Application.Countries.Responses;
using Fluxor;
using Mediator;

namespace Atlas.Web.App.Stores.DevMode;

public sealed class DevModeEffectTests
{
    private readonly IDispatcher _dispatcher = Substitute.For<IDispatcher>();
    private readonly ISender _sender = Substitute.For<ISender>();

    private readonly DevModeEffect _effect;

    public DevModeEffectTests()
    {
        _effect = new DevModeEffect(_sender);
    }

    [Fact]
    public async Task HandleGetCountryAsyncShouldSendGetCountryQuery()
    {
        DevModeActions.GetCountry action = new("CA");

        await _effect.HandleGetCountryAsync(action, _dispatcher);

        await _sender.Received(1).Send(Arg.Is<GetCountry.Query>(q => q.Cca2 == action.Cca2), CancellationToken.None);
    }

    [Fact]
    public async Task HandleGetCountryAsyncShouldDispatchGetCountryResult()
    {
        CountryResponse country = new("CA", "Canada", new ImageResponse(new Uri("https://image.com"), "image/png"), new Uri("https://map.com"));
        _sender.Send(Arg.Any<GetCountry.Query>(), CancellationToken.None).Returns(country);

        DevModeActions.GetCountry action = new("CA");

        await _effect.HandleGetCountryAsync(action, _dispatcher);

        _dispatcher.Received(1).Dispatch(Arg.Is<DevModeActions.GetCountryResult>(a => a.Country == country));
    }
}
