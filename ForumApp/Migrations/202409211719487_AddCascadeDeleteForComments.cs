namespace ForumApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCascadeDeleteForComments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments");
            AddForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments", "CommentId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments");
            AddForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments", "CommentId");
        }
    }
}
