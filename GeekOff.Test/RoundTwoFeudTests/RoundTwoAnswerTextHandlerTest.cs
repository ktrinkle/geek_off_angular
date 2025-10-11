namespace GeekOff.Test.RoundTwoFeudTests;

public class RoundTwoAnswerTextHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;

    private static readonly List<Scoring> initialScoring =
    [
        new()
        {
            Yevent = "t24",
            TeamAnswer = "CLT",
            TeamNum = 2,
            QuestionNum = 1,
            PointAmt = 0
        },
        new()
        {
            Yevent = "t24",
            QuestionNum = 202,
            TeamNum = 1,
            PlayerNum = 2,
            TeamAnswer = "JIM",
            PointAmt = 15
        },        
    ];
    private readonly DbSet<Scoring> mockScoring = initialScoring.AsQueryable().BuildMockDbSet();

    public RoundTwoAnswerTextHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundTwoAnswerTextHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Scoring.Returns(mockScoring);

    }

    [Fact]
    public async Task Handle_RoundTwoAnswerText_Normal()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoAnswerTextHandler.Handler(_contextGo);
        var request = new RoundTwoAnswerTextHandler.Request(){
            YEvent = "t24",
            QuestionNum = 201,
            TeamNum = 1,
            PlayerNum = 1,
            Answer = "BOB",
            Score = 15
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("The answer was successfully saved.", result.Value.Message);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerManual_RemoveAnswer()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoAnswerTextHandler.Handler(_contextGo);
        var request = new RoundTwoAnswerTextHandler.Request(){
            YEvent = "t24",
            QuestionNum = 202,
            TeamNum = 1,
            PlayerNum = 2,
            Answer = "BOB",
            Score = 25
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("The answer was successfully saved.", result.Value.Message);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerManual_EmptyEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoAnswerTextHandler.Handler(_contextGo);
        var request = new RoundTwoAnswerTextHandler.Request(){
            YEvent = String.Empty,
            QuestionNum = 201,
            TeamNum = 1,
            PlayerNum = 1,
            Answer = "BOB",
            Score = 15
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("The event ID is not valid. Please try again.", result.Value.Message);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerManual_BadQuestionNumLow()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoAnswerTextHandler.Handler(_contextGo);
        var request = new RoundTwoAnswerTextHandler.Request(){
            YEvent = "t24",
            QuestionNum = 150,
            TeamNum = 1,
            PlayerNum = 1,
            Answer = "BOB",
            Score = 15
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("The question is not a valid round 2 question. Please try again.", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerManual_BadQuestionNumHigh()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoAnswerTextHandler.Handler(_contextGo);
        var request = new RoundTwoAnswerTextHandler.Request(){
            YEvent = "t24",
            QuestionNum = 300,
            TeamNum = 1,
            PlayerNum = 1,
            Answer = "BOB",
            Score = 15
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("The question is not a valid round 2 question. Please try again.", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerManual_InvalidTeam()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoAnswerTextHandler.Handler(_contextGo);
        var request = new RoundTwoAnswerTextHandler.Request(){
            YEvent = "t24",
            QuestionNum = 201,
            TeamNum = -1,
            PlayerNum = 1,
            Answer = "BOB",
            Score = 15
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("A valid team number is required.", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerManual_BadPlayerLow()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoAnswerTextHandler.Handler(_contextGo);
        var request = new RoundTwoAnswerTextHandler.Request(){
            YEvent = "t24",
            QuestionNum = 201,
            TeamNum = 1,
            PlayerNum = -1,
            Answer = "BOB",
            Score = 15
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("The player number is not valid. Please try again.", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerManual_BadPlayerHigh()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoAnswerTextHandler.Handler(_contextGo);
        var request = new RoundTwoAnswerTextHandler.Request(){
            YEvent = "t24",
            QuestionNum = 201,
            TeamNum = 1,
            PlayerNum = 4,
            Answer = "BOB",
            Score = 15
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("The player number is not valid. Please try again.", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }
}
