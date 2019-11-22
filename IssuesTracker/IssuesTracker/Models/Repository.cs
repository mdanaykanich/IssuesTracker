using System;
using System.Collections.Generic;
using System.Linq;

namespace IssuesTracker.Models
{
    public class Repository : IRepository
    {
        myDBContext db = new myDBContext();

        public string addIssue(Issue issue)
        {
            try
            {
                db.Issues.Add(issue);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return "Error. Wrong data entry";
            }
            return "Successfully added";
        }

        public List<User_for_View> getUsersByProjectId(int projectId)
        {
            List<AppUser> users = db.Users.Where(u => u.ProjectId == projectId).ToList();
            List<User_for_View> result = new List<User_for_View>();

            foreach (AppUser user in users)
            {
                result.Add(new User_for_View() { User = user, Role = getUserRoleName(user.Email) });
            }
            return result;
        }

        public string addUser(AppUser user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return "Successfully added";
        }

        public bool checkUserByEmail(string email)
        {
            return db.Users.Any(u => u.Email == email);
        }

        public bool isValidUser(string email, string password)
        {
            return db.Users.Any(u => u.Email == email && u.PasswordHash == password);
        }

        public string changeType(int id, string type)
        {
            var issue = db.Issues.Where(i => i.Id == id).First();
            if (issue != null)
            {
                issue.Type = (Type)Enum.Parse(typeof(Models.Type), type, true);
                db.SaveChanges();
                return "Successfully changed";
            }
            return "Error";
        }

        public string editIssue(Issue issue)
        {
            bool status;
            try
            {
                Issue iss = db.Issues.Where(i => i.Id == issue.Id).FirstOrDefault();
                if (iss != null)
                {
                    iss.Summary = issue.Summary;
                    iss.Description = issue.Description;
                    iss.Priority = issue.Priority;
                    iss.Assignee = issue.Assignee;
                    iss.Type = issue.Type;
                    iss.ProjectId = issue.ProjectId;
                    db.SaveChanges();
                }
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            if (status)
                return "Successfully updated";
            return "Error";

        }

        public Issue_for_View getIssue(int id)
        {
            return ViewIssues(db.Issues.Where(i => i.Id == id).ToList()).First();
        }

        public List<Issue_for_View> getIssues(int projectId)
        {
            return ViewIssues(db.Issues.Where(i => i.ProjectId == projectId).ToList());

        }

        public int getProjectIdByName(string projectName)
        {
            return db.Projects.Where(p => p.Name == projectName).First().Id;
        }

        public List<Project> getProjects()
        {
            return db.Projects.ToList();
        }

        public List<Issue_for_View> ViewIssues(List<Issue> issues)
        {
            List<Issue_for_View> iss = new List<Issue_for_View>();

            foreach (var i in issues)
            {
                iss.Add(new Issue_for_View()
                {
                    Id = i.Id,
                    Summary = i.Summary,
                    Description = i.Description,
                    Priority = i.Priority.ToString(),
                    Type = i.Type.ToString(),
                    Assignee = i.Assignee,
                    ProjectName = db.Projects.Where(p => p.Id == i.ProjectId).Select(pr => pr.Name).First(),
                    ProjectId = i.ProjectId,
                    Types = new List<string>() { "New", "InProgress", "Done" },
                    Priorities = new List<string>() { "Trivial", "Low", "Medium", "High", "Critical" }
                });
            }
            return iss;
        }

        public string getUserRoleName(string email)
        {
            var roleId = db.Users.Where(u => u.Email == email).FirstOrDefault().Roles.First().RoleId;
            return db.Roles.Where(r => r.Id == roleId).FirstOrDefault().Name;
        }

        public Project addProject(Project project)
        {
            db.Projects.Add(project);
            db.SaveChanges();
            return db.Projects.Where(p => p.Name == project.Name).FirstOrDefault();
        }

        public List<User_for_View> getUsers()
        {
            List<User_for_View> users = new List<User_for_View>();
            foreach (AppUser user in db.Users)
            {
                users.Add(new User_for_View() { User = user, Role = getUserRoleName(user.Email) });
            }
            return users;
        }

        public string addUserToProject(int projectId, string email)
        {
            db.Users.Where(u => u.Email == email).First().ProjectId = projectId;
            db.SaveChanges();
            return $"User {email} was successfully added to project {projectId}";
        }
    }
}