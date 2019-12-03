using System.Data.Entity.Migrations;

namespace IssuesTracker.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<Models.myDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "IssuesTracker.Models.myDBContext";
        }
    }
}
