using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IssuesTracker.Models
{
    public class myDBContext : IdentityDbContext<AppUser>
    {
        public myDBContext() : base("DefaultConnection") { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
    }
}