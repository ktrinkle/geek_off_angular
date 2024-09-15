namespace GeekOff.Test.RoundTwoFeudTests;

public class RoundTwoFirstPlayerHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<Scoring> initialScoring = 
        [
            new ()
            {
                Yevent = "t24",
                RoundNum = 2,
                QuestionNum = 201,
                TeamNum = 1,
                PlayerNum = 1,
                TeamAnswer = "This is an answer",
                PointAmt = 61
            },
            new ()
            {
                Yevent = "t24",
                RoundNum = 2,
                QuestionNum = 202,
                TeamNum = 1,
                PlayerNum = 1,
                TeamAnswer = "This is not an answer",
                PointAmt = 22
            },
            new ()
            {
                Yevent = "t24",
                RoundNum = 2,
                QuestionNum = 201,
                TeamNum = 1,
                PlayerNum = 2,
                TeamAnswer = "This is less of an answer",
                PointAmt = 22
            },
            new ()
            {
                Yevent = "t24",
                RoundNum = 2,
                QuestionNum = 201,
                TeamNum = 2,
                PlayerNum = 1,
                TeamAnswer = "This is more of an answer",
                PointAmt = 26
            },
        ];

    private readonly DbSet<Scoring> mockScoring = initialScoring.AsQueryable().BuildMockDbSet();

    public RoundTwoFirstPlayerHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundTwoFirstPlayerHandler).Assembly))
            .BuildServiceProvider();

        _contextGo.Scoring.Returns(mockScoring);

    }

    [Fact]
    public async Task Handle_RoundTwoFirstPlayerHandler_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoFirstPlayerHandler.Handler(_contextGo);
        var request = new RoundTwoFirstPlayerHandler.Request(){
            YEvent = "t24",
            TeamNum = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value!);
        Assert.Equal(2, result.Value!.Count);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundTwoFirstPlayerHandler_NotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoFirstPlayerHandler.Handler(_contextGo);
        var request = new RoundTwoFirstPlayerHandler.Request(){
            YEvent = "t21",
            TeamNum = 3
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_RoundTwoFirstPlayerHandler_BadTeamNum()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoFirstPlayerHandler.Handler(_contextGo);
        var request = new RoundTwoFirstPlayerHandler.Request(){
            YEvent = "t24",
            TeamNum = 1203
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_RoundTwoFirstPlayerHandler_BadEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoFirstPlayerHandler.Handler(_contextGo);
        var request = new RoundTwoFirstPlayerHandler.Request(){
            YEvent = string.Empty,
            TeamNum = 2
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }
}
