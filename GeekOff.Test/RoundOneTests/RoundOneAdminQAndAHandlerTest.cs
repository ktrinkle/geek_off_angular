namespace GeekOff.Test.RoundOneTests;

public class RoundOneAdminQAndAHandlerTest
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

    public RoundOneAdminQAndAHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(RoundOneAdminQAndAHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.QuestionAns.Returns(mockQuestions);

    }

    [Fact]
    public async Task Handle_RoundOneAdminQAndAHandler_Questions()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundOneAdminQAndAHandler.Handler(_contextGo);
        var request = new RoundOneAdminQAndAHandler.Request(){
            YEvent = "t24"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value!);
        Assert.Equal(3, result.Value!.Count);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_RoundOnePlayerQAndAHandler_NotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new RoundOneAdminQAndAHandler.Handler(_contextGo);
        var request = new RoundOneAdminQAndAHandler.Request(){
            YEvent = "t21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}
