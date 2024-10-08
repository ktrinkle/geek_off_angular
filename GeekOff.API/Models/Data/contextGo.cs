using System.Text.RegularExpressions;

namespace GeekOff.Data;

public partial class ContextGo : DbContext
{
    public ContextGo() { }

    public ContextGo(DbContextOptions<ContextGo> options)
        : base(options) { }

    public virtual DbSet<AdminUser> AdminUser { get; set; }
    public virtual DbSet<CurrentQuestion> CurrentQuestion { get; set; }
    public virtual DbSet<CurrentTeam> CurrentTeam { get; set; }
    public virtual DbSet<EventMaster> EventMaster { get; set; }
    public virtual DbSet<FundraisingTotal> FundraisingTotal { get; set; }
    public virtual DbSet<LogError> LogError { get; set; }
    public virtual DbSet<QuestionAns> QuestionAns { get; set; }
    public virtual DbSet<RoundCategory> RoundCategories{ get; set; }
    public virtual DbSet<Roundresult> Roundresult { get; set; }
    public virtual DbSet<Scoreposs> Scoreposs { get; set; }
    public virtual DbSet<Scoring> Scoring { get; set; }
    public virtual DbSet<Teamreference> Teamreference { get; set; }
    public virtual DbSet<UserAnswer> UserAnswer { get; set; }
    public virtual DbSet<TeamUser> TeamUser { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //automatically convert camel case to DB column names

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            //Loop through all the columns and convert from CamelCase to database_case
            //DO NOT MODIFY THIS CODE!
            foreach (var prop in entity.GetProperties())
            {
                prop.SetColumnName(TableRegex().Replace(prop.Name, "_$1").ToLower());
            }
        }

        // handle composite PKs
        modelBuilder.Entity<QuestionAns>(entity => entity.HasKey(k => new { k.Yevent, k.RoundNum, k.QuestionNum }));

        modelBuilder.Entity<Scoreposs>(entity => entity.HasKey(k => new { k.Yevent, k.RoundNum, k.QuestionNum, k.SurveyOrder }));

        modelBuilder.Entity<RoundCategory>(entity => entity.HasKey(k => new { k.Yevent, k.RoundNum, k.SubCategoryNum }));

        modelBuilder.Entity<Roundresult>(entity => entity.HasKey(k => new { k.Yevent, k.RoundNum, k.TeamNum }));

        modelBuilder.Entity<Teamreference>(entity => entity.HasKey(k => new { k.Yevent, k.TeamNum }));

        modelBuilder.Entity<UserAnswer>(entity => entity.HasKey(k => new { k.Yevent, k.RoundNum, k.TeamNum, k.QuestionNum }));

        modelBuilder.CreateEventMasterData();
        modelBuilder.CreateQuestionAnsData();
        modelBuilder.CreateScorepossData();
        modelBuilder.CreateTeamReferenceData();
        modelBuilder.CreateTeamUserData();
        modelBuilder.CreateAdminUserData();
        modelBuilder.CreateRoundCategoryData();
    }

    [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])")]
    private static partial Regex TableRegex();
}
