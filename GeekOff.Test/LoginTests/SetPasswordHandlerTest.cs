using GeekOff.Services;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace GeekOff.Test.LoginTests;

public class SetPasswordHandlerTest
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

    public SetPasswordHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(SetPasswordHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.AdminUser.Returns(mockAdminUser);
    }

    [Fact]
    public async Task Handle_SetPasswordSuccess()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetPasswordHandler.Handler(_contextGo);
        var request = new SetPasswordHandler.Request()
        {
            UserName = "admintest",
            Password = newPasswordTest
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.Success, result.Status);
        Assert.Equal("The password was successfully saved.", result.Value.Message);
    }

    [Fact]
    public async Task Handle_SetPasswordNotFound()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new SetPasswordHandler.Handler(_contextGo);
        var request = new SetPasswordHandler.Request()
        {
            UserName = "notatest",
            Password = oldPasswordTest
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }
}

