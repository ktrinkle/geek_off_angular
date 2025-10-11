namespace GeekOff.Test.EventStatusTests;

public class GetCurrentEventHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<EventMaster> initialEventData =
        [
            new()
            {
                Yevent = "e21",
                EventName = "Employee 2021",
                SelEvent = false
            },
            new()
            {
                Yevent = "t21",
                EventName = "Test 2021",
                SelEvent = true                    
            }
        ];
    private readonly DbSet<EventMaster> mockEventMaster = initialEventData.AsQueryable().BuildMockDbSet();

    public GetCurrentEventHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetCurrentEventHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.EventMaster.Returns(mockEventMaster);
    }

    [Fact]
    public async Task Handle_GetCurrentEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetCurrentEventHandler.Handler(_contextGo);
        var request = new GetCurrentEventHandler.Request();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal("t21", result);
    }
}