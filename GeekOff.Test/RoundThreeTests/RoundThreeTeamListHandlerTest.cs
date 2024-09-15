namespace GeekOff.Test.RoundThreeTests;

public class RoundThreeTeamListHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<Teamreference> initialTeamreference = 
        [
            new()
            {
                Yevent = "t24",
                TeamNum = 1,
                Teamname = "Team 1",
                TeamGuid = Guid.NewGuid(),
                Dollarraised = 200
            },
            new()
            {
                Yevent = "t24",
                TeamNum = 2,
                Teamname = "Team 2",
                TeamGuid = Guid.NewGuid(),
                Dollarraised = 150
            },
            new()
            {
                Yevent = "t24",
                TeamNum = 3,
                Teamname = "Team 3",
                TeamGuid = Guid.NewGuid(),
                Dollarraised = 111
            },
            new()
            {
                Yevent = "t24",
                TeamNum = 4,
                Teamname = "Team 4",
                TeamGuid = Guid.NewGuid(),
                Dollarraised = 0
            },
            new()
            {
                Yevent = "t24",
                TeamNum = 5,
                Teamname = "Team 5",
                TeamGuid = Guid.NewGuid(),
                Dollarraised = 50
            },
            new()
            {
                Yevent = "t24",
                TeamNum = 6,
                Teamname = "Team 6",
                TeamGuid = Guid.NewGuid(),
                Dollarraised = 100
            },
            new()
            {
                Yevent = "t24",
                TeamNum = 7,
                Teamname = "Team 7",
                TeamGuid = Guid.NewGuid(),
                Dollarraised = 10
            }
        ];

    private static readonly List<Roundresult> initialRoundresult =
    [
        new()
        {
            Yevent = "t24",
            RoundNum = 1,
            TeamNum = 1,
            Ptswithbonus = 40,
            Rnk = 1
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 1,
            TeamNum = 2,
            Ptswithbonus = 25,
            Rnk = 2
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 1,
            TeamNum = 3,
            Ptswithbonus = 16,
            Rnk = 4
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 1,
            TeamNum = 4,
            Ptswithbonus = 18,
            Rnk = 3
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 1,
            TeamNum = 5,
            Ptswithbonus = 6,
            Rnk = 6
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 1,
            TeamNum = 6,
            Ptswithbonus = 10,
            Rnk = 5
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 1,
            TeamNum = 7,
            Ptswithbonus = 2,
            Rnk = 7
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 2,
            TeamNum = 6,
            Ptswithbonus = 30,
            Rnk = 1
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 2,
            TeamNum = 5,
            Ptswithbonus = 29,
            Rnk = 2
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 2,
            TeamNum = 4,
            Ptswithbonus = 15,
            Rnk = 4
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 2,
            TeamNum = 3,
            Ptswithbonus = 27,
            Rnk = 3
        }
    ];

    private readonly DbSet<Roundresult> mockRoundResult = initialRoundresult.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Teamreference> mockTeamReference = initialTeamreference.AsQueryable().BuildMockDbSet();

    public RoundThreeTeamListHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundThreeTeamListHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Roundresult.Returns(mockRoundResult);
        _contextGo.Teamreference.Returns(mockTeamReference);

    }

    [Fact]
    public async Task Handle_RoundThreeTeamListHandler_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeTeamListHandler.Handler(_contextGo);
        var request = new RoundThreeTeamListHandler.Request(){
            YEvent = "t24"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Value!.Count);
        Assert.Equal(6, result.Value[0].TeamNum);
        Assert.Equal(5, result.Value[1].TeamNum);
        Assert.Equal(3, result.Value[2].TeamNum);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundThreeTeamList_BadEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeTeamListHandler.Handler(_contextGo);
        var request = new RoundThreeTeamListHandler.Request(){
            YEvent = string.Empty
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_RoundThreeTeamList_MissingEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeTeamListHandler.Handler(_contextGo);
        var request = new RoundThreeTeamListHandler.Request(){
            YEvent = "t22"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}
