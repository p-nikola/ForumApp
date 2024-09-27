namespace ForumApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPostAndCommentVotes : DbMigration
    {
        public override void Up()
        {
            // Create the PostVotes table
            CreateTable(
                "dbo.PostVotes",
                c => new
                {
                    PostVoteId = c.Int(nullable: false, identity: true),
                    PostId = c.Int(nullable: false),
                    UserId = c.String(maxLength: 128),
                    VoteType = c.Int(nullable: false),
                    DateVoted = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.PostVoteId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.UserId);

            // Create the CommentVotes table
            CreateTable(
                "dbo.CommentVotes",
                c => new
                {
                    CommentVoteId = c.Int(nullable: false, identity: true),
                    CommentId = c.Int(nullable: false),
                    UserId = c.String(maxLength: 128),
                    VoteType = c.Int(nullable: false),
                    DateVoted = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.CommentVoteId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.CommentId)
                .Index(t => t.UserId);

            // Remove old columns related to votes in the Post and Comment tables, if applicable
            DropColumn("dbo.Comments", "Upvotes");
            DropColumn("dbo.Comments", "Downvotes");
            DropColumn("dbo.Posts", "Upvotes");
            DropColumn("dbo.Posts", "Downvotes");
        }

        public override void Down()
        {
            // Re-add columns to Posts and Comments in case of a rollback
            AddColumn("dbo.Comments", "Downvotes", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "Upvotes", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "Downvotes", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "Upvotes", c => c.Int(nullable: false));

            // Drop the CommentVotes and PostVotes tables in case of a rollback
            DropForeignKey("dbo.CommentVotes", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.CommentVotes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostVotes", "PostId", "dbo.Posts");
            DropForeignKey("dbo.PostVotes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.CommentVotes", new[] { "UserId" });
            DropIndex("dbo.CommentVotes", new[] { "CommentId" });
            DropIndex("dbo.PostVotes", new[] { "UserId" });
            DropIndex("dbo.PostVotes", new[] { "PostId" });
            DropTable("dbo.CommentVotes");
            DropTable("dbo.PostVotes");
        }
    }
}
