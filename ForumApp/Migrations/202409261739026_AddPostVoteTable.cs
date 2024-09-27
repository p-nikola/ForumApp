namespace ForumApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostVoteTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostVotes",
                c => new
                    {
                        PostVoteId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PostId = c.Int(nullable: false),
                        IsUpvote = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostVoteId)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostVotes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostVotes", "PostId", "dbo.Posts");
            DropIndex("dbo.PostVotes", new[] { "PostId" });
            DropIndex("dbo.PostVotes", new[] { "UserId" });
            DropTable("dbo.PostVotes");
        }
    }
}
