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
        string addUser(AppUser user);
        string editIssue(Issue issue);
        string changeType(int id, string type);
        List<Issue_for_View> getIssues(int projectId);
        List<Project> getProjects();
        List<Issue_for_View> ViewIssues(List<Issue> issues);
        Issue_for_View getIssue(int id);
        bool isValidUser(string email, string password);
        bool checkUserByEmail(string email);
        int getProjectIdByName(string projectName);
    }
}
