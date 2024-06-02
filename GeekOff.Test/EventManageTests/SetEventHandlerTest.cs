using GeekOff.Models;

namespace GeekOff.Test.EventManageTests;

public class SetEventHandlerTest
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

    public SetEventHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(SetEventHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.EventMaster.Returns(mock);

    }

    [Fact]
    public async Task Handle_SetEventMaster()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetEventHandler.Handler(_contextGo);
        var request = new SetEventHandler.Request(){
            YEvent = "t21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("The selected event was made active.", result.Value.Message!);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_SetEventMissingEventId()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetEventHandler.Handler(_contextGo);
        var request = new SetEventHandler.Request(){
            YEvent = "s21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("The selected event does not exist.", result.Value.Message!);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

}