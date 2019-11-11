using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssuesTracker.Models;

namespace IssuesTracker.Models
{
    interface IRepository
    {
        string addIssue(Issue issue);
        string editIssue(Issue issue);
        List<Issue_for_View> getIssues(int projectId);
        List<Project> getProjects();
        List<Issue_for_View> ViewIssues(List<Issue> issues);
        Issue_for_View getIssue(int id);
        int getProjectIdByName(string projectName);
    }
}
