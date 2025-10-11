namespace GeekOff.Test.RoundThreeTests;

public class RoundThreeTeamAnswerHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    

    private readonly DbSet<Scoring> mockScoring = new List<Scoring>().AsQueryable().BuildMockDbSet();
   
    public RoundThreeTeamAnswerHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundThreeTeamAnswerHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Scoring.Returns(mockScoring);

    }

    [Fact]
    public async Task Handle_RoundThreeAnswerHandler_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeTeamAnswerHandler.Handler(_contextGo);
        var request = new RoundThreeTeamAnswerHandler.Request(){
            Round3Answers = 
                [
                    new ()
                    {
                        YEvent = "t24",
                        QuestionNum = 301,
                        TeamNum = 1,
                        Score = -100
                    },
                    new ()
                    {
                        YEvent = "t24",
                        QuestionNum = 301,
                        TeamNum = 3,
                        Score = 100
                    },  
                    new ()
                    {
                        YEvent = "t24",
                        QuestionNum = 301,
                        TeamNum = 5,
                        Score = 0
                    },                  
                ]
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Scores were added to the system.", result.Value.Message);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundThreeAnswerHandler_NoData()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundThreeTeamAnswerHandler.Handler(_contextGo);
        var request = new RoundThreeTeamAnswerHandler.Request(){
            Round3Answers = []
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }
}
