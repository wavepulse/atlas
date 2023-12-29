// Copyright (c) Pulsewave. All rights reserved.
// The source code is licensed under MIT License.

using Atlas.Application.Fakes;
using Atlas.Application.Flags.Abstractions;
using Atlas.Contracts.Flags;

namespace Atlas.Application.Flags.Handlers;

public sealed class GetAllQueryHandlerTests
{
    private readonly IFlagRepository _flagRepository = Substitute.For<IFlagRepository>();
    private readonly GetAllQueryHandler _handler;

    public GetAllQueryHandlerTests() => _handler = new(_flagRepository);

    [Fact]
    public async Task HandleShouldRetrieveAllFlagsFromRepository()
    {
        await _handler.Handle(new FlagRequests.GetAll(), CancellationToken.None);

        await _flagRepository.Received(1).GetAllAsync(CancellationToken.None);
    }

    [Fact]
    public async Task HandleShouldReturnAllFlags()
    {
        _flagRepository.GetAllAsync(CancellationToken.None).Returns([FakeFlags.Canada]);

        IEnumerable<Flag> flags = await _handler.Handle(new FlagRequests.GetAll(), CancellationToken.None);

        Flag flag = flags.Single();

        flag.Code.Cca2.Should().Be(FakeFlags.Canada.Code.Cca2);
    }
}
