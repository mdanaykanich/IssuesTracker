﻿using System;
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
            db.Issues.Add(issue);
            db.SaveChanges();
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

        public List<Issue_for_View> getIssues(int projectId)
        {
            return ViewIssues(db.Issues.Where(i => i.ProjectId == projectId).ToList());
            
        }

        public List<Project> getProjects()
        {
            return db.Projects.ToList();
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
                    ProjectId = i.ProjectId
                });
            }
            return iss;
        }
    }
}