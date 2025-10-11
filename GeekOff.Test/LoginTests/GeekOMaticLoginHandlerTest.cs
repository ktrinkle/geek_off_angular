using System.Security.Cryptography;
using System.Text;
using GeekOff.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GeekOff.Test.LoginTests;

public class GeekOMaticLoginHandlerTest
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
        GeekOMaticUser = Guid.NewGuid().ToString(),
        Issuer = "http://localhost/",
        Audience = "http://localhost/"
    };

    private static readonly string geekOMaticToken = SHA512.HashData(
        Encoding.UTF8.GetBytes(testSettings.GeekOMaticUser))
        .Aggregate("", (current, x) => current + $"{x:x2}");

    public GeekOMaticLoginHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        _logger = Substitute.For<ILogger<LoginService>>();
        var _options = Options.Create(testSettings);

        _loginService = new LoginService(_logger, _contextGo, _options);
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GeekOMaticLoginHandler).Assembly))
            .AddLogging().BuildServiceProvider();
    }

    [Fact]
    public async Task Handle_GeekOMaticLoginSuccess()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GeekOMaticLoginHandler.Handler(_loginService);
        var request = new GeekOMaticLoginHandler.Request()
        {
            Token = geekOMaticToken
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.Value!.TeamNum);
        Assert.Equal("GeekOMatic", result.Value!.UserName);
        Assert.Equal("GeekOMatic", result.Value!.HumanName);
        Assert.NotNull(result.Value!.BearerToken);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_GeekOMaticLoginBadToken()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GeekOMaticLoginHandler.Handler(_loginService);
        var request = new GeekOMaticLoginHandler.Request()
        {
            Token = "grtYifosnfkvd0FDYFUDISNDIKSMFDS"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result.Value!);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_GeekOMaticLoginNoToken()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new GeekOMaticLoginHandler.Handler(_loginService);
        var request = new GeekOMaticLoginHandler.Request()
        {
            Token = string.Empty
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result.Value!);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }
}

