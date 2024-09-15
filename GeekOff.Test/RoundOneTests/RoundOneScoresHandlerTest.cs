namespace GeekOff.Test.RoundOneTests;

public class RoundOneScoresHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<Scoring> initialScoring =
    [
            new()
            {
                Yevent = "t24",
                QuestionNum = 1,
                RoundNum = 1,
                TeamNum = 1,
                PointAmt = 10,
                Id = 1
            },
            new()
            {
                Yevent = "t24",
                QuestionNum = 2,
                RoundNum = 1,
                TeamNum = 1,
                PointAmt = 10,
                Id = 2
            },  
            new()
            {
                Yevent = "t24",
                QuestionNum = 3,
                RoundNum = 1,
                TeamNum = 1,
                PointAmt = 10,
                Id = 3
            },  
            new()
            {
                Yevent = "t24",
                QuestionNum = 1,
                RoundNum = 1,
                TeamNum = 2,
                PointAmt = 10,
                Id = 4
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 3,
                RoundNum = 1,
                TeamNum = 2,
                PointAmt = 10,
                Id = 5
            },
            new()
            {
                Yevent = "t24",
                QuestionNum = 3,
                RoundNum = 1,
                TeamNum = 3,
                PointAmt = 10,
                Id = 6
            }    
    ];
    private static readonly List<QuestionAns> initialQuestionAns =
    [
        new ()
        {
            Yevent = "t24",
            QuestionNum = 1,
            RoundNum = 1,
            MatchQuestion = false,
            TextQuestion = "Test 1",
            TextAnswer = "Test"
        },
        new ()
        {
            Yevent = "t24",
            QuestionNum = 2,
            RoundNum = 1,
            MatchQuestion = false,
            TextQuestion = "Test 2",
            TextAnswer = "Test"
        },
        new ()
        {
            Yevent = "t24",
            QuestionNum = 3,
            RoundNum = 1,
            MatchQuestion = false,
            TextQuestion = "Test 3",
            TextAnswer = "Test"
        }
    ];

    private static readonly List<Teamreference> initialTeamreference =
    [
        new ()
        {
            Yevent = "t24",
            TeamNum = 1,
            Teamname = "Test Team 1",
            Dollarraised = 10,
            TeamGuid = Guid.NewGuid(),
            LoginTime = DateTime.Now
        },
        new ()
        {
            Yevent = "t24",
            TeamNum = 2,
            Teamname = "Test Team 2",
            Dollarraised = 110,
            TeamGuid = Guid.NewGuid(),
            LoginTime = DateTime.Now
        },
        new ()
        {
            Yevent = "t24",
            TeamNum = 3,
            Teamname = "Test Team 3",
            Dollarraised = 300,
            TeamGuid = Guid.NewGuid(),
            LoginTime = DateTime.Now
        }
    ];

    private readonly DbSet<Scoring> mockScoring = initialScoring.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Teamreference> mockTeamReference = initialTeamreference.AsQueryable().BuildMockDbSet();
    private readonly DbSet<QuestionAns> mockQuestionAns = initialQuestionAns.AsQueryable().BuildMockDbSet();

    public RoundOneScoresHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundOneScoresHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Scoring.Returns(mockScoring);
        _contextGo.Teamreference.Returns(mockTeamReference);
        _contextGo.QuestionAns.Returns(mockQuestionAns);
    }

    [Fact]
    public async Task Handle_RoundOneScoresHandler_Scoreboard()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundOneScoresHandler.Handler(_contextGo);
        var request = new RoundOneScoresHandler.Request(){
            YEvent = "t24"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value!);
        Assert.Equal(3, result.Value!.Count);
        Assert.Equal("TEST TEAM 1", result.Value![0].TeamName);
        Assert.Equal(1, result.Value![0].TeamNum);
        Assert.Equal("TEST TEAM 3", result.Value![2].TeamName);
        Assert.Equal(3, result.Value![2].TeamNum);
        Assert.Equal(20, result.Value![2].TeamScore);
        Assert.Equal(21, result.Value![1].TeamScore);
        Assert.Equal(30, result.Value![0].TeamScore);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundOneScoresHandler_BadEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundOneScoresHandler.Handler(_contextGo);
        var request = new RoundOneScoresHandler.Request(){
            YEvent = string.Empty
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}
