namespace IssuesTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeIssue : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Issues", "Project_Id", "dbo.Projects");
            DropIndex("dbo.Issues", new[] { "Project_Id" });
            RenameColumn(table: "dbo.Issues", name: "Project_Id", newName: "ProjectId");
            AlterColumn("dbo.Issues", "ProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.Issues", "ProjectId");
            AddForeignKey("dbo.Issues", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Issues", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Issues", new[] { "ProjectId" });
            AlterColumn("dbo.Issues", "ProjectId", c => c.Int());
            RenameColumn(table: "dbo.Issues", name: "ProjectId", newName: "Project_Id");
            CreateIndex("dbo.Issues", "Project_Id");
            AddForeignKey("dbo.Issues", "Project_Id", "dbo.Projects", "Id");
        }
    }
}
