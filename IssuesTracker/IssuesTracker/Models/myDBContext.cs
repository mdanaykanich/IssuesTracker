using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IssuesTracker.Models
{
    public class myDBContext : IdentityDbContext<AppUser>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public myDBContext() : base("DefaultConnection") { }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            Database.SetInitializer<myDBContext>(null);
            base.OnModelCreating(builder);
        }
    }
}