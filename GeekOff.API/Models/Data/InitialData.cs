namespace GeekOff.Data;

public static class ModelBuilderExtensions
{
    public static readonly List<EventMaster> EventMaster =
    [
        new EventMaster()
        {
            Yevent = "t24",
            EventName = "Employee 2021",
            SelEvent = true
        },
        new EventMaster()
        {
            Yevent = "e21",
            EventName = "Employee 2021",
            SelEvent = true
        },
        new EventMaster()
        {
            Yevent = "e19",
            EventName = "Employee 2019"
        }
    ];

    public static readonly List<QuestionAns> QuestionAns =
    [
        new QuestionAns()
        {
            Yevent = "e21",
            QuestionNum = 201,
            RoundNum = 2,
            TextQuestion = "Name your favorite developer."
        }
    ];

    public static readonly List<Scoreposs> ScorePoss =
    [
        new Scoreposs()
        {
            Yevent = "e21",
            RoundNum = 2,
            QuestionNum = 201,
            SurveyOrder = 1,
            QuestionAnswer = "Kevin",
            Ptsposs = 8
        },
        new Scoreposs()
        {
            Yevent = "e21",
            RoundNum = 2,
            QuestionNum = 201,
            SurveyOrder = 2,
            QuestionAnswer = "Kristin",
            Ptsposs = 7
        },
        new Scoreposs()
        {
            Yevent = "e21",
            RoundNum = 2,
            QuestionNum = 201,
            SurveyOrder = 3,
            QuestionAnswer = "Diyalo",
            Ptsposs = 6
        },
        new Scoreposs()
        {
            Yevent = "e21",
            RoundNum = 2,
            QuestionNum = 201,
            SurveyOrder = 5,
            QuestionAnswer = "Li",
            Ptsposs = 2
        },
        new Scoreposs()
        {
            Yevent = "e21",
            RoundNum = 2,
            QuestionNum = 201,
            SurveyOrder = 4,
            QuestionAnswer = "Dan",
            Ptsposs = 5
        }
    ];

    public static readonly List<Teamreference> TeamReference =
    [
        new Teamreference()
        {
            Yevent = "e21",
            TeamNum = 1,
            Teamname = "Go Aggies",
            Dollarraised = 1000,
            TeamGuid = new Guid()
        }
    ];

    public static readonly List<TeamUser> TeamUser =
    [
        new TeamUser()
        {
            Id = 104,
            Yevent = "e21",
            TeamNum = 1,
            PlayerName = "Grant Hill",
            PlayerNum = 1,
            WorkgroupName = "Information Technology",
        },
        new TeamUser()
        {
            Id = 106,
            Yevent = "e21",
            TeamNum = 1,
            PlayerName = "Brandon Heath",
            PlayerNum = 2,
            WorkgroupName = "Information Technology",
        },
        new TeamUser()
        {
            Id = 107,
            Yevent = "e21",
            TeamNum = 2,
            PlayerName = "Roger Marsolek",
            PlayerNum = 1,
            WorkgroupName = "Information Technology",
        },
    ];

    public static readonly List<AdminUser> AdminUser =
    [
        new ()
        {
            Id = 100,
            Username = "362525",
            AdminName = "Kevin Trinkle",
        },
        new ()
        {
            Id = 101,
            Username = "446792",
            AdminName = "Kristin Russell",
        },
        new ()
        {
            Id = 102,
            Username = "226250",
            AdminName = "Diyalo Manral",
        },
        new AdminUser()
        {
            Id = 103,
            Username = "288132",
            AdminName = "Dan Mullings",
        },
        new AdminUser()
        {
            Id = 105,
            Username = "jaycox",
            AdminName = "Jay Cox",
        },
    ];

    public static readonly List<RoundCategory> RoundCategory =
    [
        new ()
        {
            Id = 1,
            Yevent = "e21",
            RoundNum = 3,
            CategoryName = "Potent Potables",
            SubCategoryNum = 10,
        },
        new ()
        {
            Id = 2,
            Yevent = "e21",
            RoundNum = 3,
            CategoryName = "Therapists",
            SubCategoryNum = 20,
        },
        new ()
        {
            Id = 3,
            Yevent = "e21",
            RoundNum = 3,
            CategoryName = "An Album",
            SubCategoryNum = 30,
        },
        new ()
        {
            Id = 4,
            Yevent = "e21",
            RoundNum = 3,
            CategoryName = "Potpourri",
            SubCategoryNum = 40,
        },
        new ()
        {
            Id = 5,
            Yevent = "e21",
            RoundNum = 3,
            CategoryName = "Words that begin with G",
            SubCategoryNum = 50,
        }
    ];

    public static void CreateEventMasterData(this ModelBuilder modelBuilder) => modelBuilder.Entity<EventMaster>().HasData(EventMaster.AsEnumerable());

    public static void CreateScorepossData(this ModelBuilder modelBuilder) => modelBuilder.Entity<Scoreposs>().HasData(ScorePoss.AsEnumerable());

    public static void CreateQuestionAnsData(this ModelBuilder modelBuilder) => modelBuilder.Entity<QuestionAns>().HasData(QuestionAns.AsEnumerable());

    public static void CreateTeamReferenceData(this ModelBuilder modelBuilder) => modelBuilder.Entity<Teamreference>().HasData(TeamReference.AsEnumerable());

    public static void CreateTeamUserData(this ModelBuilder modelBuilder) => modelBuilder.Entity<TeamUser>().HasData(TeamUser.AsEnumerable());

    public static void CreateAdminUserData(this ModelBuilder modelBuilder) => modelBuilder.Entity<AdminUser>().HasData(AdminUser.AsEnumerable());

    public static void CreateRoundCategoryData(this ModelBuilder modelBuilder) => modelBuilder.Entity<RoundCategory>().HasData(RoundCategory.AsEnumerable());
}

