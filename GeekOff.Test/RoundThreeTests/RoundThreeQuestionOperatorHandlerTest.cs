namespace GeekOff.Test.RoundTwoFeudTests;

public class RoundThreeQuestionOperatorHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<Scoreposs> initialScoreposs = 
        [
            new ()
            {
                Yevent = "t24",
                RoundNum = 3,
                QuestionNum = 301,
                QuestionAnswer = "DFW",
                Ptsposs = 100
            },
            new ()
            {
                Yevent = "t24",
                RoundNum = 3,
                QuestionNum = 302,
                QuestionAnswer = "SAT",
                Ptsposs = 200
            },
            new ()
            {
                Yevent = "t24",
                RoundNum = 3,
                QuestionNum = 313,
                QuestionAnswer = "Brother Boy",
                Ptsposs = 300
            },
            new ()
            {
                Yevent = "t24",
                RoundNum = 3,
                QuestionNum = 314,
                QuestionAnswer = "Lavonda",
                Ptsposs = 400
            }
        ];

    private readonly DbSet<Scoreposs> mockScoreposs = initialScoreposs.AsQueryable().BuildMockDbSet();

    public RoundThreeQuestionOperatorHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundThreeQuestionOperatorHandler).Assembly))
            .BuildServiceProvider();

        _contextGo.Scoreposs.Returns(mockScoreposs);

    }

    [Fact]
    public async Task Handle_RoundThreeQuestionOperatorHandler_Questions()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeQuestionOperatorHandler.Handler(_contextGo);
        var request = new RoundThreeQuestionOperatorHandler.Request(){
            YEvent = "t24"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value!);
        Assert.Equal(4, result.Value!.Count);
        Assert.Equal(301, result.Value[0].QuestionNum);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundThreeQuestionOperatorHandler_NotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeQuestionOperatorHandler.Handler(_contextGo);
        var request = new RoundThreeQuestionOperatorHandler.Request(){
            YEvent = "t21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_RoundThreeQuestionOperatorHandler_BlankEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeQuestionOperatorHandler.Handler(_contextGo);
        var request = new RoundThreeQuestionOperatorHandler.Request(){
            YEvent = string.Empty
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }
}
