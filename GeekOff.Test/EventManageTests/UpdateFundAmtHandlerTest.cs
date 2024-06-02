using GeekOff.Models;

namespace GeekOff.Test.EventManageTests;

public class UpdateFundAmtHandlerTest
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

    public UpdateFundAmtHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UpdateFundAmtHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Teamreference.Returns(mock);

    }

    [Fact]
    public async Task Handle_UpdateFundAmtMaster()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new UpdateFundAmtHandler.Handler(_contextGo);
        var request = new UpdateFundAmtHandler.Request(){
            YEvent = "t21",
            TeamNum = 1,
            DollarAmt = 50
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("The dollar amount is successfully updated.", result.Value.Message!);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_UpdateFundAmtBadAmt()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new UpdateFundAmtHandler.Handler(_contextGo);
        var request = new UpdateFundAmtHandler.Request(){
            YEvent = "t21",
            TeamNum = 1,
            DollarAmt = -50
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("You can't have a fundraising amount less than zero.", result.Value.Message!);
        Assert.Equal(QueryStatus.BadRequest, result.Status);
    }

    [Fact]
    public async Task Handle_UpdateFundAmtBadTeam()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new UpdateFundAmtHandler.Handler(_contextGo);
        var request = new UpdateFundAmtHandler.Request(){
            YEvent = "t21",
            TeamNum = 3,
            DollarAmt = 15
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("Invalid team number is entered.", result.Value.Message!);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_UpdateFundAmtNullAmt()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new UpdateFundAmtHandler.Handler(_contextGo);
        var request = new UpdateFundAmtHandler.Request(){
            YEvent = "t21",
            TeamNum = 1,
            DollarAmt = null
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("The dollar amount is successfully updated.", result.Value.Message!);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

}