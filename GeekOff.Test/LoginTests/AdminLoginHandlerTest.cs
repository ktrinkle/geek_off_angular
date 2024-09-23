using System.Security.Cryptography;
using GeekOff.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GeekOff.Test.LoginTests;

public class AdminLoginHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly LoginService _loginService;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private readonly ILogger<LoginService> _logger;

    private static readonly string badPasswordTest = RandomNumberGenerator.GetString(
        choices: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789$!@#%^&",
        length: 10);

    private static readonly string goodPasswordTest = RandomNumberGenerator.GetString(
        choices: "!@#$%^&*()ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789",
        length: 12);

    private static PasswordHasher<AdminLogin> passwordHasher = new ();

    private static readonly AppSettings testSettings = new() 
    {
        Secret = "ThisIsALongSecretThatYouWouldNeverUseInARealSystemOnlyForUnitTesting",
        Salt = "SaltShakeSomeSalt",
        GeekOMaticUser = "SaltShakerOrSomethingLikeThatTest",
        Issuer = "http://localhost/",
        Audience = "http://localhost/"
    };

    private static readonly List<AdminUser> initialAdminData =
        [
            new()
            {
                Id = 1,
                Username = "admintest",
                AdminName = "Admin Name",
                Password = passwordHasher.HashPassword(
                    new AdminLogin(){
                        UserName = "admintest",
                        Password = goodPasswordTest
                    }, 
                goodPasswordTest),
                LoginTime = DateTime.Now
            },
            new()
            {
                Id = 2,
                Username = "badpasswordtest",
                AdminName = "BadPassword Name",
            }
        ];

    private readonly DbSet<AdminUser> mockAdminUser = initialAdminData.AsQueryable().BuildMockDbSet();

    public AdminLoginHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        _logger = Substitute.For<ILogger<LoginService>>();
        var _options = Options.Create(testSettings);

        _loginService = new LoginService(_logger, _contextGo, _options);
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AdminLoginHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.AdminUser.Returns(mockAdminUser);
    }

    [Fact]
    public async Task Handle_AdminLoginSuccess()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new AdminLoginHandler.Handler(_contextGo, _loginService);
        var request = new AdminLoginHandler.Request()
        {
            UserLogin = new() {
                UserName = "admintest",
                Password = goodPasswordTest
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.Value!.TeamNum);
        Assert.Equal("admintest", result.Value!.UserName);
        Assert.Equal("Admin Name", result.Value!.HumanName);
        Assert.Null(result.Value!.TeamName);
        Assert.NotNull(result.Value!.BearerToken);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    // add test for user that doesn't exist for notfound() - should this be unauthorized?

    [Fact]
    public async Task Handle_AdminLoginNotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new AdminLoginHandler.Handler(_contextGo, _loginService);
        var request = new AdminLoginHandler.Request()
        {
            UserLogin = new() {
                UserName = "notatest",
                Password = badPasswordTest
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result.Value!);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}

