namespace GeekOff.Test.SharedTests;

public class SubmitAnswerHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly ILogger<SubmitAnswerHandler> _logger;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<LogError> initialLogError = [];
    private static readonly List<UserAnswer> initialUserAnswer =
        [
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 1,
                QuestionNum  = 1,
                TextAnswer = "Help",
                AnswerUser = "2",
                AnswerTime = DateTime.UtcNow
            },
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 3,
                QuestionNum  = 1,
                TextAnswer = "Help",
                AnswerUser = "3",
                AnswerTime = DateTime.UtcNow
            },
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 4,
                QuestionNum  = 1,
                TextAnswer = "Answer",
                AnswerUser = "4",
                AnswerTime = DateTime.UtcNow
            }
        ];
    private readonly DbSet<UserAnswer> mockUserAnswer = initialUserAnswer.AsQueryable().BuildMockDbSet();
    private readonly DbSet<LogError> mockLogError = initialLogError.AsQueryable().BuildMockDbSet();

    public SubmitAnswerHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        _logger = Substitute.For<ILogger<SubmitAnswerHandler>>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(SubmitAnswerHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.LogError.Returns(mockLogError);
        _contextGo.UserAnswer.Returns(mockUserAnswer);

    }

    [Fact]
    public async Task Handle_SubmitAnswerHandlerRoundOne_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 1,
            QuestionNum = 2,
            TeamNum = 1,
            TextAnswer = "Testing"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Your answer is in. Good luck!", result.Value.Message);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_SubmitAnswerHandlerRoundOne_BadRoundNumUpper()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 5,
            QuestionNum = 2,
            TeamNum = 1,
            TextAnswer = "Testing"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Incorrect round number.", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SubmitAnswerHandlerRoundOne_BadRoundNumLower()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 0,
            QuestionNum = 2,
            TeamNum = 1,
            TextAnswer = "Testing"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Incorrect round number.", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SubmitAnswerHandlerRoundOne_BadQuestionUpper()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 1,
            QuestionNum = 200,
            TeamNum = 1,
            TextAnswer = "Testing"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Bad Question - Question 200 Team ID 1 YEvent t24", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SubmitAnswerHandlerRoundOne_BadQuestionLower()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 1,
            QuestionNum = 0,
            TeamNum = 1,
            TextAnswer = "Testing"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Bad Question - Question 0 Team ID 1 YEvent t24", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SubmitAnswerHandlerRoundTwo_BadQuestionUpper()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 2,
            QuestionNum = 300,
            TeamNum = 1,
            TextAnswer = "Testing"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Bad Question - Question 300 Team ID 1 YEvent t24", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SubmitAnswerHandlerRoundTwo_BadQuestionLower()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 2,
            QuestionNum = 0,
            TeamNum = 1,
            TextAnswer = "Testing"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Bad Question - Question 0 Team ID 1 YEvent t24", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SubmitAnswerHandlerRoundThree_BadQuestionUpper()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 3,
            QuestionNum = 400,
            TeamNum = 1,
            TextAnswer = "Testing"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Bad Question - Question 400 Team ID 1 YEvent t24", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SubmitAnswerHandlerRoundThree_BadQuestionLower()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 3,
            QuestionNum = 0,
            TeamNum = 1,
            TextAnswer = "Testing"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Bad Question - Question 0 Team ID 1 YEvent t24", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SubmitAnswerHandler_BadTeamNumber()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 3,
            QuestionNum = 304,
            TeamNum = 0,
            TextAnswer = "Testing"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Zero team - Question 304 Team ID 0 YEvent t24", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SubmitAnswerHandler_EmptyAnswer()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SubmitAnswerHandler.Handler(_contextGo, _logger);
        var request = new SubmitAnswerHandler.Request(){
            YEvent = "t24",
            RoundNum = 1,
            QuestionNum = 5,
            TeamNum = 2,
            TextAnswer = ""
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Null answer - Question 5 Team ID 2 YEvent t24", result.Value.Message);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

}
