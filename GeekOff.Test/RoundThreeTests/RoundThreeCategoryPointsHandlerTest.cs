using Microsoft.VisualBasic;

namespace GeekOff.Test.RoundThreeTests;

public class RoundThreeCategoryPointsHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;

    private readonly static List<Scoreposs> initialScoreposs = [
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 301,
            QuestionAnswer = "",
            Ptsposs = 100
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 302,
            QuestionAnswer = "",
            Ptsposs = 200
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 303,
            QuestionAnswer = "",
            Ptsposs = 300
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 304,
            QuestionAnswer = "",
            Ptsposs = 400
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 311,
            QuestionAnswer = "",
            Ptsposs = 100
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 312,
            QuestionAnswer = "",
            Ptsposs = 200
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 313,
            QuestionAnswer = "",
            Ptsposs = 300
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 314,
            QuestionAnswer = "",
            Ptsposs = 400
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 321,
            QuestionAnswer = "",
            Ptsposs = 100
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 322,
            QuestionAnswer = "",
            Ptsposs = 200
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 323,
            QuestionAnswer = "",
            Ptsposs = 300
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 324,
            QuestionAnswer = "",
            Ptsposs = 400
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 331,
            QuestionAnswer = "",
            Ptsposs = 100
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 332,
            QuestionAnswer = "",
            Ptsposs = 200
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 333,
            QuestionAnswer = "",
            Ptsposs = 300
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 334,
            QuestionAnswer = "",
            Ptsposs = 400
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 341,
            QuestionAnswer = "",
            Ptsposs = 100
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 342,
            QuestionAnswer = "",
            Ptsposs = 200
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 343,
            QuestionAnswer = "",
            Ptsposs = 300
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 344,
            QuestionAnswer = "",
            Ptsposs = 400
        },
        new ()
        {
            Yevent = "t24",
            RoundNum = 3,
            QuestionNum = 350,
            QuestionAnswer = "",
            Ptsposs = 0
        },
    ]
    
    private readonly DbSet<Scoreposs> mockScorePoss = initialScoreposs.AsQueryable().BuildMockDbSet();

    public RoundThreeCategoryPointsHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundThreeCategoryPointsHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Scoreposs.Returns(mockScorePoss);

    }

    [Fact]
    public async Task Handle_RoundThreeCategoryPointsHandler_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeCategoryPointsHandler.Handler(_contextGo);
        var request = new RoundThreeCategoryPointsHandler.Request(){
            YEvent = "t24"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(20, result.Value!.Count);
        Assert.Equal(301, result.Value[0].QuestionNum);
        Assert.Equal(314, result.Value[7].QuestionNum);
        Assert.Equal(344, result.Value[19].QuestionNum);
        Assert.Equal(100, result.Value[0].Ptsposs);
        Assert.Equal(200, result.Value[5].Ptsposs);
        Assert.Equal(400, result.Value[7].Ptsposs);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundThreeCategoryPointsHandler_BadEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeCategoryPointsHandler.Handler(_contextGo);
        var request = new RoundThreeCategoryPointsHandler.Request(){
            YEvent = string.Empty
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_RoundThreeCategoryPointsHandler_MissingEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeCategoryPointsHandler.Handler(_contextGo);
        var request = new RoundThreeCategoryPointsHandler.Request(){
            YEvent = "t22"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}
