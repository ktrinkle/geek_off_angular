using GeekOff.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GeekOff.Test.EventStatusTests;

public class PlayerLoginHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly LoginService _loginService;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private readonly ILogger<LoginService> _logger;

    private static readonly AppSettings testSettings = new() 
    {
        Secret = "ThisIsALongSecretThatYouWouldNeverUseInARealSystemOnlyForUnitTesting",
        Salt = "SaltShakeSomeSalt",
        GeekOMaticUser = "SaltShakerOrSomethingLikeThatTest",
        Issuer = "http://localhost/",
        Audience = "http://localhost/"
    };

    private static readonly List<Teamreference> initialTeamData =
        [
            new()
            {
                Yevent = "t21",
                Teamname = "Team 1",
                TeamNum = 1,
                Dollarraised = null,
                TeamGuid = Guid.NewGuid()
            }
        ];
    private readonly DbSet<Teamreference> mockTeamReference = initialTeamData.AsQueryable().BuildMockDbSet();

    public PlayerLoginHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        _logger = Substitute.For<ILogger<LoginService>>();
        var _options = Options.Create(testSettings);

        _loginService = new LoginService(_logger, _contextGo, _options);
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(PlayerLoginHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Teamreference.Returns(mockTeamReference);
    }

    [Fact]
    public async Task Handle_PlayerLoginSuccess()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new PlayerLoginHandler.Handler(_contextGo, _loginService);
        var request = new PlayerLoginHandler.Request()
        {
            YEvent = "t21",
            TeamGuid = initialTeamData[0].TeamGuid
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.Value!.TeamNum);
        Assert.Equal("Team 1", result.Value!.TeamName);
        Assert.NotNull(result.Value!.BearerToken);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    // add test for user that doesn't exist for notfound() - should this be unauthorized?

    [Fact]
    public async Task Handle_PlayerLoginNotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new PlayerLoginHandler.Handler(_contextGo, _loginService);
        var request = new PlayerLoginHandler.Request()
        {
            YEvent = "t21",
            TeamGuid = Guid.NewGuid()
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result.Value!);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}

