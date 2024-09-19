namespace ForumApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_upvotes_downvotes_for_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TotalUpvotes", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "TotalDownvotes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TotalDownvotes");
            DropColumn("dbo.AspNetUsers", "TotalUpvotes");
        }
    }
}
