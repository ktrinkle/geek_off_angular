using Microsoft.VisualBasic;

namespace GeekOff.Test.RoundThreeTests;

public class RoundThreeGeekOMaticScoreHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;

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

    private static readonly List<Scoring> initialScoring = 
    [
        new()
        {
            Yevent = "t24",
            RoundNum = 3,
            TeamNum = 6,
            QuestionNum = 301,
            PointAmt = -100
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 3,
            TeamNum = 6,
            QuestionNum = 311,
            PointAmt = 200
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 3,
            TeamNum = 5,
            QuestionNum = 301,
            PointAmt = 100
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 3,
            TeamNum = 5,
            QuestionNum = 321,
            PointAmt = 300
        },
        new()
        {
            Yevent = "t24",
            RoundNum = 3,
            TeamNum = 3,
            QuestionNum = 321,
            PointAmt = 10000
        },
    ];

    private readonly DbSet<Roundresult> mockRoundResult = initialRoundresult.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Scoring> mockScoring = initialScoring.AsQueryable().BuildMockDbSet();

    public RoundThreeGeekOMaticScoreHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundThreeGeekOMaticScoreHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Roundresult.Returns(mockRoundResult);
        _contextGo.Scoring.Returns(mockScoring);

    }

    [Fact]
    public async Task Handle_RoundThreeGeekOMaticScoreHandler_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeGeekOMaticScoreHandler.Handler(_contextGo);
        var request = new RoundThreeGeekOMaticScoreHandler.Request(){
            YEvent = "t24"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Value!.Count);
        Assert.Equal(6, result.Value[0].TeamNum);
        Assert.Equal(5, result.Value[1].TeamNum);
        Assert.Equal(3, result.Value[2].TeamNum);
        Assert.Equal("B", result.Value[0].TeamColor);
        Assert.Equal("G", result.Value[1].TeamColor);
        Assert.Equal("R", result.Value[2].TeamColor);
        Assert.Equal(" 100", result.Value[0].TeamScore);
        Assert.Equal(" 400", result.Value[1].TeamScore);
        Assert.Equal("9999", result.Value[2].TeamScore);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundThreeTeamColorHandler_BadEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeGeekOMaticScoreHandler.Handler(_contextGo);
        var request = new RoundThreeGeekOMaticScoreHandler.Request(){
            YEvent = string.Empty
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_RoundThreeTeamColorHandler_MissingEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeGeekOMaticScoreHandler.Handler(_contextGo);
        var request = new RoundThreeGeekOMaticScoreHandler.Request(){
            YEvent = "t22"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}
