namespace GeekOff.Test.SharedTests;

public class GetScoresHandlerTest
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
    private static readonly List<Scoring> initialScoring =
        [
            new()
            {
                Yevent = "t24",
                QuestionNum = 1,
                RoundNum = 1,
                TeamNum = 1,
                PointAmt = 10
            },
            new()
            {
                Yevent = "t24",
                QuestionNum = 1,
                RoundNum = 1,
                TeamNum = 2,
                PointAmt = 10
            },
            new()
            {
                Yevent = "t24",
                QuestionNum = 2,
                RoundNum = 1,
                TeamNum = 1,
                PointAmt = 10
            },
            new()
            {
                Yevent = "t24",
                QuestionNum = 2,
                RoundNum = 1,
                TeamNum = 3,
                PointAmt = 10
            },    
            new()
            {
                Yevent = "t24",
                QuestionNum = 3,
                RoundNum = 1,
                TeamNum = 3,
                PointAmt = 5
            },   
            new()
            {
                Yevent = "t24",
                QuestionNum = 3,
                RoundNum = 1,
                TeamNum = 4,
                PointAmt = 10
            },      
            new()
            {
                Yevent = "t24",
                QuestionNum = 2,
                RoundNum = 1,
                TeamNum = 4,
                PointAmt = 8
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 2,
                RoundNum = 1,
                TeamNum = 5,
                PointAmt = 6
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 1,
                RoundNum = 1,
                TeamNum = 6,
                PointAmt = 10
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 2,
                RoundNum = 1,
                TeamNum = 7,
                PointAmt = 2
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 201,
                RoundNum = 2,
                TeamNum = 6,
                PointAmt = 10
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 202,
                RoundNum = 2,
                TeamNum = 6,
                PointAmt = 10
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 203,
                RoundNum = 2,
                TeamNum = 6,
                PointAmt = 10
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 201,
                RoundNum = 2,
                TeamNum = 5,
                PointAmt = 8
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 202,
                RoundNum = 2,
                TeamNum = 5,
                PointAmt = 16
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 203,
                RoundNum = 2,
                TeamNum = 5,
                PointAmt = 5
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 203,
                RoundNum = 2,
                TeamNum = 4,
                PointAmt = 15
            }, 
            new()
            {
                Yevent = "t24",
                QuestionNum = 202,
                RoundNum = 2,
                TeamNum = 3,
                PointAmt = 27
            },
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

    private readonly DbSet<Scoring> mockScoring = initialScoring.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Roundresult> mockRoundResult = initialRoundresult.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Teamreference> mockTeamReference = initialTeamreference.AsQueryable().BuildMockDbSet();

    public GetScoresHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetScoresHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Scoring.Returns(mockScoring);
        _contextGo.Roundresult.Returns(mockRoundResult);
        _contextGo.Teamreference.Returns(mockTeamReference);

    }

    [Fact]
    public async Task Handle_GetScores_Round2_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetScoresHandler.Handler(_contextGo);
        var request = new GetScoresHandler.Request(){
            YEvent = "t24",
            RoundNum = 2,
            MaxRnk = 6
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(6, result.Value!.Count);
        Assert.Equal(1, result.Value[0].TeamNum);
        Assert.Null(result.Value[0].TeamScore);
        Assert.Equal(5, result.Value[5].TeamNum);
        Assert.Equal(29, result.Value[5].TeamScore);
        Assert.Equal(QueryStatus.Success, result.Status);

        // 6 = 30
        // 5 = 29
        // 4 = 15
        // 3 = 27
        // 2 = 0
        // 1 = 0
    }

    [Fact]
    public async Task Handle_GetScores_Round3_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetScoresHandler.Handler(_contextGo);
        var request = new GetScoresHandler.Request(){
            YEvent = "t24",
            RoundNum = 3,
            MaxRnk = 3
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Value!.Count);
        Assert.Equal(6, result.Value[0].TeamNum);
        Assert.Null(result.Value[0].TeamScore);
        Assert.Equal(5, result.Value[1].TeamNum);
        Assert.Null(result.Value[0].TeamScore);
        Assert.Equal(3, result.Value[2].TeamNum);
        Assert.Null(result.Value[2].TeamScore);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_GetScores_BadEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetScoresHandler.Handler(_contextGo);
        var request = new GetScoresHandler.Request(){
            YEvent = "t24",
            RoundNum = 1,
            MaxRnk = 8
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_GetScores_MissingEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetScoresHandler.Handler(_contextGo);
        var request = new GetScoresHandler.Request(){
            YEvent = "t22",
            RoundNum = 2,
            MaxRnk = 3
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}
