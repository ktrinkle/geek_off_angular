namespace GeekOff.Test.EventStatusTests;

public class GetCurrentQuestionHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<CurrentQuestion> initialCurrentQuestion =
        [
            new()
            {
                YEvent = "t21",
                QuestionNum = 1,
                QuestionTime = DateTime.UtcNow - TimeSpan.FromMinutes(2),
                Status = 4
            },
            new()
            {
                YEvent = "t21",
                QuestionNum = 2,
                QuestionTime = DateTime.UtcNow,
                Status = 3
            },
        ];
    private readonly DbSet<CurrentQuestion> mockCurrentQuestion = initialCurrentQuestion.AsQueryable().BuildMockDbSet();

    public GetCurrentQuestionHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetCurrentQuestionHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.CurrentQuestion.Returns(mockCurrentQuestion);
    }

    [Fact]
    public async Task Handle_GetCurrentQuestion()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetCurrentQuestionHandler.Handler(_contextGo);
        var request = new GetCurrentQuestionHandler.Request()
        {
            YEvent = "t21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Value!.QuestionNum);
        Assert.Equal(3, result.Value!.Status);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_GetCurrentQuestionEmptyEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GetCurrentQuestionHandler.Handler(_contextGo);
        var request = new GetCurrentQuestionHandler.Request()
        {
            YEvent = "o21"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NoContent, result.Status);
    }
}

