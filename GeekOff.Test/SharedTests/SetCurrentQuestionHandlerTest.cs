namespace GeekOff.Test.SharedTests;

public class SetCurrentQuestionHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<CurrentQuestion> initialCurrentQuestion = [];
    
    private readonly DbSet<CurrentQuestion> mockCurrentQuestion = initialCurrentQuestion.AsQueryable().BuildMockDbSet();

    public SetCurrentQuestionHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(SetCurrentQuestionHandler).Assembly))
            .BuildServiceProvider();

        _contextGo.CurrentQuestion.Returns(mockCurrentQuestion);

    }

    [Fact]
    public async Task Handle_SetCurrentQuestion_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetCurrentQuestionHandler.Handler(_contextGo);
        var request = new SetCurrentQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 1,
            QuestionNum = 2,
            Status = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Value!.QuestionNum);
        Assert.Equal(1, result.Value!.Status);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_SetCurrentQuestion_BadEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetCurrentQuestionHandler.Handler(_contextGo);
        var request = new SetCurrentQuestionHandler.Request(){
            YEvent = string.Empty,
            RoundNum = 1,
            QuestionNum = 2,
            Status = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.Value!.QuestionNum);
        Assert.Equal(0, result.Value!.Status);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_SetCurrentQuestion_BadRound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetCurrentQuestionHandler.Handler(_contextGo);
        var request = new SetCurrentQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 4,
            QuestionNum = 2,
            Status = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.Value!.QuestionNum);
        Assert.Equal(0, result.Value!.Status);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SetCurrentQuestion_BadQuestionNumRoundOne()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetCurrentQuestionHandler.Handler(_contextGo);
        var request = new SetCurrentQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 1,
            QuestionNum = 101,
            Status = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.Value!.QuestionNum);
        Assert.Equal(0, result.Value!.Status);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SetCurrentQuestion_BadQuestionNumRoundTwo()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetCurrentQuestionHandler.Handler(_contextGo);
        var request = new SetCurrentQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 2,
            QuestionNum = 50,
            Status = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.Value!.QuestionNum);
        Assert.Equal(0, result.Value!.Status);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SetCurrentQuestion_BadQuestionNumRoundThree()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetCurrentQuestionHandler.Handler(_contextGo);
        var request = new SetCurrentQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 3,
            QuestionNum = 101,
            Status = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.Value!.QuestionNum);
        Assert.Equal(0, result.Value!.Status);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SetCurrentQuestion_BadStatusLower()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetCurrentQuestionHandler.Handler(_contextGo);
        var request = new SetCurrentQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 1,
            QuestionNum = 1,
            Status = -1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.Value!.QuestionNum);
        Assert.Equal(0, result.Value!.Status);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_SetCurrentQuestion_BadStatusUpper()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetCurrentQuestionHandler.Handler(_contextGo);
        var request = new SetCurrentQuestionHandler.Request(){
            YEvent = "t24",
            RoundNum = 1,
            QuestionNum = 1,
            Status = 5
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.Value!.QuestionNum);
        Assert.Equal(0, result.Value!.Status);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }
}
