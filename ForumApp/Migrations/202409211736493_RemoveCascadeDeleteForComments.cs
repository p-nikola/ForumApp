namespace ForumApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCascadeDeleteForComments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments");
            AddForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments", "CommentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments");
            AddForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments", "CommentId", cascadeDelete: true);
        }
    }
}
