using Microsoft.AspNet.Identity.EntityFramework;

namespace IssuesTracker.Models
{
    public class AppUser : IdentityUser
    {
        public int ProjectId { get; set; }
    }
}