// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Commons;
using Atlas.Application.Fakes;
using Atlas.Application.Flags.Abstractions;
using Atlas.Domain.Flags;
using FlagCodeContract = Atlas.Contracts.Flags.FlagCode;

namespace Atlas.Application.Flags.Handlers;

public sealed class RandomizeQueryHandlerTests
{
    private readonly IFlagRepository _flagRepository = Substitute.For<IFlagRepository>();
    private readonly IRandomizer _randomizer = Substitute.For<IRandomizer>();

    private readonly RandomizeQueryHandler _handler;

    public RandomizeQueryHandlerTests()
    {
        _randomizer.Randomize(Arg.Any<IEnumerable<Flag>>()).Returns(FakeFlags.Canada);

        _handler = new RandomizeQueryHandler(_flagRepository, _randomizer);
    }

    [Fact]
    public async Task HandleShouldRetrieveAllFlags()
    {
        await _handler.Handle(new FlagRequests.Randomize(), CancellationToken.None);

        await _flagRepository.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Fact]
    public async Task HandleShouldRandomizeAllFlags()
    {
        Flag[] flags = [];

        _flagRepository.GetAllAsync(CancellationToken.None).Returns(flags);

        await _handler.Handle(new FlagRequests.Randomize(), CancellationToken.None);

        _randomizer.Received(1).Randomize(flags);
    }

    [Fact]
    public async Task HandleShouldReturnTheRandomizedFlagCode()
    {
        Flag[] flags = [FakeFlags.Canada];

        _flagRepository.GetAllAsync(CancellationToken.None).Returns(flags);
        _randomizer.Randomize(flags).Returns(FakeFlags.Canada);

        FlagCodeContract code = await _handler.Handle(new FlagRequests.Randomize(), CancellationToken.None);

        code.Cca2.Should().Be(FakeFlags.Canada.Code.Cca2);
    }
}
