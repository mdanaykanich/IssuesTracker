using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace IssuesTracker.Models
{
    public class myDBContext: DbContext
    {
        public myDBContext(): base("DefaultConnection") { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
    }
}