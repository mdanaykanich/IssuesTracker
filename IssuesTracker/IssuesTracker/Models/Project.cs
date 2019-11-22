using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IssuesTracker.Models
{
    public class Project
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Issue> Issues { get; set; }

    }
}