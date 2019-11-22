using System.Collections.Generic;

namespace IssuesTracker.Models
{
    public class Issue_for_View
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Assignee { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public List<string> Types { get; set; }
        public List<string> Priorities { get; set; }

    }
}