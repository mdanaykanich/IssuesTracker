using System.Collections.Generic;

namespace IssuesTracker.Models
{
    interface IRepository
    {
        string addIssue(Issue issue);
        string addUser(AppUser user);
        string addUserToProject(int projectId, string email);
        Project addProject(Project project);
        string editIssue(Issue issue);
        string changeType(int id, string type);
        List<Issue_for_View> getIssues(int projectId);
        List<Project> getProjects();
        List<Issue_for_View> ViewIssues(List<Issue> issues);
        Issue_for_View getIssue(int id);
        bool isValidUser(string email, string password);
        bool checkUserByEmail(string email);
        List<User_for_View> getUsers();
        List<string> getRoleNames();
        string getUserRoleName(string email);
        int getProjectIdByName(string projectName);
        List<User_for_View> getUsersByProjectId(int projectId);
        string getEmailByUsername(string username);
        string getUsernameByEmail(string email);
        string updateUser(string email, string newUsername, string newPassword);
    }
}
