namespace GeekOff.Test.RoundTwoFeudTests;

public class RoundTwoSurveyQuestionHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<Scoreposs> initialScoreposs = 
        [
            new ()
            {
                Yevent = "t24",
                RoundNum = 2,
                QuestionNum = 201,
                QuestionAnswer = "DFW",
                SurveyOrder = 1,
                Ptsposs = 16
            },
            new ()
            {
                Yevent = "t24",
                RoundNum = 2,
                QuestionNum = 201,
                QuestionAnswer = "SAT",
                SurveyOrder = 2,
                Ptsposs = 10
            },
            new ()
            {
                Yevent = "t24",
                RoundNum = 2,
                QuestionNum = 202,
                QuestionAnswer = "Brother Boy",
                SurveyOrder = 1,
                Ptsposs = 30
            },
            new ()
            {
                Yevent = "t24",
                RoundNum = 2,
                QuestionNum = 202,
                QuestionAnswer = "Lavonda",
                SurveyOrder = 2,
                Ptsposs = 15
            }
        ];

    private static readonly List<QuestionAns> initialQuestions =
        [
            new()
            {
                Yevent = "t24",
                QuestionNum = 201,
                TextQuestion = "Name an airport",
                RoundNum = 2,
                CorrectAnswer = "DFW",
            },
            new()
            {
                Yevent = "t24",
                QuestionNum = 202,
                TextQuestion = "Name your favorite Sordid Lives character",
                RoundNum = 2,
                CorrectAnswer = "DFW",
            }         
        ];
    private readonly DbSet<QuestionAns> mockQuestions = initialQuestions.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Scoreposs> mockScoreposs = initialScoreposs.AsQueryable().BuildMockDbSet();

    public RoundTwoSurveyQuestionHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundTwoSurveyQuestionHandler).Assembly))
            .BuildServiceProvider();

        _contextGo.QuestionAns.Returns(mockQuestions);
        _contextGo.Scoreposs.Returns(mockScoreposs);

    }

    [Fact]
    public async Task Handle_RoundTwoSurveyQuestionHandler_Questions()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoSurveyQuestionHandler.Handler(_contextGo);
        var request = new RoundTwoSurveyQuestionHandler.Request(){
            YEvent = "t24"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value!);
        Assert.Equal(2, result.Value!.Count);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundTwoSurveyQuestionHandler_NotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundTwoSurveyQuestionHandler.Handler(_contextGo);
        var request = new RoundTwoSurveyQuestionHandler.Request(){
            YEvent = "t21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}
