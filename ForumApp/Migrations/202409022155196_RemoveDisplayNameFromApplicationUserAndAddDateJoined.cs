namespace ForumApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDisplayNameFromApplicationUserAndAddDateJoined : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DateJoined", c => c.DateTime(nullable: false));
            DropColumn("dbo.AspNetUsers", "DisplayName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DisplayName", c => c.String());
            DropColumn("dbo.AspNetUsers", "DateJoined");
        }
    }
}
