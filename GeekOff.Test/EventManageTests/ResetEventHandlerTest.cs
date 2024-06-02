using GeekOff.Models;

namespace GeekOff.Test.EventManageTests;

public class ResetEventHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<CurrentQuestion> initialCurrentQuestion =
        [
            new()
            {
                Id = 7,
                YEvent = "t21",
                QuestionTime = DateTime.UtcNow,
                QuestionNum = 1,
                Status = 1
            }
        ];
    
    private static readonly List<Roundresult> initialRoundresult =
        [
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 1,
                Ptswithbonus = 76,
                Rnk = 2
            }
        ];

    private static readonly List<Scoring> initialScoring =
        [
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 1,
                TeamAnswer = "Here",
                PlayerNum = 1,
                PointAmt = 50
            }
        ];

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
            }
        ];

    private readonly DbSet<CurrentQuestion> mockCurrentQuestion = initialCurrentQuestion.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Roundresult> mockRoundresult = initialRoundresult.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Scoring> mockScoring = initialScoring.AsQueryable().BuildMockDbSet();
    private readonly DbSet<UserAnswer> mockUserAnswer = initialUserAnswer.AsQueryable().BuildMockDbSet();

    public ResetEventHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ResetEventHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.CurrentQuestion.Returns(mockCurrentQuestion);
        _contextGo.Roundresult.Returns(mockRoundresult);
        _contextGo.Scoring.Returns(mockScoring);
        _contextGo.UserAnswer.Returns(mockUserAnswer);

    }

    [Fact]
    public async Task Handle_ResetEventMaster()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new ResetEventHandler.Handler(_contextGo);
        var request = new ResetEventHandler.Request(){
            YEvent = "t21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("Event t21 results were removed from the system.", result.Value.Message!);
        Assert.Equal(QueryStatus.Success, result.Status);
    }
}