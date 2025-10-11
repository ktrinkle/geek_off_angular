namespace GeekOff.Test.RoundOneTests;

public class RoundOneEnteredAnswersHandlerTest
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
                PointAmt = 0
            },     
        ];
    private static readonly List<UserAnswer> initialUserAnswer =
        [
            new ()
            {
                Yevent = "t24",
                TeamNum = 1,
                QuestionNum = 1,
                RoundNum = 1,
                TextAnswer = "Testing",
                AnswerTime = DateTime.UtcNow
            },
            new ()
            {
                Yevent = "t24",
                TeamNum = 2,
                QuestionNum = 1,
                RoundNum = 1,
                TextAnswer = "Testing 2",
                AnswerTime = DateTime.UtcNow
            },
            new ()
            {
                Yevent = "t24",
                TeamNum = 3,
                QuestionNum = 1,
                RoundNum = 1,
                TextAnswer = "Testing 3",
                AnswerTime = DateTime.UtcNow
            }
        ];
    private readonly DbSet<Scoring> mockScoring = initialScoring.AsQueryable().BuildMockDbSet();
    private readonly DbSet<UserAnswer> mockUserAnswer = initialUserAnswer.AsQueryable().BuildMockDbSet();

    public RoundOneEnteredAnswersHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundOneEnteredAnswersHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Scoring.Returns(mockScoring);
        _contextGo.UserAnswer.Returns(mockUserAnswer);
    }

    [Fact]
    public async Task Handle_RoundOneAdminQAndAHandler_Questions()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundOneEnteredAnswersHandler.Handler(_contextGo);
        var request = new RoundOneEnteredAnswersHandler.Request(){
            YEvent = "t24",
            QuestionNum = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value!);
        Assert.Equal(3, result.Value!.Count);
        Assert.Equal(QueryStatus.Success, result.Status);
    }
}
