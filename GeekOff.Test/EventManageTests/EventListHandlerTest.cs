using GeekOff.Models;

namespace GeekOff.Test.EventManageTests;

public class EventListHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;

    public EventListHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(EventListHandler).Assembly))
            .AddLogging().BuildServiceProvider();
    }

    [Fact]
    public async Task Handle_ReturnsListOfEventMaster()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new EventListHandler.Handler(_contextGo);
        var request = new EventListHandler.Request();
        var expectedResult = new List<EventMaster>() 
        {
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
        }.AsQueryable();

        var mock = expectedResult.AsQueryable().BuildMockDbSet();

        _contextGo.EventMaster.Returns(mock);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(result.Value.Count, expectedResult.Count());
        Assert.Equal(QueryStatus.Success, result.Status);
    }
}