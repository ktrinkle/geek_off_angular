namespace GeekOff.Test.RoundOneTests;

public class RoundOneSingleQAndAHandlerTest
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
            }            
        ];
    private readonly DbSet<QuestionAns> mockQuestions = initialQuestions.AsQueryable().BuildMockDbSet();

    public RoundOneSingleQAndAHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundOneSingleQAndAHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.QuestionAns.Returns(mockQuestions);

    }

    [Fact]
    public async Task Handle_RoundOneSingleQAndAHandler_MultipleChoiceQuestion()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundOneSingleQAndAHandler.Handler(_contextGo);
        var request = new RoundOneSingleQAndAHandler.Request(){
            YEvent = "t24",
            QuestionId = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.Value!.QuestionNum);
        Assert.Equal(4, result.Value!.Answers.Count);
        Assert.Equal(QuestionAnswerType.MultipleChoice, result.Value!.AnswerType);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundOneSingleQAndAHandler_FreeTextQuestion()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundOneSingleQAndAHandler.Handler(_contextGo);
        var request = new RoundOneSingleQAndAHandler.Request(){
            YEvent = "t24",
            QuestionId = 2
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Value!.QuestionNum);
        Assert.Equal(QuestionAnswerType.FreeText, result.Value!.AnswerType);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundOneSingleQAndAHandler_MatchQuestion()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundOneSingleQAndAHandler.Handler(_contextGo);
        var request = new RoundOneSingleQAndAHandler.Request(){
            YEvent = "t24",
            QuestionId = 3
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Value!.QuestionNum);
        Assert.Equal(4, result.Value!.Answers.Count);
        Assert.Equal(QuestionAnswerType.Match, result.Value!.AnswerType);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundOneSingleQAndAHandler_EventNotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundOneSingleQAndAHandler.Handler(_contextGo);
        var request = new RoundOneSingleQAndAHandler.Request(){
            YEvent = "t21",
            QuestionId = 3
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_RoundOneSingleQAndAHandler_QuestionNotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundOneSingleQAndAHandler.Handler(_contextGo);
        var request = new RoundOneSingleQAndAHandler.Request(){
            YEvent = "t24",
            QuestionId = 4
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}
