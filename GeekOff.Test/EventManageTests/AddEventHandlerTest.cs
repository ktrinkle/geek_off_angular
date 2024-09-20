namespace GeekOff.Test.EventManageTests;

public class AddEventHandlerTest
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
                SelEvent = true
            },
            new()
            {
                Yevent = "t21",
                EventName = "Test 2021",
                SelEvent = false                    
            }
        ];
    private readonly DbSet<EventMaster> mock = initialEventData.AsQueryable().BuildMockDbSet();

    public AddEventHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddEventHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.EventMaster.Returns(mock);

    }

    [Fact]
    public async Task Handle_AddEventMaster()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new AddEventHandler.Handler(_contextGo);
        var request = new AddEventHandler.Request(){
            Yevent = "t24",
            EventName = "Test 2024",
            SelEvent = false
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("The new event was successfully added to the system.", result.Value.Message!);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_DuplicatedEventId()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new AddEventHandler.Handler(_contextGo);
        var request = new AddEventHandler.Request(){
            Yevent = "t21",
            EventName = "Test 2021",
            SelEvent = false
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("The created event already exists. Please create a new code.", result.Value.Message!);
        Assert.Equal(QueryStatus.Conflict, result.Status);
    }

}