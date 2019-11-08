using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssuesTracker.Models
{
    public class Issues_for_Kanban
    {
        public List<Issue_for_View> newIssues { get; set; }
        public List<Issue_for_View> inprogressIssues { get; set; }
        public List<Issue_for_View> doneIssues { get; set; }

        public Issues_for_Kanban()
        {
            newIssues = new List<Issue_for_View>();
            inprogressIssues = new List<Issue_for_View>();
            doneIssues = new List<Issue_for_View>();
        }
    }
}