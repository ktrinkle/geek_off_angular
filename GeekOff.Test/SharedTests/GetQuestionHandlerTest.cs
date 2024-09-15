namespace GeekOff.Test.SharedTests;

public class GetQuestionHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<QuestionAns> initialQuestions =
        [
            new()
            {
                Yevent = "t24",
                QuestionNum = 1,
                RoundNum = 1,
                TextAnswer = "ORD",
                MultipleChoice = true,
                TextAnswer2 = "DFW",
                TextAnswer3 = "CLT",
                TextAnswer4 = "PHX",
                CorrectAnswer = "PHX",
            },
            new()
            {
                Yevent = "t24",
                QuestionNum = 2,
                RoundNum = 1,
                MultipleChoice = false,
                CorrectAnswer = "An airplane",                   
            },
            new()
            {
                Yevent = "t24",
                QuestionNum = 3,
                RoundNum = 1,
                TextAnswer = "ORD",
                MultipleChoice = true,
                TextAnswer2 = "DFW",
                TextAnswer3 = "CLT",
                TextAnswer4 = "PHX",
                CorrectAnswer = "1234",
                MatchQuestion = true                
            },
            new()
            {
                Yevent = "t24",
                QuestionNum = 301,
                RoundNum = 3,
                TextAnswer = "",
                MultipleChoice = true,
                TextQuestion = "Artist who sang \"Eat It\" ",
                CorrectAnswer = "Weird Al Yankovic",
                MatchQuestion = false             
            },
            new()
            {
                Yevent = "t24",
                QuestionNum = 311,
                RoundNum = 3,
                TextAnswer = "",
                MultipleChoice = true,
                TextQuestion = "The pilot who successfully landed flight 209 in Chicago after food poisoning",
                CorrectAnswer = "Ted Stryker",
                MatchQuestion = false             
            }          
        ];
    private readonly DbSet<QuestionAns> mockQuestions = initialQuestions.AsQueryable().BuildMockDbSet();

    public GetQuestionHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetQuestionHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.QuestionAns.Returns(mockQuestions);

    }

    [Fact]
    public async Task Handle_GetQuestionHandler_Round1Questions()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetQuestionHandler.Handler(_contextGo);
        var request = new GetQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value!);
        Assert.Equal(3, result.Value!.Count);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_GetQuestionHandler_NotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetQuestionHandler.Handler(_contextGo);
        var request = new GetQuestionHandler.Request(){
            YEvent = "t21",
            RoundNum = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_GetQuestionHandler_BadEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetQuestionHandler.Handler(_contextGo);
        var request = new GetQuestionHandler.Request(){
            YEvent = string.Empty,
            RoundNum = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_GetQuestionHandler_BadRoundLower()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetQuestionHandler.Handler(_contextGo);
        var request = new GetQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 0
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_GetQuestionHandler_BadRoundUpper()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetQuestionHandler.Handler(_contextGo);
        var request = new GetQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 5
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_GetQuestionHandler_Round3Questions()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetQuestionHandler.Handler(_contextGo);
        var request = new GetQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 3
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value!);
        Assert.Equal(2, result.Value!.Count);
        Assert.Equal(QuestionAnswerType.Jeopardy, result.Value![0].AnswerType);
        Assert.Equal(QueryStatus.Success, result.Status);
    }
}
