namespace GeekOff.Test.EventManageTests;

public class TeamListWithPlayersHandlerTest
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

    private readonly static List<TeamUser> initialTeamUser = 
    [
        new()
        {
            Yevent = "t21",
            TeamNum = 1,
            PlayerName = "Bob",
            PlayerNum = 1,
            WorkgroupName = "Airport"
        },
        new()
        {
            Yevent = "t21",
            TeamNum = 1,
            PlayerName = "Bill",
            PlayerNum = 2,
            WorkgroupName = "Airport"
        },
        new()
        {
            Yevent = "t21",
            TeamNum = 3,
            PlayerName = "Matt",
            PlayerNum = 1,
            WorkgroupName = "Airport"
        },
        new()
        {
            Yevent = "t21",
            TeamNum = 3,
            PlayerName = "Mateo",
            PlayerNum = 2,
            WorkgroupName = "Along for the ride"
        },
        new()
        {
            Yevent = "t21",
            TeamNum = 4,
            PlayerName = "Lone Ranger",
            PlayerNum = 1,
            WorkgroupName = "Wild West"
        },
    ];

    private readonly DbSet<Teamreference> mockTeamReference = initialTeamData.AsQueryable().BuildMockDbSet();
    private readonly DbSet<TeamUser> mockTeamUser = initialTeamUser.AsQueryable().BuildMockDbSet();

    public TeamListWithPlayersHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(TeamListWithPlayersHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Teamreference.Returns(mockTeamReference);
        _contextGo.TeamUser.Returns(mockTeamUser);
    }

    [Fact]
    public async Task Handle_TeamListMaster()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new TeamListWithPlayersHandler.Handler(_contextGo);
        var request = new TeamListWithPlayersHandler.Request(){
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
    public async Task Handle_TeamListNotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new TeamListWithPlayersHandler.Handler(_contextGo);
        var request = new TeamListWithPlayersHandler.Request(){
            YEvent = "o21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}