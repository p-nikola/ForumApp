namespace ForumApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVotesTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PostVotes", "PostId", "dbo.Posts");
            DropForeignKey("dbo.PostVotes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.PostVotes", new[] { "UserId" });
            DropIndex("dbo.PostVotes", new[] { "PostId" });
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PostId = c.Int(),
                        CommentId = c.Int(),
                        IsUpvote = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VoteId)
                .ForeignKey("dbo.Comments", t => t.CommentId)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.CommentId);
            
            DropTable("dbo.PostVotes");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.PostVoteId);
            
            DropForeignKey("dbo.Votes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Votes", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Votes", "CommentId", "dbo.Comments");
            DropIndex("dbo.Votes", new[] { "CommentId" });
            DropIndex("dbo.Votes", new[] { "PostId" });
            DropIndex("dbo.Votes", new[] { "UserId" });
            DropTable("dbo.Votes");
            CreateIndex("dbo.PostVotes", "PostId");
            CreateIndex("dbo.PostVotes", "UserId");
            AddForeignKey("dbo.PostVotes", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PostVotes", "PostId", "dbo.Posts", "PostId", cascadeDelete: true);
        }
    }
}
