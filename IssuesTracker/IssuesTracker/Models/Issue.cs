using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssuesTracker.Models
{
    public enum Priority { Trivial, Low, Medium, High, Critical}
    public enum Type {  New, InProgress, Done}
    public class Issue
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Assignee { get; set; }
        public Priority Priority { get; set; }
        public Type Type { get; set; }
    }
}