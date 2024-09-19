namespace ForumApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_upvotes_downvotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Upvotes", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "Downvotes", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "Upvotes", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "Downvotes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Downvotes");
            DropColumn("dbo.Posts", "Upvotes");
            DropColumn("dbo.Comments", "Downvotes");
            DropColumn("dbo.Comments", "Upvotes");
        }
    }
}
