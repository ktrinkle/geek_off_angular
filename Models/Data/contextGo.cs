using System;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GeekOff.Data
{
    public partial class contextGo : DbContext
    {
        public contextGo() { }

        public contextGo(DbContextOptions<contextGo> options)
            : base(options) { }

        public virtual DbSet<CurrentTeam> CurrentTeam { get; set; }
        public virtual DbSet<EventMaster> EventMaster { get; set; }
        public virtual DbSet<FundraisingTotal> FundraisingTotal { get; set; }
        public virtual DbSet<QuestionAns> QuestionAns { get; set; }
        public virtual DbSet<Round1score> Round1Score { get; set; }
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
                    prop.SetColumnName(Regex.Replace(prop.Name, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", "_$1").ToLower());
                }
            }

            // handle composite PK
            modelBuilder.Entity<QuestionAns>(entity => 
            {
                entity.HasKey(k => new {k.Yevent, k.RoundNo, k.QuestionNo});
            });

            modelBuilder.Entity<Scoreposs>(entity => 
            {
                entity.HasKey(k => new {k.Yevent, k.RoundNo, k.QuestionNo, k.SurveyOrder});
            });

            modelBuilder.Entity<Roundresult>(entity => 
            {
                entity.HasKey(k => new {k.Yevent, k.RoundNo, k.TeamNo});
            });

            modelBuilder.Entity<Scoring>(entity => 
            {
                entity.HasKey(k => new {k.Yevent, k.RoundNo, k.TeamNo, k.QuestionNo});
            });

            modelBuilder.Entity<Teamreference>(entity => 
            {
                entity.HasKey(k => new {k.Yevent, k.TeamNo});
            });

            modelBuilder.Entity<UserAnswer>(entity => 
            {
                entity.HasKey(k => new {k.Yevent, k.RoundNo, k.TeamNo, k.QuestionNo});
            });

            modelBuilder.CreateEventMasterData();
            modelBuilder.CreateQuestionAnsData();
            modelBuilder.CreateScorepossData();

        }
    }
}