namespace GeekOff.Test.EventManageTests;

public class TeamListHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<Teamreference> initialTeamData =
        [
            new()
            {
                Yevent = "t21",
                Teamname = "Team 1",
                TeamNum = 1,
                Dollarraised = null,
                TeamGuid = Guid.NewGuid()
            },
            new()
            {
                Yevent = "t21",
                Teamname = "Team 3",
                TeamNum = 3,
                Dollarraised = 100,
                TeamGuid = Guid.NewGuid()                
            },
            new()
            {
                Yevent = "t21",
                Teamname = "Team 4",
                TeamNum = 4,
                Dollarraised = 101,
                TeamGuid = Guid.NewGuid()                
            }
        ];

    private readonly DbSet<Teamreference> mockTeamReference = initialTeamData.AsQueryable().BuildMockDbSet();

    public TeamListHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(TeamListHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Teamreference.Returns(mockTeamReference);
    }

    [Fact]
    public async Task Handle_TeamListMaster()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new TeamListHandler.Handler(_contextGo);
        var request = new TeamListHandler.Request(){
            YEvent = "t21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value!);
        Assert.Equal(result.Value!.Count, initialTeamData.Count);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_TeamListNoYEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new TeamListHandler.Handler(_contextGo);
        var request = new TeamListHandler.Request(){
            YEvent = "o21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Empty(result.Value!);
        Assert.Equal(QueryStatus.Success, result.Status);
    }
}