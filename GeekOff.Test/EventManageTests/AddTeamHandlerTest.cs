using GeekOff.Models;

namespace GeekOff.Test.EventManageTests;

public class AddTeamHandlerTest
{
    private readonly ContextGo _contextGo;
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly ServiceProvider _serviceProvider;
    private static readonly List<Teamreference> initialTeamData =
        [
            new()
            {
                Yevent = "t21",
                Teamname = "Team 1",
                TeamNum = 1,
                Dollarraised = null,
                TeamGuid = new Guid()
            },
            new()
            {
                Yevent = "t21",
                Teamname = "Team 2",
                TeamNum = 2,
                Dollarraised = 100,
                TeamGuid = new Guid()                
            }
        ];
    private readonly DbSet<Teamreference> mock = initialTeamData.AsQueryable().BuildMockDbSet();

    public AddTeamHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddTeamHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Teamreference.Returns(mock);

    }

    [Fact]
    public async Task Handle_AddTeamIncrementNumber()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new AddTeamHandler.Handler(_contextGo);
        var request = new AddTeamHandler.Request(){
            YEvent = "t21",
            TeamName = "Test 2024"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Value!.TeamNum);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_AddTeamNewEvent()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new AddTeamHandler.Handler(_contextGo);
        var request = new AddTeamHandler.Request(){
            YEvent = "t22",
            TeamName = "Test 2022"
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.Value!.TeamNum);
        Assert.Equal(QueryStatus.Success, result.Status);
    }
}