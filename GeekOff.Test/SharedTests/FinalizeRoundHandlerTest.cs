namespace GeekOff.Test.SharedTests;

public class FinalizeRoundHandlerTest
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
        ];

    private static readonly List<Roundresult> initialRoundresult =
    [
        new()
        {
            Yevent = "t23",
            RoundNum = 1,
            TeamNum = 1,
            Ptswithbonus = 100,
            Rnk = 1
        },
        new()
        {
            Yevent = "t23",
            RoundNum = 1,
            TeamNum = 2,
            Ptswithbonus = 75,
            Rnk = 2
        }
    ];

    private readonly DbSet<Scoring> mockScoring = initialScoring.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Roundresult> mockRoundResult = initialRoundresult.AsQueryable().BuildMockDbSet();

    public FinalizeRoundHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(FinalizeRoundHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Scoring.Returns(mockScoring);
        _contextGo.Roundresult.Returns(mockRoundResult);

    }

    [Fact]
    public async Task Handle_FinalizeRoundResult_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new FinalizeRoundHandler.Handler(_contextGo);
        var request = new FinalizeRoundHandler.Request(){
            YEvent = "t24",
            RoundNum = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("The selected round was finalized and saved to the system.", result.Value.Message);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_FinalizeRoundResult_MissingScores()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new FinalizeRoundHandler.Handler(_contextGo);
        var request = new FinalizeRoundHandler.Request(){
            YEvent = "t24",
            RoundNum = 3
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("We found no scores for this event and round.", result.Value.Message);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_FinalizeRoundResult_BadRound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new FinalizeRoundHandler.Handler(_contextGo);
        var request = new FinalizeRoundHandler.Request(){
            YEvent = "t24",
            RoundNum = 4
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Incorrect round number.", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }
}
