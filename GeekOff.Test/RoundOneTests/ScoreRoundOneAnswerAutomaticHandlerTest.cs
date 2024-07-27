namespace GeekOff.Test.RoundOneTests;

public class ScoreRoundOneAnswerAutomaticHandlerTest
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
            }         
        ];

    private static readonly List<Scoreposs> initialScoreposs =
        [
            new()
            {
                Yevent = "t24",
                QuestionNum = 1,
                RoundNum = 1,
                Ptsposs = 10
            }
        ];

    private static readonly List<UserAnswer> initialUserAnswer = 
        [
            new()
            {
                Yevent = "t24",
                TeamNum = 1,
                QuestionNum = 1,
                RoundNum = 1,
                TextAnswer = "PHX",
                AnswerTime = DateTime.UtcNow
            },
            new()
            {
                Yevent = "t24",
                TeamNum = 2,
                QuestionNum = 1,
                RoundNum = 1,
                TextAnswer = "ORD",
                AnswerTime = DateTime.UtcNow.AddSeconds(-20)
            },
            new()
            {
                Yevent = "t24",
                TeamNum = 3,
                QuestionNum = 1,
                RoundNum = 1,
                TextAnswer = "PHX",
                AnswerTime = DateTime.UtcNow.AddSeconds(-15)
            },
            
        ];

    private static readonly List<Scoring> initialScoring =
    [
        new()
        {
            Yevent = "t24",
            TeamAnswer = "CLT",
            TeamNum = 1,
            QuestionNum = 1,
            PointAmt = 0
        }
    ];
    private readonly DbSet<QuestionAns> mockQuestions = initialQuestions.AsQueryable().BuildMockDbSet();
    private readonly DbSet<UserAnswer> mockUserAnswer = initialUserAnswer.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Scoreposs> mockScoreposs = initialScoreposs.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Scoring> mockScoring = initialScoring.AsQueryable().BuildMockDbSet();

    public ScoreRoundOneAnswerAutomaticHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ScoreRoundOneAnswerAutomaticHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.QuestionAns.Returns(mockQuestions);
        _contextGo.Scoreposs.Returns(mockScoreposs);
        _contextGo.UserAnswer.Returns(mockUserAnswer);
        _contextGo.Scoring.Returns(mockScoring);

    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerAutomatic_Normal()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new ScoreRoundOneAnswerAutomaticHandler.Handler(_contextGo);
        var request = new ScoreRoundOneAnswerAutomaticHandler.Request(){
            YEvent = "t24",
            QuestionId = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Auto-scoring complete.", result.Value.Message);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerAutomatic_EmptyEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new ScoreRoundOneAnswerAutomaticHandler.Handler(_contextGo);
        var request = new ScoreRoundOneAnswerAutomaticHandler.Request(){
            YEvent = String.Empty,
            QuestionId = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Invalid event.", result.Value.Message);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerAutomatic_BadQuestionNum()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new ScoreRoundOneAnswerAutomaticHandler.Handler(_contextGo);
        var request = new ScoreRoundOneAnswerAutomaticHandler.Request(){
            YEvent = "t24",
            QuestionId = -1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Invalid question.", result.Value.Message);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_ScoreRoundOneAnswerAutomatic_QuestionNotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new ScoreRoundOneAnswerAutomaticHandler.Handler(_contextGo);
        var request = new ScoreRoundOneAnswerAutomaticHandler.Request(){
            YEvent = "t24",
            QuestionId = 2
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Unable to load question.", result.Value.Message);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}
