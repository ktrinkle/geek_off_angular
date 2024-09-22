using GeekOff.Services;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace GeekOff.Test.LoginTests;

public class CreateAdminUserHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;

    private static readonly string oldPasswordTest = RandomNumberGenerator.GetString(
        choices: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789$!@#%^&",
        length: 10);

    private static readonly string newPasswordTest = RandomNumberGenerator.GetString(
        choices: "!@#$%^&*()ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789",
        length: 12);

    private static readonly List<AdminUser> initialAdminData =
        [
            new()
            {
                Id = 1,
                Username = "admintest",
                AdminName = "Admin Name",
                Password = oldPasswordTest,
                LoginTime = DateTime.Now
            }
        ];

    private readonly DbSet<AdminUser> mockAdminUser = initialAdminData.AsQueryable().BuildMockDbSet();

    public CreateAdminUserHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateAdminUserHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.AdminUser.Returns(mockAdminUser);
    }

    [Fact]
    public async Task Handle_CreateAdminUser_Success()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new CreateAdminUserHandler.Handler(_contextGo);
        var request = new CreateAdminUserHandler.Request()
        {
            UserName = "newadminuser",
            AdminName = "New Admin User",
            Password = newPasswordTest
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.Success, result.Status);
        Assert.Equal("The user was successfully created.", result.Value.Message);
    }

    [Fact]
    public async Task Handle_CreateAdminUser_NoPassword()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new CreateAdminUserHandler.Handler(_contextGo);
        var request = new CreateAdminUserHandler.Request()
        {
            UserName = "notatest",
            Password = ""
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_CreateAdminUser_DupeUser()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new CreateAdminUserHandler.Handler(_contextGo);
        var request = new CreateAdminUserHandler.Request()
        {
            UserName = "admintest",
            AdminName = "Second Admin name",
            Password = oldPasswordTest
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.Conflict, result.Status);
    }
}

