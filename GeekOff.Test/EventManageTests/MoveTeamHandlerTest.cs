namespace GeekOff.Test.EventManageTests;

public class MoveTeamHandlerTest
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
                TeamGuid = Guid.NewGuid()
            },
            new()
            {
                Yevent = "t21",
                Teamname = "Team 3",
                TeamNum = 3,
                Dollarraised = 100,
                TeamGuid = Guid.NewGuid()                
            },
            new()
            {
                Yevent = "t21",
                Teamname = "Team 4",
                TeamNum = 4,
                Dollarraised = 101,
                TeamGuid = Guid.NewGuid()                
            }
        ];
    private static readonly List<TeamUser> initialTeamUser = 
    [
        new()
        {
            Id = 1,
            Yevent = "t21",
            TeamNum = 1,
            BadgeId = "123456",
            Username = "bberman",
            PlayerName = "Bob Berman",
            WorkgroupName = "ACS",
            PlayerNum = 1,
            LoginTime = DateTime.Now,
            SessionId = Guid.NewGuid()
        },
        new()
        {
            Id = 2,
            Yevent = "t21",
            TeamNum = 3,
            BadgeId = "123457",
            Username = "bberman2",
            PlayerName = "Bill Berman",
            WorkgroupName = "Corporate",
            PlayerNum = 1,
            LoginTime = DateTime.Now,
            SessionId = Guid.NewGuid()
        },
        new()
        {
            Id = 3,
            Yevent = "t21",
            TeamNum = 4,
            BadgeId = "123458",
            Username = "cberman",
            PlayerName = "Carol Berman",
            WorkgroupName = "Pilot",
            PlayerNum = 1,
            LoginTime = DateTime.Now,
            SessionId = Guid.NewGuid()
        }
    ];
    private static readonly List<Roundresult> initialRoundresult =
        [
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 1,
                Ptswithbonus = 76,
                Rnk = 2
            },
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 3,
                Ptswithbonus = 95,
                Rnk = 1
            },
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 4,
                Ptswithbonus = 23,
                Rnk = 3
            }
        ];

    private static readonly List<Scoring> initialScoring =
        [
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 1,
                TeamAnswer = "Here",
                PlayerNum = 1,
                PointAmt = 50
            },
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 3,
                TeamAnswer = "Here",
                PlayerNum = 1,
                PointAmt = 75
            },
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 4,
                TeamAnswer = "There",
                PlayerNum = 1,
                PointAmt = 22
            }
        ];

    private static readonly List<UserAnswer> initialUserAnswer =
        [
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 1,
                QuestionNum  = 1,
                TextAnswer = "Help",
                AnswerUser = "2",
                AnswerTime = DateTime.UtcNow
            },
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 3,
                QuestionNum  = 1,
                TextAnswer = "Help",
                AnswerUser = "3",
                AnswerTime = DateTime.UtcNow
            },
            new()
            {
                Yevent = "t21",
                RoundNum = 1,
                TeamNum = 4,
                QuestionNum  = 1,
                TextAnswer = "Answer",
                AnswerUser = "4",
                AnswerTime = DateTime.UtcNow
            }
        ];
    private readonly DbSet<Teamreference> mockTeamReference = initialTeamData.AsQueryable().BuildMockDbSet();
    private readonly DbSet<TeamUser> mockTeamUser = initialTeamUser.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Roundresult> mockRoundresult = initialRoundresult.AsQueryable().BuildMockDbSet();
    private readonly DbSet<Scoring> mockScoring = initialScoring.AsQueryable().BuildMockDbSet();
    private readonly DbSet<UserAnswer> mockUserAnswer = initialUserAnswer.AsQueryable().BuildMockDbSet();

    public MoveTeamHandlerTest()
    {
        _contextGo = Substitute.For<ContextGo>();
        
        _serviceProvider = _services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MoveTeamHandler).Assembly))
            .AddLogging().BuildServiceProvider();

        _contextGo.Teamreference.Returns(mockTeamReference);
        _contextGo.TeamUser.Returns(mockTeamUser);
        _contextGo.Roundresult.Returns(mockRoundresult);
        _contextGo.Scoring.Returns(mockScoring);
        _contextGo.UserAnswer.Returns(mockUserAnswer);
    }

    [Fact]
    public async Task Handle_MoveTeamMaster()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new MoveTeamHandler.Handler(_contextGo);
        var request = new MoveTeamHandler.Request(){
            YEvent = "t21",
            OldTeamNum = 3,
            NewTeamNum = 2
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("The team was successfully moved to the new number.", result.Value.Message!);
        Assert.Equal(QueryStatus.Success, result.Status);
    }

    [Fact]
    public async Task Handle_MoveTeamNoTeamId()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new MoveTeamHandler.Handler(_contextGo);
        var request = new MoveTeamHandler.Request(){
            YEvent = "t21",
            OldTeamNum = 5,
            NewTeamNum = 6
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("The original team number does not exist.", result.Value.Message!);
        Assert.Equal(QueryStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_MoveTeamToExistingTeam()
    {
        // Arrange
        _ = _serviceProvider.GetRequiredService<IMediator>();

        var handler = new MoveTeamHandler.Handler(_contextGo);
        var request = new MoveTeamHandler.Request(){
            YEvent = "t21",
            OldTeamNum = 4,
            NewTeamNum = 1
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.Value.Message!);
        Assert.Equal("The new team number already exists.", result.Value.Message!);
        Assert.Equal(QueryStatus.Conflict, result.Status);
    }
}