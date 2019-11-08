using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            } catch(Exception)
            {
                return "Error. Wrong data entry";
            }
            return "Successfully added"; 
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
            catch(Exception)
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

        public Issue_for_View convertIssue(Issue issue)
        {
            return new Issue_for_View()
            {
                Id = issue.Id,
                Summary = issue.Summary,
                Description = issue.Description,
                Priority = issue.Priority.ToString(),
                Type = issue.Type.ToString(),
                Assignee = issue.Assignee,
                ProjectName = db.Projects.Where(p => p.Id == issue.ProjectId).Select(pr => pr.Name).First(),
                ProjectId = issue.ProjectId,
                Types = new List<string>() { "New", "InProgress", "Done" },
                Priorities = new List<string>() { "Trivial", "Low", "Medium", "High", "Critical" }
            };
        }

        public Issues_for_Kanban sortIssues(int projectId)
        {
            Issues_for_Kanban sortIss = new Issues_for_Kanban();
            List<Issue> issues = new List<Issue>();
            issues = db.Issues.Where(i => i.ProjectId == projectId).ToList();
            foreach (var issue in issues)
            {
                switch (issue.Type)
                {
                    case Type.New:
                        {
                            sortIss.newIssues.Add(convertIssue(issue));
                            break;
                        }
                    case Type.InProgress:
                        {
                            sortIss.inprogressIssues.Add(convertIssue(issue));
                            break;
                        }
                    case Type.Done:
                        {
                            sortIss.doneIssues.Add(convertIssue(issue));
                            break;
                        }

                }
            }
            return sortIss;
        }

        public List<Issue_for_View> ViewIssues(List<Issue> issues)
        {
            List<Issue_for_View> iss = new List<Issue_for_View>();

            foreach(var i in issues)
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
                    Types = new List<string>() {"New", "InProgress", "Done" },
                    Priorities = new List<string>() {"Trivial", "Low", "Medium", "High", "Critical" }
                });
            }
            return iss;
        }
    }
}